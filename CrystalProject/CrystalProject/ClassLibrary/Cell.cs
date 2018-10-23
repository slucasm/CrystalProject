using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;




namespace ClassLibrary
{
    public class Cell
    {
        double phase_actual;
        double temperature_actual;

        Conditions conditions;

        double phase_future;
        double temperature_future;
        SolidColorBrush color_phase,color_temp;

        public Cell(double phase_actual, double temperature_actual, Conditions conditions)
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

        public SolidColorBrush getColorphase()
        {
            return this.color_phase;
        }

        public SolidColorBrush getColortemp()
        {
            return this.color_temp;
        }

        public int isSolid()
        {
            if (phase_actual < 0.15)
            {
                return 1;
            }
            else
            {
                return 0;
            }
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
            double temperature_time = temperature_laplacian - (1 / (conditions.getdelta())) * (30 * phase_actual * phase_actual - 60 * phase_actual * phase_actual * phase_actual + 30 * phase_actual * phase_actual * phase_actual * phase_actual) * phase_time;

            this.phase_future = phase_actual + phase_time * conditions.getdelta_time();
            this.temperature_future = temperature_actual + temperature_time * conditions.getdelta_time();
        }

        public void actualizar()
        {
            temperature_actual = temperature_future;
            temperature_future = 0;
            phase_actual = phase_future;
            phase_future = 0;

        }
        public void colear_phase()
        {
            
            if (phase_actual > 0.95)
            {
                color_phase = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            }
            else if (phase_actual < 0.95 && phase_actual > 0.85)
            {
                color_phase = new SolidColorBrush(Color.FromRgb(205, 255, 255));
            }
            else if (phase_actual < 0.85 && phase_actual > 0.75)
            {
                color_phase = new SolidColorBrush(Color.FromRgb(155, 255, 250));
            }
            else if (phase_actual < 0.75 && phase_actual > 0.65)
            {
                color_phase = new SolidColorBrush(Color.FromRgb(105, 255, 255));
            }
            else if (phase_actual < 0.65 && phase_actual > 0.55)
            {
                color_phase = new SolidColorBrush(Color.FromRgb(55, 255, 255));
            }
            else if (phase_actual < 0.55 && phase_actual > 0.45)
            {
                color_phase = new SolidColorBrush(Color.FromRgb(5, 255, 255));
            }
            else if (phase_actual < 0.45 && phase_actual > 0.35)
            {
                color_phase = new SolidColorBrush(Color.FromRgb(0, 210, 255));
            }
            else if (phase_actual < 0.35 && phase_actual > 0.25)
            {
                color_phase = new SolidColorBrush(Color.FromRgb(0, 170, 255));
            }
            else if (phase_actual < 0.25 && phase_actual > 0.15)
            {
                color_phase = new SolidColorBrush(Color.FromRgb(0, 120, 255));
            }
            else if (phase_actual < 0.15 && phase_actual > 0.05)
            {
                color_phase = new SolidColorBrush(Color.FromRgb(0, 70, 250));
            }
            else
            {
                color_phase = new SolidColorBrush(Color.FromRgb(0, 20, 255));
            }

        }

        public void colorear_temp()
        {
            if (temperature_actual < -0.95)
            {
                color_temp = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            }
            else if (temperature_actual > -0.95 && temperature_actual < -0.85)
            {
                color_temp = new SolidColorBrush(Color.FromRgb(255, 230, 230));
            }
            else if (temperature_actual > -0.85 && temperature_actual < -0.75)
            {
                color_temp = new SolidColorBrush(Color.FromRgb(255, 204, 204));
            }
            else if (temperature_actual > -0.75 && temperature_actual < -0.65)
            {
                color_temp = new SolidColorBrush(Color.FromRgb(255, 179, 179));
            }
            else if (temperature_actual > -0.65 && temperature_actual < -0.55)
            {
                color_temp = new SolidColorBrush(Color.FromRgb(255, 153, 153));
            }
            else if (temperature_actual > -0.55 && temperature_actual < -0.45)
            {
                color_temp = new SolidColorBrush(Color.FromRgb(255, 128, 128));
            }
            else if (temperature_actual > -0.45 && temperature_actual < -0.35)
            {
                color_temp = new SolidColorBrush(Color.FromRgb(255, 102, 102));
            }
            else if (temperature_actual > -0.35 && temperature_actual < -0.25)
            {
                color_temp = new SolidColorBrush(Color.FromRgb(255, 77, 77));
            }
            else if (temperature_actual > -0.25 && temperature_actual < -0.15)
            {
                color_temp = new SolidColorBrush(Color.FromRgb(255, 51, 51));
            }
            else if (temperature_actual > -0.15 && temperature_actual < -0.05)
            {
                color_temp = new SolidColorBrush(Color.FromRgb(255, 26, 26));
            }
            else
            {
                color_temp = new SolidColorBrush(Color.FromRgb(255, 0, 0));
            }
        }

    }
}
