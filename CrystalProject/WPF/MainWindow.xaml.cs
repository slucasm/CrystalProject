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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Conditions condition1 = new Conditions(0.005, 0.005, 0.005, 20, 5e-6, 0.5, 400);
            Conditions condition2 = new Conditions(0.005, 0.005, 0.005, 30, 5e-6, 0.7, 300);
            conditionslist.Add(condition1);
            conditionslist.Add(condition2);
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;

        }
        private void timer_Tick(object sender, EventArgs e)
        {
            matrix.initialconditions();
            matrix.initialSolid(5, 5);
            matrix.neighbours();
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
            int rows = Convert.ToInt32(textBox_rows.Text);
            int columns = Convert.ToInt32(textBox_columns.Text);
            matrix = new Matriz(rows, columns, conditionslist[comboBox_conditions.SelectedIndex]);
            matrix.createMatrix();
            tabla = matrix.createTable();
            dataGrid_cells.ItemsSource = tabla.DefaultView;
        }

        private void button_newcond_Click(object sender, RoutedEventArgs e)
        {
            Create_condition crearcondition = new Create_condition(this);
            crearcondition.Show();
        }

        private void button_ciclo_Click(object sender, RoutedEventArgs e)
        {
            matrix.initialconditions();
            matrix.initialSolid(5, 5);
            matrix.neighbours();
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


    }
}
