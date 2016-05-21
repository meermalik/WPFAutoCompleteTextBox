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
using System.Collections.ObjectModel;
using converter;

namespace converter
{
    /// <summary>
    /// Interaction logic for AutoCompleteBox1.xaml
    /// </summary>
    public partial class AutoCompleteBox1 : UserControl
    {
        private int _currentItemIndex = -1;
        private string txtEntered;
        ListBoxItem currentItem;
        bool __shiftPressed;
        bool IsMouseDown;
        bool IsListItemEntered;
        string s;
        int listSelectedIndex;
        Window parentWindow;
        ObservableCollection<string> filter;
        IEnumerable<string> lstNames;

        public AutoCompleteBox1()
        {
            InitializeComponent();
            txtAutoComplete.KeyUp += new KeyEventHandler(txtAutoComplete_KeyUp);
        }
        public Window FindParentWindow(DependencyObject child)
        {
            DependencyObject parent = VisualTreeHelper.GetParent(child);
            //DependencyObject RootParent = VisualTreeHelper.GetParent(parent);
            //CHeck if this is the end of the tree
            if (parent == null) return null;
            parentWindow = parent as Window;
            if (parentWindow != null)
            {
                parentWindow.MouseDown += new MouseButtonEventHandler(parentWindow_MouseDown);
                return parentWindow;
            }
            else
            {
                //use recursion until it reaches a Window
                return FindParentWindow(parent);
            }
        }
        private void parentWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            lstData.Visibility = Visibility.Hidden;
        }

        public IEnumerable<string> ItemsSource
        {
            get { return (IEnumerable<string>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public int Width
        {
            get { return (int)GetValue(WidthProperty); }
            set { SetValue(WidthProperty, value); }
        }

        public int Height
        {
            get { return (int)GetValue(HeightProperty); }
            set { SetValue(HeightProperty, value); }
        }
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        #region Region DependencyProperty
        // Using a DependencyProperty as the backing store for DataSource. // This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsSourceProperty =
        DependencyProperty.Register("ItemsSource", typeof(IEnumerable<string>), typeof(AutoCompleteBox1));

        public static readonly DependencyProperty WidthProperty =
        DependencyProperty.Register("Width", typeof(int), typeof(AutoCompleteBox1));
        public static readonly DependencyProperty HeightProperty =
        DependencyProperty.Register("Height", typeof(int), typeof(AutoCompleteBox1));
        public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register("Text", typeof(string), typeof(AutoCompleteBox1));
        #endregion

        #region Region RoutedEventHandler

        public event RoutedEventHandler TextChanged
        {
            add { AddHandler(TextBoxContentChangedEvent, value); }
            remove { RemoveHandler(TextBoxContentChangedEvent, value); }
        }

        public static readonly RoutedEvent TextBoxContentChangedEvent =
        EventManager.RegisterRoutedEvent("TextChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(AutoCompleteBox1));

        public event RoutedEventHandler KeyUp
        {
            add { AddHandler(KeyUpEvent, value); }
            remove { RemoveHandler(KeyUpEvent, value); }
        }

        public static readonly RoutedEvent KeyUpEvent =
        EventManager.RegisterRoutedEvent("KeyUp", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(AutoCompleteBox1));

        # endregion
        private void LstData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            e.Handled = true;
            if (IsMouseDown == true)
            {
                txtAutoComplete.Text = Convert.ToString(lstData.SelectedItem);
                txtEntered = txtAutoComplete.Text;
                lstData.Visibility = Visibility.Hidden;
                txtAutoComplete.Focus();
                txtAutoComplete.CaretIndex = txtAutoComplete.Text.TrimEnd('\r', '\n').Length;
                IsMouseDown = false;
            }
            if (lstData.Visibility == Visibility.Visible && lstData.SelectedIndex != -1)
            {
                txtAutoComplete.Text = Convert.ToString(lstData.SelectedItem);
                listSelectedIndex = lstData.SelectedIndex;
            }
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (txtAutoComplete.Text == "")
            {
                _currentItemIndex = -1;
                txtEntered = "";
            }
            if (e.Key == Key.Back && txtAutoComplete.IsFocused == true)
            {
                _currentItemIndex = -1;
                if (txtEntered != "")
                    txtEntered = txtEntered.Substring(0, txtEntered.Length - 1);
                lstData.SelectedIndex = -1;
            }
            if (e.Key == Key.Delete)
            {
                txtEntered = txtAutoComplete.Text;
            }
            if (e.Key != Key.Down && e.Key != Key.Up)
            {
                char c = '\0';
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
                if (e.Key == Key.Up)
                {
                    if (lstData.Visibility == Visibility.Hidden || txtAutoComplete.IsFocused)
                    {
                        return;
                    }
                    _currentItemIndex -= 1;
                    if (_currentItemIndex < 0)
                    {
                        txtAutoComplete.Text = txtEntered;
                        txtAutoComplete.Focus();
                        txtAutoComplete.CaretIndex = txtAutoComplete.Text.TrimEnd('\r', '\n').Length;
                        //_currentItemIndex = lstData.Items.Count - 1;
                    }
                }
                else if (e.Key == Key.Down)
                {
                    //if (txtAutoComplete.Text == "")
                    //{
                    //    lstData.SelectedIndex = 0;
                    //    //_currentItemIndex++;
                    //    return;
                    //}
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
                        //txtAutoComplete.Focus();
                        //txtAutoComplete.CaretIndex = txtAutoComplete.Text.Length;
                    }
                }
                if (_currentItemIndex < lstData.Items.Count && _currentItemIndex >= 0)
                {
                    currentItem = (ListBoxItem)lstData.ItemContainerGenerator.ContainerFromIndex(_currentItemIndex);
                    if (currentItem != null)
                    {
                        currentItem.Focus();
                        //currentItem.Background = (Brush)colorConverter.ConvertFromString("");
                        currentItem.BringIntoView();
                        txtAutoComplete.Text = Convert.ToString(currentItem.Content);
                    }
                }
                e.Handled = true;
                return;
            }
            else if (e.Key == Key.Enter)
            {
                if (_currentItemIndex > -1)
                {
                    IsListItemEntered = true;
                    lstData.Visibility = Visibility.Hidden;
                    lstData.SelectedItem = lstData.Items[_currentItemIndex];
                    _currentItemIndex = -1;
                    txtAutoComplete.Focus();
                    txtAutoComplete.CaretIndex = txtAutoComplete.Text.TrimEnd('\r', '\n').Length;
                    txtEntered = Convert.ToString(currentItem.Content);
                }
            }
            base.OnPreviewKeyDown(e);
        }
        private void Filter()
       {
            s = txtAutoComplete.Text.Trim();
            if (lstNames == null)
            {
                return;
            }
            filter = new ObservableCollection<string>(from name in lstNames
                                                      where (name.ToLower().Contains(s.ToLower()))
                                                      select name);
            foreach (var item in filter)
            {
                if (item.Equals(s))
                {
                    txtAutoComplete.Text = s;
                    return;
                }
            }
            if (string.IsNullOrWhiteSpace(txtAutoComplete.Text))
            {
                lstData.ItemsSource = lstNames;
                lstData.Visibility = Visibility.Visible;
                return;
            }
            else
            if (filter == null || filter.Count == 0)
            {
                lstData.Visibility = Visibility.Hidden;

            }
            else
              if (filter != null && filter.Count > 0)
            {
                lstData.Visibility = Visibility.Visible;
            }

            lstData.ItemsSource = filter;

            if (s == string.Empty)
            {
                filter.Clear();
                lstData.ItemsSource = filter;
            }
        }

        private void btnDrop_Click(object sender, RoutedEventArgs e)
        {
            if (s != null && s != "")
            {
                Filter();
                lstData.ItemsSource = filter;
                ToggleVisibility();
            }
            else
            {
                lstData.ItemsSource = lstNames;
                ToggleVisibility();
            }

        }
        private void ToggleVisibility()
        {
            if (btnDrop.IsChecked == true && lstNames != null)
            {
                lstData.Visibility = Visibility.Visible;
            }
            else
            {
                lstData.Visibility = Visibility.Hidden;
            }
        }

        private void txtAutoComplete_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            e.Handled = true;
            if (IsListItemEntered == true)
            {
                txtAutoComplete.Text = txtAutoComplete.Text.TrimEnd('\r', '\n');
                txtAutoComplete.CaretIndex = txtAutoComplete.Text.Length;
                IsListItemEntered = false;
            }
            Filter();
            RoutedEventArgs args = new RoutedEventArgs(TextBoxContentChangedEvent);
            RaiseEvent(args);
        }

        private void txtAutoComplete_KeyUp(object sender, KeyEventArgs e)
        {
            e.Handled = true;
            RoutedEventArgs args = new RoutedEventArgs(KeyUpEvent);
            RaiseEvent(args);
        }

        private void rootcontrol_Loaded(object sender, RoutedEventArgs e)
        {
            lstNames = ItemsSource;
            FindParentWindow(rootcontrol);
            lstData.Width = txtAutoComplete.Width + btnDrop.Width + 2;
        }

        private void lstData_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            IsMouseDown = true;
            if (lstData.Items.Count == 1 && lstData.SelectedIndex >= 0)
            {
                txtAutoComplete.Text = Convert.ToString(lstData.SelectedItem);
                txtEntered = txtAutoComplete.Text;
                txtAutoComplete.Focus();
                //string text = txtAutoComplete.Text.TrimEnd('\r', '\n');
                txtAutoComplete.CaretIndex = txtAutoComplete.Text.TrimEnd('\r', '\n').Length;
                lstData.Visibility = Visibility.Hidden;
                IsMouseDown = false;
            }
        }



    }
}
