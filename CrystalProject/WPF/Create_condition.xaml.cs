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

namespace WPF
{
    /// <summary>
    /// Interaction logic for Create_condition.xaml
    /// </summary>
    public partial class Create_condition : Window
    {
        MainWindow originalForm;
        public Create_condition(MainWindow incomingForm)//Abrimos form con la información del MainWindow
        {
            try
            {
                originalForm = incomingForm;
                InitializeComponent();
            }
            catch (Exception a)
            {
                MessageBox.Show(a.Message);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            comboBox_standard.Items.Add(originalForm.conditionslist[0].getname());
            comboBox_standard.Items.Add(originalForm.conditionslist[1].getname());
            comboBox_standard.SelectedIndex = -1;
        }

        private void button_newcondition_Click(object sender, RoutedEventArgs e)//Crear nueva condition click
        {
            try//Creamos nueva condición y la añadimos en el combobox del MainWindow
            {
                Conditions condition = new Conditions(Convert.ToDouble(textBox_AX.Text), Convert.ToDouble(textBox_AY.Text), Convert.ToDouble(textBox_epsylon.Text), Convert.ToDouble(textBox_M.Text), Convert.ToDouble(textBox_At.Text), Convert.ToDouble(textBox_delta.Text), Convert.ToDouble(textBox_B.Text), "New condition");
                originalForm.comboBox_conditions.Items.Add("New condition");
                originalForm.conditionslist.Add(condition);
                MessageBox.Show("Your condition is indexed in combobox as: 'New condition'");
                this.Close();
            }
            catch (Exception b)
            {
                MessageBox.Show(b.Message);
            }
        }

        private void comboBox_standard_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBox_standard.SelectedIndex == 0)
            {
                textBox_AX.Text = Convert.ToString(originalForm.conditionslist[0].getdelta_x());
                textBox_AY.Text = Convert.ToString(originalForm.conditionslist[0].getdelta_y());
                textBox_B.Text = Convert.ToString(originalForm.conditionslist[0].getalpha());
                textBox_epsylon.Text = Convert.ToString(originalForm.conditionslist[0].getepsylon());
                textBox_M.Text = Convert.ToString(originalForm.conditionslist[0].getM());
                textBox_delta.Text = Convert.ToString(originalForm.conditionslist[0].getdelta());
                textBox_At.Text = Convert.ToString(originalForm.conditionslist[0].getdelta_time());
            }
            else
            {
                textBox_AX.Text = Convert.ToString(originalForm.conditionslist[1].getdelta_x());
                textBox_AY.Text = Convert.ToString(originalForm.conditionslist[1].getdelta_y());
                textBox_B.Text = Convert.ToString(originalForm.conditionslist[1].getalpha());
                textBox_epsylon.Text = Convert.ToString(originalForm.conditionslist[1].getepsylon());
                textBox_M.Text = Convert.ToString(originalForm.conditionslist[1].getM());
                textBox_delta.Text = Convert.ToString(originalForm.conditionslist[1].getdelta());
                textBox_At.Text = Convert.ToString(originalForm.conditionslist[1].getdelta_time());
            }
        }




        
    }
}
