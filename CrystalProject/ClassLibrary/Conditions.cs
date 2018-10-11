using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library
{
    class Conditions
    {
        int delta_x;
        int delta_y;
        int epsylon;
        int B;
        int M;
        int delta_time;
        int delta;

        public Conditions(int delta_x, int delta_y, int epsylon, int B, int M, int delta_time, int delta)
        {
            this.delta_x = delta_x;
            this.delta_y = delta_y;
            this.epsylon = epsylon;
            this.B = B;
            this.M = M;
            this.delta_time = delta_time;
            this.delta = delta;
        }

        public int getdelta_x()
        {
            return delta_x;
        }
        public int getdelta_y()
        {
            return delta_y;
        }
        public int getepsylon()
        {
            return epsylon;
        }
        public int getB()
        {
            return B;
        }
        public int getM()
        {
            return M;
        }
        public int getdelta_time()
        {
            return delta_time;
        }
        public int getdelta()
        {
            return delta;
        }


    }
}
