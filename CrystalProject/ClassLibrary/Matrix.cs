using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Resources;


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
                    if ((i == 0) || (j == 0) || (i == numberrows) || (j == numbercolumns))
                    {
                        if (boundary == 1)
                        {
                            matrix[i, j] = new Cell(1, -1, conditions);
                        }
                        else
                        {
                            matrix[i, j] = new Cell(1, -1, conditions);
                        }
                    }
                    else
                    {
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


       

    }
}
