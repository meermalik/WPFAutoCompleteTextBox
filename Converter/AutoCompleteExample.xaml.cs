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
using System.Collections.ObjectModel;

namespace converter
{
    /// <summary>
    /// Interaction logic for AutoCompleteExample.xaml
    /// </summary>
    public partial class AutoCompleteExample : Window
    {
        List<string> lstNames;

        public AutoCompleteExample()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lstNames = new List<string>();
            lstNames.Add("Ajay");
            lstNames.Add("Rahul");
            lstNames.Add("Rajesh");
            lstNames.Add("Ravi");
            lstNames.Add("Suprotim");
            lstNames.Add("Ajit");
            lstNames.Add("Suraj");
            lstNames.Add("Vikram");
            lstNames.Add("Vikas");
            lstNames.Add("Pravin");
            lstNames.Add("Suprabhat");
            lstNames.Add("Ajay\r\nNitish");
            txtAutoCompleteBox.ItemsSource = lstNames;
        }

       
    }
    }