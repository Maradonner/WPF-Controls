using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitializeTimeZoneList();
            TimeZoneChoice.SelectionChanged += ComboBox_SelectionChanged;
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void Minimize_Button(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void Maximize_Button(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow.WindowState != WindowState.Maximized)
            {
                Application.Current.MainWindow.WindowState = WindowState.Maximized;
            }
            else
            {
                Application.Current.MainWindow.WindowState = WindowState.Normal;
            }
        }

        private void Close_Button(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void InitializeTimeZoneList()
        {
            TimeZoneChoice.ItemsSource = TimeZoneInfo.GetSystemTimeZones();
            TimeZoneChoice.SelectedValue = TimeZoneInfo.Local;
        }


        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox != null)
            {
                TimeZoneInfo timeZone = comboBox.SelectedItem as TimeZoneInfo;
                if (timeZone != null)
                {
                    DefaultClock.ActualTimeZone = timeZone;
                    DigitalClock.ActualTimeZone = timeZone;
                }
            }
        }

    }
}
