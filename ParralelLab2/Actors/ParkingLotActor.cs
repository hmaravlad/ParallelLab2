using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using ParallelLab2.Entities;
using ParallelLab2.Messages;
using ParallelLab2.Utils;

namespace ParallelLab2.Actors
{
    class ParkingLotActor : ReceiveActor
    {
        ParkingLot parkingLot;

        Dictionary<uint, IActorRef> carActors = new Dictionary<uint, IActorRef>();

        public ParkingLotActor(uint placeNumber = 1)
        {
            parkingLot = new ParkingLot(placeNumber);

            Receive<RequestParkingSpace>(msg =>
            {
                OnRequestParkingSpace(msg.car, Sender);
            });

            Receive<LeaveRequest>(msg =>
            {
                OnLeaveRequest(msg.car);
            });

            Receive<CancelParkingSpaceRequest>(msg =>
            {
                OnCancelParkingSpaceRequest(msg.car);
            });

            Receive<RequestFreeParkingPlaceNumber>(msg =>
            {
                OnRequestFreeParkingSpace(Sender);
            });
        }

        private void OnRequestParkingSpace(Car car, IActorRef carActor)
        {
            carActors[car.id] = carActor;
            if (parkingLot.HasFreePlace())
            {
                parkingLot.AddCar(car);
                carActor.Tell(new ParkingPlaceRequestAccepted());
            }
            else
            {
                parkingLot.EnqueueCar(car);
            }
        }

        private void OnLeaveRequest(Car car)
        {
            parkingLot.RemoveCar(car);
            carActors[car.id].Tell(new LeaveRequestAccepted());

            if (!parkingLot.IsQueueEmpty())
            {
                Car queuedCar = parkingLot.DequeueCar();
                parkingLot.AddCar(queuedCar);
                carActors[queuedCar.id].Tell(new ParkingPlaceRequestAccepted());
            }
        }

        private void OnCancelParkingSpaceRequest(Car car)
        {
            parkingLot.RemoveCarFromQueue(car);
            carActors[car.id].Tell(new CancelRequestAccepted());
        }

        private void OnRequestFreeParkingSpace(IActorRef sender)
        {
            var freeSpace = parkingLot.getFreeSpace();
            sender.Tell(new ReplyFreeParkingPlaceNumber(freeSpace));
        }
    }
}
