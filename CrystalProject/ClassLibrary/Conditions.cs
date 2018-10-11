using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ClassLibrary
{
    public class Conditions
    {
        double delta_x;
        double delta_y;
        double epsylon;
        //double B;
        double M;
        double delta_time;
        double delta;
        double alpha;

        public Conditions(double delta_x, double delta_y, double epsylon, double M, double delta_time, double delta, double alpha)
        {
            this.delta_x = delta_x;
            this.delta_y = delta_y;
            this.epsylon = epsylon;
            //this.B = B;
            this.M = M;
            this.delta_time = delta_time;
            this.delta = delta;
            this.alpha = alpha;

        }


        public double getalpha()
        {
            return this.alpha;
        }
        public double getdelta_x()
        {
            return this.delta_x;
        }
        public double getdelta_y()
        {
            return delta_y;
        }
        public double getepsylon()
        {
            return epsylon;
        }
        //public double getB()
        //{
        //    return B;
        //}
        public double getM()
        {
            return M;
        }
        public double getdelta_time()
        {
            return delta_time;
        }
        public double getdelta()
        {
            return delta;
        }


    }
}
