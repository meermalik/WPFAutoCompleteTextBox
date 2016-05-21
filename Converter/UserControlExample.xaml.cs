using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace converter
{
    /// <summary>
    /// Interaction logic for UserControlExample.xaml
    /// </summary>
    public partial class UserControlExample : Window
    {
        ObservableCollection<string> lstNames;
        ObservableCollection<string> filter;
        public UserControlExample()
        {
            InitializeComponent();
        }

        private void txtAutoCompleteTextBox_TextBoxContentChanged(object sender, RoutedEventArgs e)
        {
            Filter();
        }
        string s;
        private void Filter()
        {
            s = txtAutoCompleteTextBox.TextToChanged;
            filter = new ObservableCollection<string>(from name in lstNames
                                                          where (name.Contains(s))
                                                          select name);
                foreach (var item in filter)
                {
                    if (item.Equals(s))
                    {
                        txtAutoCompleteTextBox.TextToChanged = s;
                        return;
                    }
                }
                if (filter == null || filter.Count == 0 || txtAutoCompleteTextBox.txtAutoComplete.Text == "")
                {
                    txtAutoCompleteTextBox.lstData.Visibility = Visibility.Hidden;
                }
                else
                    if (filter != null && filter.Count > 0)
                    {
                        txtAutoCompleteTextBox.lstData.Visibility = Visibility.Visible;                        
                    }

                txtAutoCompleteTextBox.DataSource = filter;

                if (s == string.Empty)
                {
                    filter.Clear();
                    txtAutoCompleteTextBox.DataSource = filter;
                }
          }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lstNames = new ObservableCollection<string>();
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
        }
                private void txtAutoCompleteTextBox_btnDropClick(object sender, RoutedEventArgs e)
        {
            if (s != null && s != "")
            {
                Filter();
                ToggleVisibility();
            }
            else
            {
                txtAutoCompleteTextBox.DataSource = lstNames;
                ToggleVisibility();
            }
        }
        /// <summary>
        /// Toggle the visibility of a listbox
        /// </summary>
        private void ToggleVisibility()
        {
            if (txtAutoCompleteTextBox.btnDrop.IsChecked == true && lstNames!=null && lstNames.Count>0)
            {
                txtAutoCompleteTextBox.lstData.Visibility = Visibility.Visible;
            }
            else
            {
                txtAutoCompleteTextBox.lstData.Visibility = Visibility.Hidden;
            }
        }   
    }
}