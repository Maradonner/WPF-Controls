using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для CustomColorSelectionControl.xaml
    /// </summary>
    public partial class CustomColorSelectionControl : UserControl
    {
        private int _count = 0;


        public CustomColorSelectionControl()
        {
            InitializeComponent();
            Dec.IsChecked = true;
            R.TextChange += RGB_TextChange;
            G.TextChange += RGB_TextChange;
            B.TextChange += RGB_TextChange;

            this.ColorChange += CustomColorSelectionControl_ColorChange;
        }

        private void CustomColorSelectionControl_ColorChange(object sender, EventArgs e)
        {
            _count++;
            CountLabel.Content = $"Количество изменений: {_count}";
        }


        private void Dec_Checked(object sender, RoutedEventArgs e)
        {

            R.ToDec();
            G.ToDec();
            B.ToDec();
            Hex.IsChecked = false;
        }

        private void Hex_Checked(object sender, RoutedEventArgs e)
        {

            R.ToHex();
            G.ToHex();
            B.ToHex();
            Dec.IsChecked = false;
        }


        public delegate void ColorChangeHandler(object sender, EventArgs e);
        public event ColorChangeHandler ColorChange;


        private void RGB_TextChange(object sender, EventArgs e)
        {
            bool flag = false;
            try
            {
                if (Dec.IsChecked == true && !R.Hex && !G.Hex && !B.Hex)
                {
                    Img.Fill = new SolidColorBrush(Color.FromRgb(
                                Convert.ToByte(R.Text),
                                Convert.ToByte(G.Text),
                                Convert.ToByte(B.Text)));
                    flag = true;
                }

                else if (Dec.IsChecked == false && R.Hex && G.Hex && B.Hex)
                {
                    Img.Fill = new SolidColorBrush(Color.FromRgb(
                                Convert.ToByte(Convert.ToString(int.Parse(R.Text, System.Globalization.NumberStyles.HexNumber), 10)),
                                Convert.ToByte(Convert.ToString(int.Parse(G.Text, System.Globalization.NumberStyles.HexNumber), 10)),
                                Convert.ToByte(Convert.ToString(int.Parse(B.Text, System.Globalization.NumberStyles.HexNumber), 10))));
                    flag = true;
                }

                if (flag)
                    ColorChange?.Invoke(sender, e);

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);

            }
        }

    }
}
