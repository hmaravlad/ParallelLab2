using ParallelLab2.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelLab2.Messages
{
    class RequestParkingSpace
    {
        public readonly Car car;
        public RequestParkingSpace(Car car)
        {
            this.car = car;
        }
    }
}
