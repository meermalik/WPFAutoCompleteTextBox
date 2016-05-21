using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace converter
{
    /// <summary>
    /// Interaction logic for DemoUserControl.xaml
    /// </summary>
    public partial class DemoUserControl : UserControl
    {
        public DemoUserControl()
        {
            InitializeComponent();
        }
        public string FileName
        {
            get { return txtFilename.Text; }
            set { txtFilename.Text = value; }
        }
        private void btnFileBrowse_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog OpenDialog = new Microsoft.Win32.OpenFileDialog();
            if(OpenDialog.ShowDialog()==true)
            {
                this.FileName = OpenDialog.FileName;
            }
        }
        private void txtFilename_TextChanged(object sender, TextChangedEventArgs e)
        {
            e.Handled = true;
          
        }
    }
}
