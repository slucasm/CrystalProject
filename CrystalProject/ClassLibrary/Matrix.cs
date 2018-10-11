using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library
{
    public class Matrix
    {
        Cell[,] matrix;
        int numberrows;
        int numbercolumns;

        public Cell[,] createMatrix(int numberrows, int numbercolumns)
        {
            this.matrix = new Cell[numberrows, numbercolumns];
            this.numbercolumns = numbercolumns;
            this.numberrows = numberrows;
            return matrix;
        }

        public Cell[,] neighbours(Cell[,] matrix)
        {
            int row = 0;
            numberrows = matrix.GetLength(0);
            numbercolumns = matrix.GetLength(1);
            while (row < numberrows)
            {
                int column = 0;
                int counter = 0;
                while (column < numbercolumns)
                { 
                
                
                
                
                }
            
            
            }


        }

       

    }
}
