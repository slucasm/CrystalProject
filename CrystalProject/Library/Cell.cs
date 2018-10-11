using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library
{
    class Cell
    {
        int phase_actual;
        int temperature_actual;
        Conditions conditions;
        int phase_future;
        int temperature_future;

        //int phase_neighb_up;
        //int phase_neighb_down;
        //int phase_neighb_right;
        //int phase_neighb_left;

        //int temperature_neighb_up;
        //int temperature_neighb_down;
        //int temperature_neighb_right;
        //int temperature_neighb_left;
        

        public Cell(int phase_actual,int temperature_actual,Conditions conditions)
        {
            this.phase_actual = phase_actual;
            this.temperature_actual = temperature_actual;
            this.conditions = conditions;
        }

        public int getTemperature()
        {
            return this.temperature_actual;
        }

        public int getPhase()
        {
            return this.phase_actual;
        }

        public void evolucionarphase(int phase_neigh_up, int phase_neighb_down, int phase_neighb_right,int phase_neighb_left)
        {

        }


    }
}
