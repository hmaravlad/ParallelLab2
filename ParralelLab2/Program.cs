using Akka.Actor;
using ParralelLab2.Actors;
using ParralelLab2.Utils;
using System;
using System.Collections.Generic;


namespace ParralelLab2
{
    class Program
    {
        static void Main(string[] args)
        {
            ActorSystem system = ActorSystem.Create("Parking");

            var parkingLots = createParkingLotActors(2, 2, system);
            var parkingLotProvider = system.ActorOf(Props.Create(() => new ParkingLotProviderActor(parkingLots, new StaticTimeProvider(2))));
            createCarActors(6, 20, system, parkingLotProvider);
            Console.ReadLine();
        }

        static private List<IActorRef> createParkingLotActors(uint n, uint capacity, ActorSystem system)
        {
            List<IActorRef> res = new List<IActorRef>();
            for (uint i = 0; i < n; i++)
            {
                res.Add(system.ActorOf(Props.Create(() => new ParkingLotActor(capacity))));
            }
            return res;
        }

        static private void createCarActors(uint n, uint time, ActorSystem system, IActorRef parkingLotProviderActor)
        {
            for (uint i = 0; i < n; i++)
            {
                system.ActorOf(Props.Create(() => new CarActor(i, parkingLotProviderActor, new StaticTimeProvider(time + i), new StaticTimeProvider(time - 8 + i))));
            }
        }
    }
}
