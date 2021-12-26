using Akka.Actor;
using ParralelLab2.Entities;
using ParralelLab2.Messages;
using ParralelLab2.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParralelLab2.Actors
{
    class CarActor : ReceiveActor
    {
        readonly Car car;
        readonly IActorRef parkingLotProviderActor;
        private IActorRef currentParkingLot;
        private readonly ITimeSecondsProvider waitingTime;
        private readonly ITimeSecondsProvider stayingTime;
        private bool isParked = false;
        private ICancelable waiting = null;

        public CarActor(uint id, IActorRef parkingLotProviderActor, ITimeSecondsProvider waitingTime, ITimeSecondsProvider stayingTime)
        {
            this.parkingLotProviderActor = parkingLotProviderActor;
            this.waitingTime = waitingTime;
            this.stayingTime = stayingTime;
            car = new Car(id);

            Receive<MostFreeParkingLot>(msg =>
            {
                Console.WriteLine($"{DateTime.Now} - car: {car.id} - got address of parking lot, trying to park");
                currentParkingLot = msg.parkingLot;
                GoToParkingLot();
            });

            Receive<ParkingPlaceRequestAccepted>(msg =>
            {
                Console.WriteLine($"{DateTime.Now} - car: {car.id} - parked");
                OnParkingPlaceRequestAccepted();
            });

            Receive<LeaveRequestAccepted>(msg =>
            {
                Console.WriteLine($"{DateTime.Now} - car: {car.id} - left parking lot");
                OnLeaveRequestAccepted();
            });

            Receive<CancelRequestAccepted>(msg =>
            {
                Console.WriteLine($"{DateTime.Now} - car: {car.id} - waiting too long, trying to find new parking lot");
                OnCancelRequestAccepted();
            });

            parkingLotProviderActor.Tell(new RequestParkingLot());
        }

        private void GoToParkingLot()
        {
            currentParkingLot.Tell(new RequestParkingSpace(car));
            var waitingFor = TimeSpan.FromSeconds(waitingTime.getTime());

            waiting = Context.System.Scheduler.ScheduleTellOnceCancelable(
                waitingFor,
                currentParkingLot,
                new CancelParkingSpaceRequest(car),
                Self
            );
        }

        private void OnParkingPlaceRequestAccepted()
        {
            isParked = true;
            if (waiting != null)
            {
                waiting.Cancel();
            }

            var stayingFor = TimeSpan.FromSeconds(stayingTime.getTime());

            Context.System.Scheduler.ScheduleTellOnce(
                stayingFor,
                currentParkingLot,
                new LeaveRequest(car),
                Self
            );
        }

        private void OnLeaveRequestAccepted()
        {
            isParked = false;
            parkingLotProviderActor.Tell(new RequestParkingLot());
        }

        private void OnCancelRequestAccepted()
        {
            parkingLotProviderActor.Tell(new RequestParkingLot());
        }
    }
}
