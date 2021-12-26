using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelLab2.Utils
{
    class StaticTimeProvider: ITimeSecondsProvider
    {
        readonly uint time;

        public StaticTimeProvider(uint time)
        {
            this.time = time;
        }

        public uint getTime()
        {
            return time;
        }
    }
}
