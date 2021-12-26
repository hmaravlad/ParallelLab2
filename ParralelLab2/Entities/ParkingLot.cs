using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelLab2.Entities
{
    class ParkingLot
    {
        readonly uint placeNumber;

        HashSet<Car> parkedCars = new HashSet<Car>();
        LinkedList<Car> queue = new LinkedList<Car>();
        public ParkingLot(uint placeNumber)
        {
            this.placeNumber = placeNumber;
        }

        public bool HasFreePlace()
        {
            return parkedCars.Count < placeNumber;
        }

        public void AddCar(Car car)
        {
            if (HasFreePlace())
            {
                parkedCars.Add(car);
            }
            else
            {
                throw new InvalidOperationException("Parking lot is full");
            }
        }

        public void RemoveCar(Car car)
        {
            parkedCars.Remove(car);
        }

        public void EnqueueCar(Car car)
        {
            queue.AddFirst(car);
        }

        public Car DequeueCar()
        {
            Car car = queue.Last.Value;
            queue.RemoveLast();
            return car;
        }

        public void RemoveCarFromQueue(Car car)
        {
            queue.Remove(car);
        }

        public bool IsQueueEmpty()
        {
            return queue.Count == 0;
        }

        public uint getFreeSpace()
        {
            return (uint)(placeNumber - queue.Count);
        }
    }
}
