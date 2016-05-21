using System;
using System.Collections;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace converter
{
    /// <summary>
    /// Interaction logic for AutoCompleteBox.xaml
    /// </summary>
    public partial class AutoCompleteBox : UserControl
    {
        public AutoCompleteBox()
        {
            InitializeComponent();
            txtAutoComplete.TextChanged += new TextChangedEventHandler(TxtAutoComplete_TextChanged);
            lstData.SelectionChanged += LstData_SelectionChanged;
            btnDrop.Click+=btnDrop_Click;
        }

        public string TextToChanged
        {
            get { return (string)GetValue(TextToChangedProperty); }
            set { SetValue(TextToChangedProperty, value); }
        }
        public ObservableCollection<string> DataSource
        {
            get { return (ObservableCollection<string>)GetValue(DataSourceProperty); }
            set { SetValue(DataSourceProperty, value); }
        }
        #region Region DependencyProperty
        // Using a DependencyProperty as the backing store for TextToChanged. // This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextToChangedProperty =
        DependencyProperty.Register("TextToChanged", typeof(string), typeof(AutoCompleteBox));

        // Using a DependencyProperty as the backing store for DataSource. // This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DataSourceProperty =
        DependencyProperty.Register("DataSource", typeof(ObservableCollection<string>), typeof(AutoCompleteBox));
        #endregion
        #region Region RoutedEventHandler
        public event RoutedEventHandler TextBoxContentChanged
        {
            add { AddHandler(TextBoxContentChangedEvent, value); }
            remove { RemoveHandler(TextBoxContentChangedEvent, value); }
        }

        public static readonly RoutedEvent TextBoxContentChangedEvent =
        EventManager.RegisterRoutedEvent("TextBoxContentChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(AutoCompleteBox));

        public event RoutedEventHandler btnDropClick
        {
            add { AddHandler(btnDropClickEvent, value); }
            remove { RemoveHandler(btnDropClickEvent, value); }
        }

        public static readonly RoutedEvent btnDropClickEvent =
        EventManager.RegisterRoutedEvent("btnDropClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(AutoCompleteBox));
        #endregion
        public bool ListFirst { get; set; }
        private void TxtAutoComplete_TextChanged(object sender, TextChangedEventArgs e)
        
        {
            e.Handled = true;
            TextToChanged = txtAutoComplete.Text;
            RoutedEventArgs args = new RoutedEventArgs(TextBoxContentChangedEvent);
            RaiseEvent(args);
        }
        
        private void LstData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            e.Handled = true;
            if (lstData.Visibility == Visibility.Visible && lstData.SelectedIndex!=-1)
            {
                TextToChanged = Convert.ToString(lstData.SelectedItem);
            }
        }
       
        private int _currentItemIndex=-1;
        private string txtEntered;
        ListBoxItem currentItem;
        bool __shiftPressed;
        
        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (txtAutoComplete.Text == "")
            {
                _currentItemIndex = -1;
                txtEntered = "";
            }
            if ( e.Key == Key.Back)
           {
               _currentItemIndex = -1;
               //var output = input.Substring(0, input.Length - 1);
               if (txtEntered != "") 
               txtEntered = txtEntered.Substring(0, txtEntered.Length - 1);
            }
            if (e.Key == Key.Delete)
            {
                txtEntered = txtAutoComplete.Text;
            }
            if (e.Key != Key.Down && e.Key != Key.Up)
            {
               char c='\0';
                // txtEntered = txtAutoComplete.Text;                
                bool capslock = Console.CapsLock;
                System.Windows.Input.Key key = e.Key;
                if (((e.Key == Key.LeftShift) || (e.Key == Key.RightShift)))
                {
                    __shiftPressed = true;
                    e.Handled = true;
                }
                if ((key >= Key.A) && (key <= Key.Z))
                {
                  if (__shiftPressed || capslock)
                    {
                            c = (char)((int)'A' + (int)(key - Key.A));
                            txtEntered += Convert.ToString(c);
                        __shiftPressed = false;
                    }
                    else
                    {
                        if ((key >= Key.A) && (key <= Key.Z))
                        {
                            c = (char)((int)'a' + (int)(key - Key.A));
                            txtEntered += Convert.ToString(c);
                        }
                    }
                }
                    if ((key >= Key.D0) && (key <= Key.D9))
                    {
                        c = (char)((int)'0' + (int)(key - Key.D0));
                        txtEntered += Convert.ToString(c);
                    }
                    if ((key >= Key.NumPad0) && (key <= Key.NumPad9))
                    {
                        c = (char)((int)'0' + (int)(key - Key.NumPad0));
                        txtEntered += Convert.ToString(c);
                    }
                }
                if (e.Key == Key.Up || e.Key == Key.Down)
                {
                    if (txtAutoComplete.Text == "")
                    {
                        lstData.SelectedIndex = 0;
                        return;
                    }

                    if (e.Key == Key.Up)
                    {
                        if (lstData.Visibility == Visibility.Hidden || txtAutoComplete.IsFocused)
                        {
                            return;
                        }
                        _currentItemIndex -= 1;
                        if (_currentItemIndex < 0)
                        {
                            TextToChanged = txtEntered;
                            txtAutoComplete.Focus();
                            txtAutoComplete.CaretIndex = txtAutoComplete.Text.Length;
                            //_currentItemIndex = lstData.Items.Count - 1;
                        }
                    }
                    else if (e.Key == Key.Down)
                    {
                        if (lstData.Visibility == Visibility.Hidden)
                        {
                            return;
                        }
                        if (_currentItemIndex == -1)
                            lstData.SelectedIndex = 0;
                        _currentItemIndex += 1;
                        if (_currentItemIndex > lstData.Items.Count - 1)
                        {
                            _currentItemIndex = 0;
                        }
                    }
                    if (_currentItemIndex < lstData.Items.Count && _currentItemIndex >= 0)
                    {
                        currentItem = (ListBoxItem)lstData.ItemContainerGenerator.ContainerFromIndex(_currentItemIndex);
                        if (currentItem != null)
                        {
                            currentItem.Focus();
                            //currentItem.Background = (Brush)colorConverter.ConvertFromString(_selectedRowColor);
                            currentItem.BringIntoView();
                            TextToChanged = Convert.ToString(currentItem.Content);
                        }
                    }
                    e.Handled = true;
                    return;
                }
                else if (e.Key == Key.Enter)
                {
                    if (_currentItemIndex > -1)
                    {
                        lstData.Visibility = Visibility.Hidden;
                        lstData.SelectedItem = lstData.Items[_currentItemIndex];
                        _currentItemIndex = -1;
                        txtAutoComplete.Focus();
                        txtAutoComplete.CaretIndex = txtAutoComplete.Text.Length;
                        txtEntered = Convert.ToString(currentItem.Content);
                    }
                }
                base.OnPreviewKeyDown(e);
            }

        private void btnDrop_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            RoutedEventArgs args = new RoutedEventArgs(btnDropClickEvent);
            RaiseEvent(args);
        }
        }
   }

