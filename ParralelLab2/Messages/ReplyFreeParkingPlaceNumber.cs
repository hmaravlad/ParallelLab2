using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelLab2.Messages
{
    public class ReplyFreeParkingPlaceNumber
    {
        public readonly uint freeParkingPlaceNumber;

        public ReplyFreeParkingPlaceNumber(uint freeParkingPlaceNumber)
        {
            this.freeParkingPlaceNumber = freeParkingPlaceNumber;
        }
    }
}
