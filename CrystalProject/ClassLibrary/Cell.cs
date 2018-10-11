using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ClassLibrary
{
    public class Cell
    {
        double phase_actual;
        double temperature_actual;

        Conditions conditions;

        double phase_future;
        double temperature_future;

        //double phase_neighb_up;
        //double phase_neighb_down;
        //double phase_neighb_right;
        //double phase_neighb_left;

        //double temperature_neighb_up;
        //double temperature_neighb_down;
        //double temperature_neighb_right;
        //double temperature_neighb_left;
        

        public Cell(double phase_actual,double temperature_actual,Conditions conditions)
        {
            this.phase_actual = phase_actual;
            this.temperature_actual = temperature_actual;
            this.conditions = conditions;
        }

        public double getTemperature()
        {
            return this.temperature_actual;
        }

        public double getPhase()
        {
            return this.phase_actual;
        }


        public void setSolid()
        {
            this.temperature_actual = 0;
            this.phase_actual = 0;
        }


        public void compute_phase_and_temperature(double phase_neighb_right, double phase_neighb_left, double phase_neighb_up, double phase_neighb_down, double temperature_neighb_right, double temperature_neighb_left, double temperature_neighb_up, double temperature_neighb_down)
        {
            double phase_square_x = (phase_neighb_right + phase_neighb_left - 2 * phase_actual) / (conditions.getdelta_x() * conditions.getdelta_x());
            double phase_square_y = (phase_neighb_up + phase_neighb_down - 2 * phase_actual) / (conditions.getdelta_y() * conditions.getdelta_y());
            double temperature_square_x = (temperature_neighb_right + temperature_neighb_left - 2 * temperature_actual) / (conditions.getdelta_x() * conditions.getdelta_x());
            double temperature_square_y = (temperature_neighb_up + temperature_neighb_down - 2 * temperature_actual) / (conditions.getdelta_y() * conditions.getdelta_y());

            double phase_laplacian = phase_square_x + phase_square_y;
            double temperature_laplacian = temperature_square_x + temperature_square_y;

            double phase_time = 1 / (conditions.getepsylon() * conditions.getepsylon() * conditions.getM()) * (phase_actual * (1 - phase_actual) * (phase_actual - 0.5 + 30 * conditions.getepsylon() * conditions.getalpha() * conditions.getdelta() * temperature_actual * phase_actual * (1 - phase_actual)) + conditions.getepsylon() * conditions.getepsylon() * phase_laplacian);
            double temperature_time = temperature_laplacian-(1/(conditions.getdelta()))*(30*phase_actual*phase_actual-60*phase_actual*phase_actual*phase_actual+30*phase_actual*phase_actual*phase_actual*phase_actual)*phase_time;

            this.phase_future = phase_actual + phase_time * conditions.getdelta_time();
            this.temperature_future = temperature_actual + temperature_time * conditions.getdelta_time();
        }




    }
}
