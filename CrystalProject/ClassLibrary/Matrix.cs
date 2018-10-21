using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Resources;
using System.Windows.Shapes;
using System.Windows.Media;






namespace ClassLibrary
{
    public class Matriz
    {
        Cell[,] matrix;
        int numberrows;
        int numbercolumns;
        Conditions conditions;

        public Matriz(int numberrows, int numbercolumns, Conditions conditions)
        {
            this.numbercolumns = numbercolumns;
            this.numberrows = numberrows;
            this.conditions = conditions;
        }

        public DataTable createTable()
        {
            DataTable table = new DataTable();
            for (int contadorcolumns = 0; contadorcolumns < numbercolumns; contadorcolumns++)
            {
                //DataColumn dc = new DataColumn("id", typeof(int));
                table.Columns.Add();
            }
            for (int contadorrows = 0; contadorrows < numberrows; contadorrows++)
            {
                table.Rows.Add();
            }
            
            return table;
        }

        public void createMatrix()
        {
            this.matrix = new Cell[numberrows, numbercolumns];
            //return matrix;
        }
        //iniciamos la matriz con todas las celdas como líquido
        public void initialconditions(int boundary)
        {
            for (int i = 0; i < numberrows; i++)
            {
                for (int j = 0; j < numbercolumns; j++)
                {
                    if ((i == 0) || (j == 0) || (i == numberrows-1) || (j == numbercolumns-1)) //Only at boundaries
                    {
                        if (boundary == 1) //Boundary Tconstant T=0 (liquid)
                        {
                            matrix[i, j] = new Cell(1, 0, conditions); //Temperatura 0, liquid
                        }
                        else //Boundary=0 Boundary reflecting T=-1 (solid)
                        {
                            matrix[i, j] = new Cell(1, -1, conditions); //Temperatura -1, solid
                        }
                    }
                    else
                    {
                        //Error!: Select conditions please
                        matrix[i, j] = new Cell(1, -1, conditions);
                    }
                }
            }
            //return matrix;
        }
        public void initialSolid(int i, int j)
        {
            matrix[i, j].setSolid();
            //return matrix;
        }





        //Para la función que le dice los vecinos a las celdas solo calculamos las celdas que no están en la frontera!
        //Por eso empezamos con i = 1, j = 1, y hasta la longitud -1, de esta forma evitamos cammbiar los valores de la frontera.
        public void neighbours()//Cell[,] matrix)
        {
            for (int i = 1; i < numberrows-1; i++)
            {
                for (int j = 1; j < numbercolumns-1; j++)
                {
                    matrix[i, j].compute_phase_and_temperature(matrix[i, j + 1].getPhase(), matrix[i, j - 1].getPhase(), matrix[i - 1, j].getPhase(), matrix[i + 1, j].getPhase(), matrix[i, j+1].getTemperature(), matrix[i, j-1].getTemperature(), matrix[i-1, j].getTemperature(), matrix[i+1, j].getTemperature());
                }
            }
            //return matrix;
        }

        public void actualizar()
        {
            for (int i = 1; i < numberrows - 1; i++)
            {
                for (int j = 1; j < numbercolumns - 1; j++)
                {
                    matrix[i, j].actualizar();
                }
            }
        }

        public Tuple<double, double> getphaseandtemperature(int fila, int columna)
        {
            double phase = matrix[fila, columna].getPhase();
            double temp = matrix[fila, columna].getTemperature();
            Tuple<double, double> tuple = new Tuple<double, double>(phase,temp);
            return tuple;
        }

        public Rectangle[,] colorearphase(Rectangle[,] matrixrectangle_phase)
        {

            for (int i = 0; i < numberrows; i++)
            {
                for (int j = 0; j < numbercolumns; j++)
                {
                    double phase = matrix[i, j].getPhase();
                    if (phase > 0.95)
                    {
                        matrixrectangle_phase[i, j].Fill = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                    }
                    else if (phase < 0.95 && phase > 0.85)
                    {
                        matrixrectangle_phase[i, j].Fill = new SolidColorBrush(Color.FromRgb(205, 255, 255));
                    }
                    else if (phase < 0.85 && phase > 0.75)
                    {
                        matrixrectangle_phase[i, j].Fill = new SolidColorBrush(Color.FromRgb(155, 255, 250));
                    }
                    else if (phase < 0.75 && phase > 0.65)
                    {
                        matrixrectangle_phase[i, j].Fill = new SolidColorBrush(Color.FromRgb(105, 255, 255));
                    }
                    else if (phase < 0.65 && phase > 0.55)
                    {
                        matrixrectangle_phase[i, j].Fill = new SolidColorBrush(Color.FromRgb(55, 255, 255));
                    }
                    else if (phase < 0.55 && phase > 0.45)
                    {
                        matrixrectangle_phase[i, j].Fill = new SolidColorBrush(Color.FromRgb(5, 255, 255));
                    }
                    else if (phase < 0.45 && phase > 0.35)
                    {
                        matrixrectangle_phase[i, j].Fill = new SolidColorBrush(Color.FromRgb(0, 210, 255));
                    }
                    else if (phase < 0.35 && phase > 0.25)
                    {
                        matrixrectangle_phase[i, j].Fill = new SolidColorBrush(Color.FromRgb(0, 170, 255));
                    }
                    else if (phase < 0.25 && phase > 0.15)
                    {
                        matrixrectangle_phase[i, j].Fill = new SolidColorBrush(Color.FromRgb(0, 120, 255));
                    }
                    else if (phase < 0.15 && phase > 0.05)
                    {
                        matrixrectangle_phase[i, j].Fill = new SolidColorBrush(Color.FromRgb(0, 70, 250));
                    }
                    else
                    {
                        matrixrectangle_phase[i, j].Fill = new SolidColorBrush(Color.FromRgb(0, 20, 255));
                    }

                }
            }
            return matrixrectangle_phase;
        }


        public Rectangle[,] colortemperature(Rectangle[,] matrixrectangle_temperature)
        {

            for (int i = 0; i < numberrows; i++)
            {
                for (int j = 0; j < numbercolumns; j++)
                {
                    double temperature = matrix[i, j].getTemperature();
                    if (temperature < -0.95)
                    {
                        matrixrectangle_temperature[i, j].Fill = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                    }
                    else if (temperature > -0.95 && temperature < -0.85)
                    {
                        matrixrectangle_temperature[i, j].Fill = new SolidColorBrush(Color.FromRgb(255, 230, 230));
                    }
                    else if (temperature > -0.85 && temperature < -0.75)
                    {
                        matrixrectangle_temperature[i, j].Fill = new SolidColorBrush(Color.FromRgb(255, 204, 204));
                    }
                    else if (temperature > -0.75 && temperature < -0.65)
                    {
                        matrixrectangle_temperature[i, j].Fill = new SolidColorBrush(Color.FromRgb(255, 179, 179));
                    }
                    else if (temperature > -0.65 && temperature < -0.55)
                    {
                        matrixrectangle_temperature[i, j].Fill = new SolidColorBrush(Color.FromRgb(255, 153, 153));
                    }
                    else if (temperature > -0.55 && temperature < -0.45)
                    {
                        matrixrectangle_temperature[i, j].Fill = new SolidColorBrush(Color.FromRgb(255, 128, 128));
                    }
                    else if (temperature > -0.45 && temperature < -0.35)
                    {
                        matrixrectangle_temperature[i, j].Fill = new SolidColorBrush(Color.FromRgb(255, 102, 102));
                    }
                    else if (temperature > -0.35 && temperature < -0.25)
                    {
                        matrixrectangle_temperature[i, j].Fill = new SolidColorBrush(Color.FromRgb(255, 77, 77));
                    }
                    else if (temperature > -0.25 && temperature < -0.15)
                    {
                        matrixrectangle_temperature[i, j].Fill = new SolidColorBrush(Color.FromRgb(255, 51, 51));
                    }
                    else if (temperature > -0.15 && temperature < -0.05)
                    {
                        matrixrectangle_temperature[i, j].Fill = new SolidColorBrush(Color.FromRgb(255, 26, 26));
                    }
                    else
                    {
                        matrixrectangle_temperature[i, j].Fill = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                    }

                }
            }
            return matrixrectangle_temperature;
        }


    

    }
}
