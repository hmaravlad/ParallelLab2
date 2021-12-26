using ParallelLab2.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelLab2.Messages
{
    class CancelParkingSpaceRequest
    {
        public readonly Car car;
        public CancelParkingSpaceRequest(Car car)
        {
            this.car = car;
        }
    }
}
