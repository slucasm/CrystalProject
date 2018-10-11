using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClassLibrary;

namespace Cristal
{
    class Class1
    {

        static void Main(string[] args)
    {
        
        Conditions conditions = new Conditions(0.005, 0.005, 0.005, 20, 5e-6, 0.5, 400);
        Matriz matrix = new Matriz(11, 11, conditions);
        matrix.createMatrix();
        matrix.initialconditions();
        matrix.initialSolid(5, 5);
        Cell[,] prueba = matrix.neighbours();
        Console.ReadLine();
    }
        


    }
}
