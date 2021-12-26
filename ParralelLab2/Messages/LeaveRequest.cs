using ParralelLab2.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParralelLab2.Messages
{
    class LeaveRequest
    {
        public readonly Car car;
        public LeaveRequest(Car car)
        {
            this.car = car;
        }
    }
}
