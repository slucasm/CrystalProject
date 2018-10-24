using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Resources;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows;
using Microsoft.Win32;
using System.IO;






namespace ClassLibrary
{
    public class Matriz
    {
        //Atributos
        Cell[,] matrix;
        int numberrows;
        int numbercolumns;
        Conditions conditions;
        Boolean reflecting = false;
        String boundary;

        public Matriz(int numberrows, int numbercolumns, Conditions conditions)
        {
            this.numbercolumns = numbercolumns;
            this.numberrows = numberrows;
            this.conditions = conditions;
        }

        //public DataTable createTable()
        //{
        //    DataTable table = new DataTable();
        //    for (int contadorcolumns = 0; contadorcolumns < numbercolumns; contadorcolumns++)
        //    {
        //        //DataColumn dc = new DataColumn("id", typeof(int));
        //        table.Columns.Add();
        //    }
        //    for (int contadorrows = 0; contadorrows < numberrows; contadorrows++)
        //    {
        //        table.Rows.Add();
        //    }

        //    return table;
        //}

        //Inicializamos la matriz
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
                    if ((i == 0) || (j == 0) || (i == numberrows - 1) || (j == numbercolumns - 1)) //Solo en la frontera
                    {
                        if (boundary == 2) //Boundary Tconstant T=-1 (liquid)
                        {
                            matrix[i, j] = new Cell(1, -1, conditions); //Temperatura -1, liquid
                            reflecting = false;
                            this.boundary = "Liquid";
                        }
                        else if (boundary == 1) //Boundary Tconstant T=0 (solid)
                        {
                            matrix[i, j] = new Cell(0, 0, conditions); //Temperatura 0, solid
                            reflecting = false;
                            this.boundary = "Solid";
                        }
                        else //Boundary=0 Boundary reflecting
                        {
                            matrix[i, j] = new Cell(1, -1, conditions); //Temperatura -1, liquid
                            reflecting = true;
                            this.boundary = "Reflecting";
                        }
                    }
                    else //Fuera de la frontera llenamos todas las celdas de líquido
                    {
                        matrix[i, j] = new Cell(1, -1, conditions);
                    }
                }
            }
            //return matrix;
        }

        //Seleccionamos una sola celda como sólida, siempre será la celda del medio del grid
        public void initialSolid(int i, int j)
        {
            matrix[i, j].setSolid();
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

        //Actualizamos la matriz, asignamos los valores futuros de cada cell a actuales
        //Separamos el actualizar la frontera y el interior
        public void actualizar()
        {
            //Actualizar el interior del grid (sin la frontera)
            for (int i = 1; i < numberrows - 1; i++)
            {
                for (int j = 1; j < numbercolumns - 1; j++)
                {
                    matrix[i, j].actualizar();
                }
            }
            actualizarboundary(); //actualizar frontera
        }

        //Función para actualizar frontera
        public void actualizarboundary()
        {
            if (reflecting == false) //Solo actualizaremos la frontera en caso que hayamos seleccionado frontera reflectante
            {

            }
            else //actualizamos frontera reflectante
            {
                for (int i = 0; i < numberrows; i++)
                {
                    for (int j = 0; j < numbercolumns; j++)
                    {
                        //Conjunto de if's para ir recorriendo toda la frontera
                        if (i == 0 & j == 0) //esquina arriba izquierda
                        {
                            matrix[i, j].setPhase(matrix[i + 1, j + 1].getPhase());
                            matrix[i, j].setTemp(matrix[i + 1, j + 1].getTemperature());
                        }
                        else if (i == 0 && j !=0 && j !=numbercolumns-1) //primera fila (sin esquinas)
                        {
                            matrix[i, j].setPhase(matrix[i + 1, j].getPhase());
                            matrix[i, j].setTemp(matrix[i + 1, j].getTemperature());
                        }
                        else if (j == 0 && i!=0 && i!=numberrows-1) //primera columna (sin esquinas)
                        {
                            matrix[i, j].setPhase(matrix[i, j + 1].getPhase());
                            matrix[i, j].setTemp(matrix[i, j + 1].getTemperature());
                        }
                        else if (i == numberrows-1 && j != 0 && j != numbercolumns-1)//última fila (sin esquinas)
                        {
                            matrix[i, j].setPhase(matrix[i -1, j].getPhase());
                            matrix[i, j].setTemp(matrix[i - 1, j].getTemperature());
                        }
                        else if (j == numbercolumns-1 && i != 0 && i != numberrows-1)//última columna (sin esquinas)
                        {
                            matrix[i, j].setPhase(matrix[i, j - 1].getPhase());
                            matrix[i, j].setTemp(matrix[i, j - 1].getTemperature());
                        }
                        else if (i == 0 && j == numbercolumns-1) //esquina arriba derecha
                        {
                            matrix[i, j].setPhase(matrix[i + 1, j - 1].getPhase());
                            matrix[i, j].setTemp(matrix[i + 1, j - 1].getTemperature());
                        }
                        else if (j == 0 && i == numberrows-1) //esquina abajo izquierda
                        {
                            matrix[i, j].setPhase(matrix[i - 1, j + 1].getPhase());
                            matrix[i, j].setTemp(matrix[i - 1, j + 1].getTemperature());
                        }
                        else if (j == numbercolumns-1 && i == numberrows-1) //esquina abajo derecha
                        {
                            matrix[i, j].setPhase(matrix[i - 1, j - 1].getPhase());
                            matrix[i, j].setTemp(matrix[i - 1, j - 1].getTemperature());
                        }

                    }
                }
            }

        }

        // Obtenemos phase y temp actual de la cell seleccionada
        public Tuple<double, double> getphaseandtemperature(int fila, int columna)
        {
            double phase = matrix[fila, columna].getPhase();
            double temp = matrix[fila, columna].getTemperature();
            Tuple<double, double> tuple = new Tuple<double, double>(phase, temp);
            return tuple;
        }

        //Función que devuelve matriz de rectángulos con los colores de cada cell de la matriz. 
        //Bucle que recorre toda la matriz y va llamando en cada Cell la función colorear.
        public Rectangle[,] colorearphase(Rectangle[,] matrixrectangle_phase)
        {
            for (int i = 0; i < numberrows; i++)
            {
                for (int j = 0; j < numbercolumns; j++)
                {
                    matrix[i, j].colear_phase();//Asignamos color a cell
                    matrixrectangle_phase[i,j].Fill = matrix[i, j].getColorphase(); //Añadimos a la amtriz de rectangle el color de cada rectangle              
                }
            }
            return matrixrectangle_phase;
        }

        //Igual que colorear phase pero con temperatura
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

        //Devuelve una lista de puntos con el número de sólidos por iteración, esta lista sirve para el gráfico
        public List<Point> contarsolids(List<Point> listPoint_solids)
        {
            int contadorsolid = 0;
            int contador = listPoint_solids.Count;
            for (int i = 0; i < numberrows; i++)
            {
                for (int j = 0; j < numbercolumns; j++)
                {
                    contadorsolid += matrix[i, j].isSolid();//recorremos toda la matriz de Cell contando si la Cell es sólida o no
                }
            }
            listPoint_solids.Add(new Point(contador,contadorsolid));//Añadimos un punto a la lista, eje X número de iteración y eje Y número de sólidos
            return listPoint_solids;
        }

        //Devuelve una lista de puntos con la temperatura media por iteración
        public List<Point> avgtemp(List<Point> listPoint_avgtemp)
        {
            double sumatemp = 0;
            int n = 1;
            int contador = listPoint_avgtemp.Count;
            for (int i = 0; i < numberrows; i++)
            {
                for (int j = 0; j < numbercolumns; j++)
                {
                    sumatemp += matrix[i, j].getTemperature();//recorremos la amtriz de Cell sumando las temperaturas
                    n++;
                }
            }
            double avgtemp = sumatemp / n; //calculo temp media
            listPoint_avgtemp.Add(new Point(contador, avgtemp));//Añadimos un punto a la lista, eje X número de iteración y eje Y temperatura media
            return listPoint_avgtemp;
        }

        //guarda todos los elementos en un archivo txt
        public void guardar(List<Point> listPoint_solids,List<Point> listPoint_avgtemp)
        {
            SaveFileDialog ofd = new SaveFileDialog();
            ofd.DefaultExt = "txt";
            ofd.Filter = "Archivos txt(*.txt)|*.txt";
            ofd.Title = "Guarda los datos";
            ofd.ShowDialog();
            string nombre = ofd.FileName;
            StreamWriter fichero = new StreamWriter(nombre);
            fichero.Write(numberrows);//filas
            fichero.Write(" ");
            fichero.Write(numbercolumns);//columnas
            fichero.Write("\r\n");
            //escribe condiciones
            fichero.Write(conditions.getname());
            fichero.Write(" ");
            fichero.Write(conditions.getdelta_x());
            fichero.Write(" ");
            fichero.Write(conditions.getdelta_y());
            fichero.Write(" ");
            fichero.Write(conditions.getdelta());
            fichero.Write(" ");
            fichero.Write(conditions.getdelta_time());
            fichero.Write(" ");
            fichero.Write(conditions.getalpha());
            fichero.Write(" ");
            fichero.Write(conditions.getM());
            fichero.Write(" ");
            fichero.Write(conditions.getepsylon());
            fichero.Write("\r\n");
            fichero.Write(this.boundary);//escribe boundary condition
            fichero.Write("\r\n");
            //escribe valor de phase de cada una de las cell de la matriz
            for (int i = 0; i < numberrows; i++)
            {
                for (int j = 0; j < numbercolumns; j++)
                {
                    fichero.Write(matrix[i, j].getPhase());
                    fichero.Write(" ");
                }
                fichero.Write("\r\n");
            }
            //escribe valor temperatura de cada una de las cell de la matriz
            for (int i = 0; i < numberrows; i++)
            {
                for (int j = 0; j < numbercolumns; j++)
                {
                    fichero.Write(matrix[i, j].getTemperature());
                    fichero.Write(" ");
                }
                fichero.Write("\r\n");
            }
            //escribe la lista de puntos de número de sólidos por iteración
            for (int i = 0; i < listPoint_solids.Count; i++)
            {
                fichero.Write(listPoint_solids[i].X);//escribe las iteraciones
                fichero.Write(" ");
            }
            fichero.Write("\r\n");
            for (int i = 0; i < listPoint_solids.Count; i++)
            {
                fichero.Write(listPoint_solids[i].Y);//escribe el número de sólidos
                fichero.Write(" ");
            }
            fichero.Write("\r\n");

            //escribe la lista de puntos de temperatura media por iteración
            for (int i = 0; i < listPoint_avgtemp.Count; i++)
            {
                fichero.Write(listPoint_avgtemp[i].X);//escribe iteraciones
                fichero.Write(" ");
            }
            fichero.Write("\r\n");
            for (int i = 0; i < listPoint_avgtemp.Count; i++)
            {
                fichero.Write(listPoint_avgtemp[i].Y);//escribe temperatura media
                fichero.Write(" ");
            }
            fichero.Write("\r\n");
            fichero.Close();
        }

        //Abre un archivo .txt con el formato en que guardamos los datos. 
        //Devolvemos un tuple con número de filas, número columnas, condiciones, tipo de boundary conditions, matriz de Cell, y las dos listas de puntos para las gráficas
        public Tuple<int,int,Conditions,String,Cell[,],List<Point>,List<Point>> abrir()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.DefaultExt = "txt";
            ofd.Filter = "Text|*.txt";
            ofd.Title = "Abrir archivos";
            ofd.ShowDialog();
            string nombre = ofd.FileName;
            StreamReader fichero = new StreamReader(nombre);
            string linea_1 = fichero.ReadLine();
            string[] trozos_1 = linea_1.Split();
            numberrows = Convert.ToInt32(trozos_1[0]);
            numbercolumns = Convert.ToInt32(trozos_1[1]);
            string linea_2 = fichero.ReadLine();
            string[] trozos_2 = linea_2.Split();
            string nombrecond = String.Format("{0} {1}",trozos_2[0], trozos_2[1]);
            conditions = new Conditions(Convert.ToDouble(trozos_2[2]),Convert.ToDouble(trozos_2[3]),Convert.ToDouble(trozos_2[8]),Convert.ToDouble(trozos_2[7]),Convert.ToDouble(trozos_2[5]),Convert.ToDouble(trozos_2[4]),Convert.ToDouble(trozos_2[6]),nombrecond);
            string boundary = fichero.ReadLine();
            Cell[,] matrixprueba = new Cell[numberrows, numbercolumns];
            for (int i = 0; i < numberrows; i++)
            {
                string linea = Convert.ToString(fichero.ReadLine());
                string[] trozos = linea.Split();
                for (int j = 0; j < numbercolumns; j++)
                {
                    matrixprueba[i, j] = new Cell(Convert.ToDouble(trozos[j]), 0, conditions);
                }
            }
            for (int i = 0; i < numberrows; i++)
            {
                string linea = Convert.ToString(fichero.ReadLine());
                string[] trozos = linea.Split();
                for (int j = 0; j < numbercolumns; j++)
                {
                    matrixprueba[i, j].setTemp(Convert.ToDouble(trozos[j]));
                }
            }
            string linea_3 = fichero.ReadLine();
            string[] trozos_3 = linea_3.Split();

            string linea_4 = fichero.ReadLine();
            string[] trozos_4 = linea_4.Split();

            string linea_5 = fichero.ReadLine();
            string[] trozos_5 = linea_5.Split();

            string linea_6 = fichero.ReadLine();
            string[] trozos_6 = linea_6.Split();

            List<Point> list_phase = new List<Point>();
            List<Point> list_avgtemp = new List<Point>();

            for (int i = 0; i < trozos_3.Length-1; i++)
            {
                Point n = new Point();
                n.X = Convert.ToDouble(trozos_3[i]);
                n.Y = Convert.ToDouble(trozos_4[i]);
                list_phase.Add(n);

                Point m = new Point();
                m.X = Convert.ToDouble(trozos_5[i]);
                m.Y = Convert.ToDouble(trozos_6[i]);
                list_avgtemp.Add(m);
            }

            return Tuple.Create(numberrows, numbercolumns, conditions, boundary, matrixprueba, list_phase, list_avgtemp);
        }

        public void setmatrix(Cell[,] matrix)
        {
            this.matrix = matrix;
        }
        




    }
}
