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
using System.Windows.Shapes;
using ClassLibrary;



namespace Cristal
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
            MessageBox.Show("Hola");

        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Conditions conditions = new Conditions(0.005,0.005,0.005,20,5*10^-6,0.5,400);
            Matriz matrix = new Matriz(11, 11, conditions);
            matrix.createMatrix();
            matrix.initialconditions();
            matrix.initialSolid(5, 5);
            Cell[,] prueba = matrix.neighbours();

            MessageBox.Show("Hola");



        }
    }
}
