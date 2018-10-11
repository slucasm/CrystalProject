using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public Cell[,] createMatrix()
        {
            this.matrix = new Cell[numberrows, numbercolumns];
            return matrix;
        }
        //iniciamos la matriz con todas las celdas como líquido
        public Cell[,] initialconditions()
        {
            for (int i = 0; i < numberrows; i++)
            {
                for (int j = 0; j < numbercolumns; j++)
                {
                    matrix[i, j] = new Cell(1, -1, conditions);
                }
            }
            return matrix;
        }
        public Cell[,] initialSolid(int i, int j)
        {
            matrix[i, j].setSolid();
            return matrix;
        }



        //Para la función que le dice los vecinos a las celdas solo calculamos las celdas que no están en la frontera!
        //Por eso empezamos con i = 1, j = 1, y hasta la longitud -1, de esta forma evitamos cammbiar los valores de la frontera.
        public Cell[,] neighbours()//Cell[,] matrix)
        {
            for (int i = 1; i < numberrows-1; i++)
            {
                for (int j = 1; j < numbercolumns-1; j++)
                {
                    matrix[i, j].compute_phase_and_temperature(matrix[i, j + 1].getPhase(), matrix[i, j - 1].getPhase(), matrix[i - 1, j].getPhase(), matrix[i + 1, j].getPhase(), matrix[i, j+1].getTemperature(), matrix[i, j-1].getTemperature(), matrix[i-1, j].getTemperature(), matrix[i+1, j].getTemperature());
                }
            }
            return matrix;
        }

       

    }
}
