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
        DispatcherTimer timer = new DispatcherTimer();
        int rows, columns;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Conditions condition1 = new Conditions(0.005, 0.005, 0.005, 20, 5e-6, 0.5, 400);
            Conditions condition2 = new Conditions(0.005, 0.005, 0.005, 30, 5e-6, 0.7, 300);
            conditionslist.Add(condition1);
            conditionslist.Add(condition2);
            comboBox_boundary.Items.Add("Reflecting");
            comboBox_boundary.Items.Add("T constant");
            comboBox_boundary.SelectedIndex = -1;

            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;

        }
        private void timer_Tick(object sender, EventArgs e)
        {
            //if (comboBox_boundary.SelectedIndex == 0) { matrix.initialconditions(1); }
            //else { matrix.initialconditions(0); }
            matrix.neighbours();
            matrix.actualizar();
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

        private void button_creategrid_Click(object sender, RoutedEventArgs e)
        {
            grid_phase.ColumnDefinitions.Clear();
            grid_phase.RowDefinitions.Clear();
            rows = Convert.ToInt32(textBox_rows.Text);
            columns = Convert.ToInt32(textBox_columns.Text);
            grid_phase.ShowGridLines = true;
            for (int j = 0; j < columns; j++)
            {
                grid_phase.ColumnDefinitions.Add(new ColumnDefinition());
            }
            for (int i = 0; i < rows; i++)
            {
                grid_phase.RowDefinitions.Add(new RowDefinition());
            }
            
            matrix = new Matriz(rows, columns, conditionslist[comboBox_conditions.SelectedIndex]);
            matrix.createMatrix();

            matrix.initialconditions(0); //Por defecto

            //if (comboBox_boundary.SelectedIndex == 0)
            //{ 
            //    matrix.initialconditions(1); 
            //}
            //else 
            //{ 
            //    matrix.initialconditions(0); 
            //}
            
            tabla = matrix.createTable();
            
            //dataGrid_cells.ItemsSource = tabla.DefaultView;
        }

        private void button_newcond_Click(object sender, RoutedEventArgs e)
        {
            Create_condition crearcondition = new Create_condition(this);
            crearcondition.Show();
        }

        private void button_ciclo_Click(object sender, RoutedEventArgs e)
        {
            //if (comboBox_boundary.SelectedIndex == 0) { matrix.initialconditions(1); }
            //else { matrix.initialconditions(0); }

            matrix.neighbours();
            matrix.actualizar();
        }

        private void button_start_Click(object sender, RoutedEventArgs e)
        {
            timer.Start();
        }

        private void button_stop_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
        }

        private void slider1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private void grid_phase_MouseDown(object sender, MouseButtonEventArgs e)
        {
            int X = Convert.ToInt32(e.GetPosition(grid_phase).X.ToString());
            int Y = Convert.ToInt32(e.GetPosition(grid_phase).Y.ToString());
            int height = Convert.ToInt32(grid_phase.Height.ToString());
            int width = Convert.ToInt32(grid_phase.Width.ToString());
            string ColumnPosition = ColumnComputation(grid_phase.ColumnDefinitions, e.GetPosition(grid_phase).X).ToString();
            string RowPosition = RowComputation(grid_phase.RowDefinitions, e.GetPosition(grid_phase).Y).ToString();

            Tuple<double,double> tuple = matrix.getphaseandtemperature(Convert.ToInt32(RowPosition), Convert.ToInt32(ColumnPosition));
            string phase = Convert.ToString(tuple.Item1);
            string temp = Convert.ToString(tuple.Item2);
            //var element = (UIElement)e.Source;
            //int row = Grid.GetRow(element);
            //int column = Grid.GetColumn(element);
            MessageBox.Show(String.Format("Column: {0} \n Row: {1}\n Phase: {2}\n Temparature: {3}", ColumnPosition, RowPosition, phase, temp), "Results");

            //MessageBox.Show("Column:" + ColumnPosition);
            //MessageBox.Show("Row:" +RowPosition);
            //MessageBox.Show("Phase:" +phase);
            //MessageBox.Show("Temperature:" +temp);
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
            if (comboBox_boundary.SelectedIndex != -1)
            {
                if (comboBox_boundary.SelectedIndex == 0) 
                { 
                    matrix.initialconditions(0); 
                }
                else 
                { 
                    matrix.initialconditions(1); 
                }
                matrix.initialSolid(Convert.ToInt32(Math.Ceiling(Convert.ToDouble(rows / 2))), Convert.ToInt32(Math.Ceiling(Convert.ToDouble(columns / 2))));
            }
        }

        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }       


    }
}
