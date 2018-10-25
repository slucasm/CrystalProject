using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ClassLibrary;
using System.Data;
using System.Windows.Threading;


using Microsoft.Research.DynamicDataDisplay; // Core functionality
using Microsoft.Research.DynamicDataDisplay.DataSources; // EnumerableDataSource
using Microsoft.Research.DynamicDataDisplay.PointMarkers; // CirclePointMarker



namespace WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }

        public List<Conditions> conditionslist = new List<Conditions>();
        Matriz matrix;
        Rectangle[,] matrixrectangle_phase, matrixrectangle_temperature; //matriz de Rectangle, lo utilizamos para los colores
        DispatcherTimer timer = new DispatcherTimer(); //timer
        int rows, columns;
        Boolean selectboundarycond = false; //booleano para saber si hemos selecionado boundary conditions
        Boolean creategrid = false; //booleano para saber si hemos creado la matriz

        //Dos listas de puntos
        List<Point> listPoint_solids = new List<Point>();//De cada punto, eje X: número de iteración, eje Y: número de sólidos
        List<Point> listPoint_avgtemp = new List<Point>();//De cada punto, ejeX: número de iteración, eje Y: temperatura media de la matriz

        ChartPlotter plot_phase = new ChartPlotter();//Creamos un gráfico para número de sólidos/iteración
        ChartPlotter plot_temp = new ChartPlotter();//Creamos un gráfico para temperatura media(iteración




        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                //Definimos 2 condiciones estándar
                Conditions condition1 = new Conditions(0.005, 0.005, 0.005, 20, 5e-6, 0.5, 400, "Standard 1");
                Conditions condition2 = new Conditions(0.005, 0.005, 0.005, 30, 5e-6, 0.7, 300, "Standard 2");
                //Añadimos condiciones al combobox
                conditionslist.Add(condition1);
                conditionslist.Add(condition2);
                //Añadimos tipo de boundary conditions al combobox correspondiente
                comboBox_boundary.Items.Add("Reflecting");
                comboBox_boundary.Items.Add("T constant solid");
                comboBox_boundary.Items.Add("T constant liquid");
                comboBox_boundary.SelectedIndex = -1;

                //Combobox para variar entre las dos gráficas
                comboBox_grid.Items.Add("Phase grid");
                comboBox_grid.Items.Add("Temperature grid");
                comboBox_grid.SelectedIndex = 0;

                //Parámetros slider
                slider_speed.Minimum = 1;
                slider_speed.Maximum = 1000;
                slider_speed.TickFrequency = 100;
                slider_speed.Value = 1000;

                //Añadimos a las gráficas a dos grids, para mostrarlo en la MainWindow
                grid_plotphase.Children.Add(plot_phase);
                grid_plottemp.Children.Add(plot_temp);

                timer.Interval = TimeSpan.FromMilliseconds(slider_speed.Value);//Definimos velocidad timer
                timer.Tick += timer_Tick;//Definimos función para el Tick
            }
            catch (Exception a)
            {
                MessageBox.Show(a.Message);
            }

        }
        private void timer_Tick(object sender, EventArgs e) //Definimos instrucciones del timer en cada tick
        {
            try
            {
                //actualizamos matriz y pintamos matriz de rectangle (para poder hacer esto primero la matriz debe estar creada)
                matrix.neighbours();
                matrix.actualizar();
                matrixrectangle_phase = matrix.colorearphase(matrixrectangle_phase);
                matrixrectangle_temperature = matrix.colortemperature(matrixrectangle_temperature);

                //actualizamos listas de puntos
                listPoint_solids = matrix.contarsolids(listPoint_solids);
                listPoint_avgtemp = matrix.avgtemp(listPoint_avgtemp);
                RawDataSource d1 = new RawDataSource(listPoint_solids);
                RawDataSource d2 = new RawDataSource(listPoint_avgtemp);

                //añadimos cada iteración el nuevo punto a cada gráfica
                plot_phase.AddLineGraph(d1, Colors.Blue, 1, "Number solids");
                plot_phase.LegendVisible = false;//quitamos leyenda para que no se multiplique
                plot_temp.AddLineGraph(d2, Colors.Red, 1, "Average temperature");
                plot_temp.LegendVisible = false;
            }
            catch (Exception b)
            {
                MessageBox.Show(b.Message);
            }

        }

        private void comboBox_conditions_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                //Cargamos las labels de las conditiones
                comboBox_conditions.Items.Add("Standard 1");
                comboBox_conditions.Items.Add("Standard 2");
                comboBox_conditions.SelectedIndex = 0;
                label_AX.Content = "∆X: " + Convert.ToString(conditionslist[comboBox_conditions.SelectedIndex].getdelta_x());
                label_AY.Content = "∆Y: " + Convert.ToString(conditionslist[comboBox_conditions.SelectedIndex].getdelta_y());
                label_epsylon.Content = "ε: " + Convert.ToString(conditionslist[comboBox_conditions.SelectedIndex].getepsylon());
                label_B.Content = "B: " + Convert.ToString(conditionslist[comboBox_conditions.SelectedIndex].getalpha());
                label_delta.Content = "∆: " + Convert.ToString(conditionslist[comboBox_conditions.SelectedIndex].getdelta());
                label_At.Content = "∆t: " + Convert.ToString(conditionslist[comboBox_conditions.SelectedIndex].getdelta_time());
                label_M.Content = "M: " + Convert.ToString(conditionslist[comboBox_conditions.SelectedIndex].getM());
            }
            catch (Exception c)
            {
                MessageBox.Show(c.Message);
            }
        }

        private void comboBox_conditions_SelectionChanged(object sender, SelectionChangedEventArgs e)//en caso de seleccionar otra condición se actualizan labels
        {
            try
            {
                //Cambiamos labels de las condiciones según la condición seleccionada
                label_AX.Content = "∆X: " + Convert.ToString(conditionslist[comboBox_conditions.SelectedIndex].getdelta_x());
                label_AY.Content = "∆Y: " + Convert.ToString(conditionslist[comboBox_conditions.SelectedIndex].getdelta_y());
                label_epsylon.Content = "ε: " + Convert.ToString(conditionslist[comboBox_conditions.SelectedIndex].getepsylon());
                label_B.Content = "B: " + Convert.ToString(conditionslist[comboBox_conditions.SelectedIndex].getalpha());
                label_delta.Content = "∆: " + Convert.ToString(conditionslist[comboBox_conditions.SelectedIndex].getdelta());
                label_At.Content = "∆t: " + Convert.ToString(conditionslist[comboBox_conditions.SelectedIndex].getdelta_time());
                label_M.Content = "M: " + Convert.ToString(conditionslist[comboBox_conditions.SelectedIndex].getM());
            }
            catch (Exception d)
            {
                MessageBox.Show(d.Message);
            }
        }

        public void creargrid() //inicializamos grid
        {
            try
            {
                //borramos todo lo que havia cargado, para evitar errores
                grid_phase.ColumnDefinitions.Clear();
                grid_temperature.ColumnDefinitions.Clear();
                grid_phase.RowDefinitions.Clear();
                grid_temperature.RowDefinitions.Clear();
                listPoint_avgtemp.Clear();
                listPoint_solids.Clear();

                //si rows y columns es positivo seguimos, sinó, no podemos avanzar
                if ((Convert.ToInt32(textBox_rows.Text) <= 0) || (Convert.ToInt32(textBox_columns.Text) <= 0))
                {
                    MessageBox.Show("Please, insert a positive number");
                }
                else
                {
                    rows = Convert.ToInt32(textBox_rows.Text);
                    columns = Convert.ToInt32(textBox_columns.Text);

                    grid_phase.ShowGridLines = false;
                    //creamos filas y columnas del grid
                    for (int j = 0; j < columns; j++)
                    {
                        grid_phase.ColumnDefinitions.Add(new ColumnDefinition());
                        grid_temperature.ColumnDefinitions.Add(new ColumnDefinition());
                    }
                    for (int i = 0; i < rows; i++)
                    {
                        grid_phase.RowDefinitions.Add(new RowDefinition());
                        grid_temperature.RowDefinitions.Add(new RowDefinition());
                    }
                    //Creamos 2 matriz de Rectangle del mismo tamaño que matrix
                    matrixrectangle_phase = new Rectangle[rows, columns];
                    matrixrectangle_temperature = new Rectangle[rows, columns];

                    for (int i = 0; i < rows; i++)//rellenamos grid_phase y grid_tempemperature de la matrix de rectangle correspondiente
                    {
                        for (int j = 0; j < columns; j++)
                        {

                            matrixrectangle_phase[i, j] = new Rectangle();
                            matrixrectangle_phase[i, j].Fill = new SolidColorBrush(System.Windows.Media.Colors.White);
                            Grid.SetRow(matrixrectangle_phase[i, j], i);
                            Grid.SetColumn(matrixrectangle_phase[i, j], j);
                            grid_phase.Children.Add(matrixrectangle_phase[i, j]);

                            matrixrectangle_temperature[i, j] = new Rectangle();
                            matrixrectangle_temperature[i, j].Fill = new SolidColorBrush(System.Windows.Media.Colors.White);
                            Grid.SetRow(matrixrectangle_temperature[i, j], i);
                            Grid.SetColumn(matrixrectangle_temperature[i, j], j);
                            grid_temperature.Children.Add(matrixrectangle_temperature[i, j]);
                        }
                    }

                    matrix = new Matriz(rows, columns, conditionslist[comboBox_conditions.SelectedIndex]);//llenamos matriz de Cell
                    matrix.createMatrix();
                    matrix.initialconditions(0); //Por defecto
                    creategrid = true;
                }
            }
            catch (FormatException e)
            {
                MessageBox.Show(e.Message);
            }
        }


        private void button_creategrid_Click(object sender, RoutedEventArgs e)//Creamos grid
        {
            try
            {
                creargrid();
                timer.Stop();
                comboBox_boundary.IsEnabled = true;//Mostramos combobox de las condiciones de frontera
            }
            catch (Exception f)
            {
                MessageBox.Show(f.Message);
            }
        }

        private void button_newcond_Click(object sender, RoutedEventArgs e)//abrimos form de crear nueva condición
        {
            try
            {
                Create_condition crearcondition = new Create_condition(this);//le enviamos al form Create_conditions los datos de este Form
                crearcondition.Show();
            }
            catch (Exception g)
            {
                MessageBox.Show(g.Message);
            }
        }

        private void button_ciclo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (creategrid == false)//si la amtriz no está creada no podemos avanzar
                {
                    MessageBox.Show("Please, create the GRID before the Simulation");
                }
                else if (selectboundarycond == true && creategrid == true) //si la matriz está creada, y la condición de frontera también podemos avanzar
                {
                    //Actualizamos datos
                    matrix.neighbours();
                    matrix.actualizar();
                    matrixrectangle_phase = matrix.colorearphase(matrixrectangle_phase);
                    matrixrectangle_temperature = matrix.colortemperature(matrixrectangle_temperature);

                    listPoint_solids = matrix.contarsolids(listPoint_solids);
                    listPoint_avgtemp = matrix.avgtemp(listPoint_avgtemp);

                    //añadimos datos actualizados a las gráficas
                    RawDataSource d1 = new RawDataSource(listPoint_solids);
                    RawDataSource d2 = new RawDataSource(listPoint_avgtemp);
                    plot_phase.AddLineGraph(d1, Colors.Blue, 1, "Number solids");
                    plot_phase.LegendVisible = false;
                    plot_temp.AddLineGraph(d2, Colors.Red, 1, "Average temperature");
                    plot_temp.LegendVisible = false;
                }
                else if (selectboundarycond == false) //si no hemos seleccionado la condición de frontera no podemos avanzar
                {
                    MessageBox.Show("Please, select Boundary Conditions before the Simulation");
                }
            }
            catch (Exception h)
            {
                MessageBox.Show(h.Message);
            }
        }

        private void button_start_Click(object sender, RoutedEventArgs e)//Start simulation button
        {
            try
            {
                if (creategrid == false)
                {
                    MessageBox.Show("Please, create the GRID before the Simulation");
                }
                else if (selectboundarycond == true && creategrid == true)//Solo funciona si hemos creado matriz y seleccionado condicion de frontera
                {
                    timer.Start();
                }
                else if (selectboundarycond == false)
                {
                    MessageBox.Show("Please, select Boundary Conditions before the Simulation");
                }
            }
            catch (Exception i)
            {
                MessageBox.Show(i.Message);
            }

        }

        private void button_stop_Click(object sender, RoutedEventArgs e) //Stop Button
        {
            try
            {
                if (creategrid == false)
                {
                    MessageBox.Show("Please, create the GRID before the Simulation");
                }
                else if (selectboundarycond == true && creategrid == true)
                {
                    timer.Stop();
                }
                else if (selectboundarycond == false)
                {
                    MessageBox.Show("Please, select Boundary Conditions before the Simulation");
                }
            }
            catch (Exception j)
            {
                MessageBox.Show(j.Message);
            }
        }

        private void button_restart_Click(object sender, RoutedEventArgs e)//Restart button
        {
            try
            {
                if (creategrid == false)
                {
                    MessageBox.Show("Please, create the GRID before the Simulation");
                }
                else if (selectboundarycond == true && creategrid == true)
                {
                    //paramos simulación y reestblecemos todo
                    timer.Stop();
                    creargrid();
                    plot_phase.Children.RemoveAll((typeof(LineGraph)));
                    plot_temp.Children.RemoveAll((typeof(LineGraph)));
                    listPoint_avgtemp.Clear();
                    listPoint_solids.Clear();

                    comboBox_boundary.IsEnabled = false;
                    comboBox_boundary.SelectedIndex = -1;
                    creategrid = false;
                    selectboundarycond = false;
                    slider_speed.Value = 1000;
                }
                else if (selectboundarycond == false)
                {
                    MessageBox.Show("Please, select Boundary Conditions before the Simulation");
                }
            }
            catch (Exception k)
            {
                MessageBox.Show(k.Message);
            }
        }



        private void grid_phase_MouseDown(object sender, MouseButtonEventArgs e)//Click en grid_phase
        {
            try //Nos da un MessageBox con el número de fila y columna seleccionado, y el valor de phase de la celda
            //Llamamos a dos funciones que hemos creado para saber número de fila y columna según la posición X,Y que hemos clickado en el grid
            {
                int height = Convert.ToInt32(grid_phase.Height.ToString());
                int width = Convert.ToInt32(grid_phase.Width.ToString());
                string ColumnPosition = ColumnComputation(grid_phase.ColumnDefinitions, e.GetPosition(grid_phase).X).ToString();
                string RowPosition = RowComputation(grid_phase.RowDefinitions, e.GetPosition(grid_phase).Y).ToString();

                Tuple<double, double> tuple = matrix.getphaseandtemperature(Convert.ToInt32(RowPosition), Convert.ToInt32(ColumnPosition));
                string cellPhase = Convert.ToString(tuple.Item1);
                string cellTemp = Convert.ToString(tuple.Item2);

                MessageBox.Show(String.Format("Column: {0} \n Row: {1}\n Phase: {2}", ColumnPosition, RowPosition, cellPhase), "Results");
            }
            catch (Exception l)
            {
                MessageBox.Show(l.Message);
            }

        }

        private double ColumnComputation(ColumnDefinitionCollection c, double YPosition)//Cálculo de la columna según posición Y que hemos clickado en el grid_phase
        {
            var columnLeft = 0.0; var columnCount = 0;
            foreach (ColumnDefinition cd in c)
            {
                double actWidth = cd.ActualWidth;
                if (YPosition >= columnLeft && YPosition < (actWidth + columnLeft)) return columnCount;
                columnCount++;
                columnLeft += cd.ActualWidth;
            }
            return (c.Count + 1);
        }
        private double RowComputation(RowDefinitionCollection r, double XPosition)//Cálculo de la fila según posición X que hemos clickado en el grid
        {
            var rowTop = 0.0; var rowCount = 0;
            foreach (RowDefinition rd in r)
            {
                double actHeight = rd.ActualHeight;
                if (XPosition >= rowTop && XPosition < (actHeight + rowTop)) return rowCount;
                rowCount++;
                rowTop += rd.ActualHeight;
            }
            return (r.Count + 1);
        }

        private void comboBox_boundary_SelectionChanged(object sender, SelectionChangedEventArgs e)//Si combobox_boundary cambia de valor
        {
            try//cambiamos initialconditions de la matriz si desplegamos el combobox
            {
                if ((comboBox_boundary.SelectedIndex != -1)) //&& (matrix != null))
                {
                    if (comboBox_boundary.SelectedIndex == 0)
                    {
                        matrix.initialconditions(0);
                    }
                    else if (comboBox_boundary.SelectedIndex == 1)
                    {
                        matrix.initialconditions(1);
                    }
                    else if (comboBox_boundary.SelectedIndex == 2)
                    {
                        matrix.initialconditions(2);
                    }
                    else
                    {
                        matrix.initialconditions(-1);
                    }
                    //inicializamos otra vez la matriz con los nuevos valores
                    matrix.initialSolid(Convert.ToInt32(Math.Ceiling(Convert.ToDouble(rows / 2))), Convert.ToInt32(Math.Ceiling(Convert.ToDouble(columns / 2))));
                    matrixrectangle_phase = matrix.colorearphase(matrixrectangle_phase);
                    matrixrectangle_temperature = matrix.colortemperature(matrixrectangle_temperature);
                    selectboundarycond = true;

                    listPoint_solids = matrix.contarsolids(listPoint_solids);
                    listPoint_avgtemp = matrix.avgtemp(listPoint_avgtemp);

                    RawDataSource d1 = new RawDataSource(listPoint_solids);
                    RawDataSource d2 = new RawDataSource(listPoint_avgtemp);
                }
                else
                {
                    //MessageBox.Show("Please create the grid first!");
                }
            }
            catch (Exception m)
            {
                MessageBox.Show(m.Message);
            }

        }


        private void slider_speed_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) //Si cambiamos posición slider
        {
            try
            {
                timer.Interval = TimeSpan.FromMilliseconds(slider_speed.Value);//cambiamos velocidad simulación según posición slider
            }
            catch (Exception n)
            {
                MessageBox.Show(n.Message);
            }
        }

        private void grid_temperature_MouseDown(object sender, MouseButtonEventArgs e)//Igual que en el click en grid_phase
        {
            try //Nos da MessageBox con columna y fila seleccionada y el valor de temperatura de la cell
            {
                int height = Convert.ToInt32(grid_phase.Height.ToString());
                int width = Convert.ToInt32(grid_phase.Width.ToString());
                string ColumnPosition = ColumnComputation(grid_phase.ColumnDefinitions, e.GetPosition(grid_phase).X).ToString();
                string RowPosition = RowComputation(grid_phase.RowDefinitions, e.GetPosition(grid_phase).Y).ToString();

                Tuple<double, double> tuple = matrix.getphaseandtemperature(Convert.ToInt32(RowPosition), Convert.ToInt32(ColumnPosition));
                string phase = Convert.ToString(tuple.Item1);
                string temp = Convert.ToString(tuple.Item2);

                MessageBox.Show(String.Format("Column: {0} \n Row: {1}\n Temperature: {2}", ColumnPosition, RowPosition, temp), "Results");
            }
            catch (Exception o)
            {
                MessageBox.Show(o.Message);
            }

        }


        private void comboBox_grid_SelectionChanged(object sender, SelectionChangedEventArgs e)//Cambiamos combobox grid
        {
            try
            {
                if (comboBox_grid.SelectedIndex == 1)//Nos muestra el grid de temperatura
                {
                    grid_temperature.Visibility = Visibility.Visible;
                    rectangle_gradienttemp.Visibility = Visibility.Visible;
                    grid_phase.Visibility = Visibility.Hidden;
                    rectangle_gradientphase.Visibility = Visibility.Hidden;
                    label_infocell.Content = "Cell temperature";
                    label_infoizq.Content = "-1";
                    label_infoder.Content = "0";
                }
                else//Nos muestra el grid de phase 
                {
                    grid_temperature.Visibility = Visibility.Hidden;
                    rectangle_gradienttemp.Visibility = Visibility.Hidden;
                    grid_phase.Visibility = Visibility.Visible;
                    rectangle_gradientphase.Visibility = Visibility.Visible;
                    label_infocell.Content = "Cell phase";
                    label_infoizq.Content = "LIQUID";
                    label_infoder.Content = "SOLID";
                }
            }
            catch (Exception p)
            {
                MessageBox.Show(p.Message);
            }

        }



        private void button_save_Click(object sender, RoutedEventArgs e) //Guardamos todos los datos en un .txt
        {
            try
            {
                matrix.guardar(listPoint_solids, listPoint_avgtemp);
            }
            catch (Exception q)
            {
                MessageBox.Show(q.Message);
            }
        }

        private void button_open_Click(object sender, RoutedEventArgs e)//Abrimos un archivo .txt
        {
            try//cargamos todos los elementos del archvio .txt en el proyecto para poder seguir por donde se guardó
            {
                matrix = new Matriz(0, 0, conditionslist[1]);
                Tuple<int, int, Conditions, String, Cell[,], List<Point>, List<Point>> tuple = matrix.abrir();
                rows = tuple.Item1;
                textBox_rows.Text = Convert.ToString(rows);
                columns = tuple.Item2;
                textBox_columns.Text = Convert.ToString(columns);
                conditionslist.Add(tuple.Item3);
                comboBox_conditions.Items.Add(tuple.Item3.getname());
                comboBox_conditions.SelectedValue = tuple.Item3.getname();
                label_rows.Content = Convert.ToString(rows);
                label_columns.Content = Convert.ToString(columns);
                creargrid();
                matrix = new Matriz(rows, columns, tuple.Item3);

                matrix.createMatrix();

                if (string.Compare("Solid", tuple.Item4) == 0)
                {
                    comboBox_boundary.SelectedIndex = 1;
                }
                else if (string.Compare("Liquid", tuple.Item4) == 0)
                {
                    comboBox_boundary.SelectedIndex = 2;
                }
                else if (string.Compare("Reflecting", tuple.Item4) == 0)
                {
                    comboBox_boundary.SelectedIndex = 0;
                }
                comboBox_boundary.IsEnabled = true;
                matrix.setmatrix(tuple.Item5);
                listPoint_solids = tuple.Item6;
                listPoint_avgtemp = tuple.Item7;

                RawDataSource d1 = new RawDataSource(listPoint_solids);
                RawDataSource d2 = new RawDataSource(listPoint_avgtemp);

                plot_phase.AddLineGraph(d1, Colors.Blue, 1, "Number solids");
                plot_phase.LegendVisible = false;
                plot_temp.AddLineGraph(d2, Colors.Red, 1, "Average temperature");
                plot_temp.LegendVisible = false;
            }
            catch (Exception r)
            {
                MessageBox.Show(r.Message);
            }


        }

        private void button_information_Click(object sender, RoutedEventArgs e)//Button information click
        {
            try//MessageBox con toda la información del proyecto
            {
                MessageBox.Show("This Crystal Project consists in the growth of a simulated Crystal appling the Heat equation learn in Simulation lectures. \n\n The procedure is the following:\n\n 1st. Create the grid introducing the number of columns and rows. \n\n 2nd. Introduce the Boundary conditions you desire. \n\n 3rd. You can run the simulation selecting between observing the Temparature (default) or the Phase.  \n\n\n\n\n This project has been made by: \n\n Enric Gil, Adrian Garcia & Sergi Lucas");
            }
            catch (Exception s)
            {
                MessageBox.Show(s.Message);
            }
        }

        private void grid_phase_MouseEnter(object sender, MouseEventArgs e)//Cuando entra el mouse en el grid que de el valor de entrada en los labels
        {
            if (creategrid == true)
            {
                int height = Convert.ToInt32(grid_phase.Height.ToString());
                int width = Convert.ToInt32(grid_phase.Width.ToString());
                string ColumnPosition = ColumnComputation(grid_phase.ColumnDefinitions, e.GetPosition(grid_phase).X).ToString();
                string RowPosition = RowComputation(grid_phase.RowDefinitions, e.GetPosition(grid_phase).Y).ToString();

                Tuple<double, double> tuple = matrix.getphaseandtemperature(Convert.ToInt32(RowPosition), Convert.ToInt32(ColumnPosition));
                string cellPhase = Convert.ToString(tuple.Item1);
                string cellTemp = Convert.ToString(tuple.Item2);
                label_phasecell.Content = String.Format("Cell phase: {0}", cellPhase);
                label_tempcell.Content = String.Format("Cell temperature: {0}", cellTemp);
                label_gridcolumn.Content = String.Format("Column: {0}", ColumnPosition);
                label_gridrow.Content = String.Format("Row: {0}", RowPosition);
            }
            else
            { }
        }

        private void Grid_MouseLeave(object sender, MouseEventArgs e)//Cuando sale el mouse del grid que reestablezca los valores de los labels
        {
            if (creategrid == true)
            {
                label_phasecell.Content = String.Format("Cell phase:");
                label_tempcell.Content = String.Format("Cell temperature:");
                label_gridcolumn.Content = String.Format("Column:");
                label_gridrow.Content = String.Format("Row:");
            }
            else
            {
            }
        }

        private void grid_phase_MouseMove(object sender, MouseEventArgs e)//Cuando movemos el mouse por encima del grid que vaya actualizando los valores de las labels
        {
            if (creategrid == true)
            {
                int height = Convert.ToInt32(grid_phase.Height.ToString());
                int width = Convert.ToInt32(grid_phase.Width.ToString());
                string ColumnPosition = ColumnComputation(grid_phase.ColumnDefinitions, e.GetPosition(grid_phase).X).ToString();
                string RowPosition = RowComputation(grid_phase.RowDefinitions, e.GetPosition(grid_phase).Y).ToString();

                Tuple<double, double> tuple = matrix.getphaseandtemperature(Convert.ToInt32(RowPosition), Convert.ToInt32(ColumnPosition));
                string cellPhase = Convert.ToString(tuple.Item1);
                string cellTemp = Convert.ToString(tuple.Item2);
                label_phasecell.Content = String.Format("Cell phase: {0}", Math.Round(Convert.ToDecimal(cellPhase),4));
                label_tempcell.Content = String.Format("Cell temperature: {0}", Math.Round(Convert.ToDecimal(cellTemp),4));
                label_gridcolumn.Content = String.Format("Column: {0}", ColumnPosition);
                label_gridrow.Content = String.Format("Row: {0}", RowPosition);
            }
            else
            { }
        }
    }
}
