using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ClassLibrary
{
    public class Conditions
    {
        //Atributos
        double delta_x;
        double delta_y;
        double epsylon;
        double M;
        double delta_time;
        double delta;
        double alpha;
        string name;//el nombre lo utilizamos solo para cuando guardamos, pero el user no puede cambiarlo

        public Conditions(double delta_x, double delta_y, double epsylon, double M, double delta_time, double delta, double alpha,string name)
        {
            this.delta_x = delta_x;
            this.delta_y = delta_y;
            this.epsylon = epsylon;
            this.M = M;
            this.delta_time = delta_time;
            this.delta = delta;
            this.alpha = alpha;
            this.name = name;

        }

        //Get's y set's
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

       
        public string getname()
        {
            return name;
        }


    }
}
