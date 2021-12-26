using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParralelLab2.Messages
{
    class MostFreeParkingLot
    {
        public readonly IActorRef parkingLot;
        public MostFreeParkingLot(IActorRef parkingLot)
        {
            this.parkingLot = parkingLot;
        }
    }
}
