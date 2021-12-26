using ParralelLab2.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParralelLab2.Messages
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
