using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Resources;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows;






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
                    if ((i == 0) || (j == 0) || (i == numberrows - 1) || (j == numbercolumns - 1)) //Only at boundaries
                    {
                        if (boundary == 2) //Boundary Tconstant T=-1 (liquid)
                        {
                            matrix[i, j] = new Cell(1, -1, conditions); //Temperatura -1, liquid
                        }
                        else if (boundary == 1) //Boundary Tconstant T=0 (solid)
                        {
                            matrix[i, j] = new Cell(0, 0, conditions); //Temperatura 0, solid
                        }
                        else //Boundary=0 Boundary reflecting
                        {
                            //matrix[];

                            //matrix[i, j] = new Cell(1, -1, conditions); //Temperatura -1, solid
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
            for (int i = 1; i < numberrows - 1; i++)
            {
                for (int j = 1; j < numbercolumns - 1; j++)
                {
                    matrix[i, j].compute_phase_and_temperature(matrix[i, j + 1].getPhase(), matrix[i, j - 1].getPhase(), matrix[i - 1, j].getPhase(), matrix[i + 1, j].getPhase(), matrix[i, j + 1].getTemperature(), matrix[i, j - 1].getTemperature(), matrix[i - 1, j].getTemperature(), matrix[i + 1, j].getTemperature());
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
            Tuple<double, double> tuple = new Tuple<double, double>(phase, temp);
            return tuple;
        }

        public Rectangle[,] colorearphase(Rectangle[,] matrixrectangle_phase)
        {
            for (int i = 0; i < numberrows; i++)
            {
                for (int j = 0; j < numbercolumns; j++)
                {
                    matrix[i, j].colear_phase();
                    matrixrectangle_phase[i,j].Fill = matrix[i, j].getColorphase();               
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
                    matrix[i, j].colorear_temp();
                    matrixrectangle_temperature[i, j].Fill = matrix[i, j].getColortemp();
                }
            }
            return matrixrectangle_temperature;
        }


        public List<Point> contarsolids(List<Point> listPoint_solids)
        {
            int contadorsolid = 0;
            int contador = listPoint_solids.Count;
            for (int i = 0; i < numberrows; i++)
            {
                for (int j = 0; j < numbercolumns; j++)
                {
                    contadorsolid += matrix[i, j].isSolid();
                }
            }
            listPoint_solids.Add(new Point(contador,contadorsolid));
            return listPoint_solids;
        }

        public List<Point> avgtemp(List<Point> listPoint_avgtemp)
        {
            double sumatemp = 0;
            int n = 0;
            int contador = listPoint_avgtemp.Count;
            for (int i = 0; i < numberrows; i++)
            {
                for (int j = 0; j < numbercolumns; j++)
                {
                    sumatemp += matrix[i, j].getTemperature();
                    n++;
                }
            }
            double avgtemp = sumatemp / n;
            listPoint_avgtemp.Add(new Point(contador, avgtemp));
            return listPoint_avgtemp;
        }
        




    }
}
