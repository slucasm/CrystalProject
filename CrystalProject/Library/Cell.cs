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


        public Cell(int phase_actual,int temperature_actual)
        {
            this.phase_actual = phase_actual;
            this.temperature_actual = temperature_actual;
        }

        public int getTemperature()
        {
            return this.temperature_actual;
        }

        public int getPhase()
        {
            return this.phase_actual;
        }


    }
}
