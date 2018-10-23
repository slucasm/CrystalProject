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

        public List<Conditions> conditionslist = new List<Conditions>(); //= new List<Conditions>();
        Matriz matrix;
        DataTable tabla;
        Rectangle[,] matrixrectangle_phase, matrixrectangle_temperature;
        DispatcherTimer timer = new DispatcherTimer();
        int rows, columns;
        Boolean selectboundarycond = false;

        List<Point> listPoint_solids = new List<Point>();
        List<Point> listPoint_avgtemp = new List<Point>();
        //RawDataSource d1 = new RawDataSource();//listPoint_solids);
        //RawDataSource d2 = new RawDataSource();//listPoint_avgtemp);

        ChartPlotter plot_phase = new ChartPlotter();
        ChartPlotter plot_temp = new ChartPlotter();


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Conditions condition1 = new Conditions(0.005, 0.005, 0.005, 20, 5e-6, 0.5, 400);
            Conditions condition2 = new Conditions(0.005, 0.005, 0.005, 30, 5e-6, 0.7, 300);
            conditionslist.Add(condition1);
            conditionslist.Add(condition2);
            comboBox_boundary.Items.Add("Reflecting");
            comboBox_boundary.Items.Add("T constant solid");
            comboBox_boundary.Items.Add("T constant liquid");
            comboBox_boundary.SelectedIndex = -1;

            comboBox_grid.Items.Add("Phase grid");
            comboBox_grid.Items.Add("Temperature grid");
            comboBox_grid.SelectedIndex = 1;

            slider_speed.Minimum = 1;
            slider_speed.Maximum = 1000;
            slider_speed.TickFrequency = 100;

            grid_plotphase.Children.Add(plot_phase);
            grid_plottemp.Children.Add(plot_temp);

            timer.Interval = TimeSpan.FromMilliseconds(slider_speed.Value);
            timer.Tick += timer_Tick;

        }
        private void timer_Tick(object sender, EventArgs e)
        {

            matrix.neighbours();
            matrix.actualizar();
            matrixrectangle_phase = matrix.colorearphase(matrixrectangle_phase);
            matrixrectangle_temperature = matrix.colortemperature(matrixrectangle_temperature);

            listPoint_solids = matrix.contarsolids(listPoint_solids);
            listPoint_avgtemp = matrix.avgtemp(listPoint_avgtemp);

            RawDataSource d1 = new RawDataSource(listPoint_solids);
            RawDataSource d2 = new RawDataSource(listPoint_avgtemp);

            plot_phase.AddLineGraph(d1, Colors.Blue, 1, "Number solids");
            plot_phase.LegendVisible = false;
            plot_temp.AddLineGraph(d2, Colors.Red, 1, "Average temperature");
            plot_temp.LegendVisible = false;

        }

        private void comboBox_conditions_Loaded(object sender, RoutedEventArgs e)
        {
            comboBox_conditions.Items.Add("Standard 1");
            comboBox_conditions.Items.Add("Standard 2");
            comboBox_conditions.SelectedIndex = 0;
            label_AX.Content = "∆X: " + Convert.ToString(conditionslist[comboBox_conditions.SelectedIndex].getdelta_x());
            label_AY.Content = "∆Y: " + Convert.ToString(conditionslist[comboBox_conditions.SelectedIndex].getdelta_y());
            label_epsylon.Content = "ε: " + Convert.ToString(conditionslist[comboBox_conditions.SelectedIndex].getepsylon());
            label_B.Content = "B: " + Convert.ToString(conditionslist[comboBox_conditions.SelectedIndex].getepsylon());
            label_delta.Content = "∆: " + Convert.ToString(conditionslist[comboBox_conditions.SelectedIndex].getdelta());
            label_At.Content = "∆t: " + Convert.ToString(conditionslist[comboBox_conditions.SelectedIndex].getdelta_time());
            label_M.Content = "M: " + Convert.ToString(conditionslist[comboBox_conditions.SelectedIndex].getM());
        }

        private void comboBox_conditions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            label_AX.Content = "∆X: " + Convert.ToString(conditionslist[comboBox_conditions.SelectedIndex].getdelta_x());
            label_AY.Content = "∆Y: " + Convert.ToString(conditionslist[comboBox_conditions.SelectedIndex].getdelta_y());
            label_epsylon.Content = "ε: " + Convert.ToString(conditionslist[comboBox_conditions.SelectedIndex].getepsylon());
            label_B.Content = "B: " + Convert.ToString(conditionslist[comboBox_conditions.SelectedIndex].getepsylon());
            label_delta.Content = "∆: " + Convert.ToString(conditionslist[comboBox_conditions.SelectedIndex].getdelta());
            label_At.Content = "∆t: " + Convert.ToString(conditionslist[comboBox_conditions.SelectedIndex].getdelta_time());
            label_M.Content = "M: " + Convert.ToString(conditionslist[comboBox_conditions.SelectedIndex].getM());
        }

        public void creargrid()
        { 
            grid_phase.ColumnDefinitions.Clear();
            grid_temperature.ColumnDefinitions.Clear();
            grid_phase.RowDefinitions.Clear();
            grid_temperature.RowDefinitions.Clear();
            try
            {
                    if ((Convert.ToInt32(textBox_rows.Text) <= 0) || (Convert.ToInt32(textBox_columns.Text) <= 0))
                        {
                             MessageBox.Show("Please, insert a positive number");
                        }
                    else
                    {
                    rows = Convert.ToInt32(textBox_rows.Text);
                    columns = Convert.ToInt32(textBox_columns.Text);

                    grid_phase.ShowGridLines = false;
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

                    matrixrectangle_phase = new Rectangle[rows, columns];
                    matrixrectangle_temperature = new Rectangle[rows, columns];

                    for (int i = 0; i < rows; i++)
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

                    matrix = new Matriz(rows, columns, conditionslist[comboBox_conditions.SelectedIndex]);
                    matrix.createMatrix();

                    matrix.initialconditions(0); //Por defecto

                    tabla = matrix.createTable();

                    //dataGrid_cells.ItemsSource = tabla.DefaultView;
                }
                }
                catch (FormatException e)
                {
                    MessageBox.Show(e.Message);
                }
        }


        private void button_creategrid_Click(object sender, RoutedEventArgs e)
        {
            creargrid();
            comboBox_boundary.IsEnabled=true;
        }

        private void button_newcond_Click(object sender, RoutedEventArgs e)
        {
            Create_condition crearcondition = new Create_condition(this);
            crearcondition.Show();
        }

        private void button_ciclo_Click(object sender, RoutedEventArgs e)
        {
            if (matrix == null)
            {
                MessageBox.Show("Please, create the GRID before the Simulation");
            }
            else if (selectboundarycond == true)
            {
                matrix.neighbours();
                matrix.actualizar();
                matrixrectangle_phase = matrix.colorearphase(matrixrectangle_phase);
                matrixrectangle_temperature = matrix.colortemperature(matrixrectangle_temperature);

                listPoint_solids = matrix.contarsolids(listPoint_solids);
                listPoint_avgtemp = matrix.avgtemp(listPoint_avgtemp);

                RawDataSource d1 = new RawDataSource(listPoint_solids);
                RawDataSource d2 = new RawDataSource(listPoint_avgtemp);

                plot_phase.AddLineGraph(d1, Colors.Blue, 1,"Number solids");
                plot_phase.LegendVisible = false;
                plot_temp.AddLineGraph(d2, Colors.Red, 1,"Average temperature");
                plot_temp.LegendVisible = false;
            }
            else if (selectboundarycond == false)
            {
                MessageBox.Show("Please, select Boundary Conditions before the Simulation");
            }
        }

        private void button_start_Click(object sender, RoutedEventArgs e)
        {
            if (matrix == null)
            {
                MessageBox.Show("Please, create the GRID before the Simulation");
            }
            else if (selectboundarycond == true)
            {
                timer.Start();
            }
            else if (selectboundarycond == false)
            {
                MessageBox.Show("Please, select Boundary Conditions before the Simulation");
            }
        
        }
               
        private void button_stop_Click(object sender, RoutedEventArgs e) //Stop Button
        {
            if (matrix == null)
            {
                MessageBox.Show("Please, create the GRID before the Simulation");
            }
            else if (selectboundarycond == true)
            {
                timer.Stop();
            }
            else if (selectboundarycond == false)
            {
                MessageBox.Show("Please, select Boundary Conditions before the Simulation");
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e) //Restart Button
        {
            if (matrix == null)
            {
                MessageBox.Show("Please, create the GRID before the Simulation");
            }
            else if (selectboundarycond == true)
            {
                timer.Stop();
                creargrid();
            }
            else if (selectboundarycond == false)
            {
                MessageBox.Show("Please, select Boundary Conditions before the Simulation");
            }
        }

        private void grid_phase_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //int X = Convert.ToInt32(e.GetPosition(grid_phase).X.ToString());
            //int Y = Convert.ToInt32(e.GetPosition(grid_phase).Y.ToString());
            int height = Convert.ToInt32(grid_phase.Height.ToString());
            int width = Convert.ToInt32(grid_phase.Width.ToString());
            string ColumnPosition = ColumnComputation(grid_phase.ColumnDefinitions, e.GetPosition(grid_phase).X).ToString();
            string RowPosition = RowComputation(grid_phase.RowDefinitions, e.GetPosition(grid_phase).Y).ToString();

            Tuple<double, double> tuple = matrix.getphaseandtemperature(Convert.ToInt32(RowPosition), Convert.ToInt32(ColumnPosition));
            string phase = Convert.ToString(tuple.Item1);
            string temp = Convert.ToString(tuple.Item2);

            MessageBox.Show(String.Format("Column: {0} \n Row: {1}\n Phase: {2}", ColumnPosition, RowPosition, phase), "Results");

        }

        private double ColumnComputation(ColumnDefinitionCollection c, double YPosition)
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
        private double RowComputation(RowDefinitionCollection r, double XPosition)
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

        private void comboBox_boundary_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((comboBox_boundary.SelectedIndex != -1) && (matrix != null))
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
                MessageBox.Show("Please create the grid first!");
            }
            
        }


        private void slider_speed_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            timer.Interval = TimeSpan.FromMilliseconds(slider_speed.Value);
        }

        private void grid_temperature_MouseDown(object sender, MouseButtonEventArgs e)
        {

            //int X = Convert.ToInt32(e.GetPosition(grid_temperature).X.ToString());
            //int Y = Convert.ToInt32(e.GetPosition(grid_temperature).Y.ToString());
            int height = Convert.ToInt32(grid_phase.Height.ToString());
            int width = Convert.ToInt32(grid_phase.Width.ToString());
            string ColumnPosition = ColumnComputation(grid_phase.ColumnDefinitions, e.GetPosition(grid_phase).X).ToString();
            string RowPosition = RowComputation(grid_phase.RowDefinitions, e.GetPosition(grid_phase).Y).ToString();

            Tuple<double, double> tuple = matrix.getphaseandtemperature(Convert.ToInt32(RowPosition), Convert.ToInt32(ColumnPosition));
            string phase = Convert.ToString(tuple.Item1);
            string temp = Convert.ToString(tuple.Item2);

            MessageBox.Show(String.Format("Column: {0} \n Row: {1}\n Temperature: {2}", ColumnPosition, RowPosition, temp), "Results");

        }


        private void comboBox_grid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBox_grid.SelectedIndex == 1)
            {
                //grid_phase.Opacity = 0;
                //grid_temperature.Opacity = 1;
                grid_temperature.Visibility = Visibility.Visible;
                grid_phase.Visibility = Visibility.Hidden;
            }
            else
            {
                //grid_phase.Opacity = 1;
                //grid_temperature.Opacity = 0;
                grid_temperature.Visibility = Visibility.Hidden;
                grid_phase.Visibility = Visibility.Visible;
            }

        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("This Crystal Project consists in the growth of a simulated Crystal appling the Heat equation learn in Simulation lectures. \n\n The procedure is the following:\n\n 1st. Create the grid introducing the number of columns and rows. \n\n 2nd. Introduce the Boundary conditions you desire. \n\n 3rd. You can run the simulation selecting between observing the Temparature (default) or the Phase.  \n\n\n\n\n This project has been made by: \n\n Enric Gil, Adrian Gonzalez & Sergi Lucas");

        }








    }
}
