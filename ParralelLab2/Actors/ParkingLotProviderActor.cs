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
    class ParkingLotProviderActor : ReceiveActor
    {
        private readonly List<IActorRef> parkingLotActors;
        private List<uint> freePlaceNumbers;

        public ParkingLotProviderActor(List<IActorRef> parkingLotActors, ITimeSecondsProvider requestPeriodProvider)
        {
            this.parkingLotActors = parkingLotActors;
            freePlaceNumbers = new List<uint>(new uint[parkingLotActors.Count]);
            foreach (IActorRef parkingLotActor in parkingLotActors)
            {
                var requestPeriod = TimeSpan.FromSeconds(requestPeriodProvider.getTime());

                Context.System.Scheduler.ScheduleTellRepeatedly(
                    TimeSpan.FromSeconds(0),
                    requestPeriod,
                    parkingLotActor,
                    new RequestFreeParkingPlaceNumber(),
                    Self
                );

                parkingLotActor.Tell(new RequestFreeParkingPlaceNumber());
            }

            Receive<ReplyFreeParkingPlaceNumber>(msg =>
            {
                OnReplyFreeParkingPlaceNumber(msg.freeParkingPlaceNumber, Sender);
            });

            Receive<RequestParkingLot>(msg =>
            {
                OnRequestParkingLot(Sender);
            });
        }

        void OnRequestParkingLot(IActorRef parkingLot)
        {
            var minValue = freePlaceNumbers.Min();
            var index = freePlaceNumbers.FindIndex(x => x == minValue);
            parkingLot.Tell(new MostFreeParkingLot(parkingLotActors[index]));
        }

        void OnReplyFreeParkingPlaceNumber(uint freeParkingPlaceNumber, IActorRef parkingLot)
        {
            var index = parkingLotActors.FindIndex(x => x == parkingLot);
            freePlaceNumbers[index] = freeParkingPlaceNumber;
        }
    }
}
