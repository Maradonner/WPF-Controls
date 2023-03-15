using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для RGBBox.xaml
    /// </summary>
    public partial class ColorTextBox : UserControl
    {
        public ColorTextBox()
        {
            InitializeComponent();
        }

        public string Text
        {
            get => text.Text;
            set
            {
                text.Text = value;
            }
        }

        public bool Hex { get; set; } = false;

        public delegate void TextChangeHandler(object sender, EventArgs e);
        public event TextChangeHandler TextChange;



        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Text.Contains(" "))
                Text = Text.Replace(" ", "");

            if (string.IsNullOrWhiteSpace(Text))
                Text = "0";

            if (this.Hex)
            {
                if (int.Parse(Text, System.Globalization.NumberStyles.HexNumber) > 255)
                    Text = "FF";
            }
            else
            {
                if (int.Parse(Text) > 255)
                    Text = "255";
            }
            TextChange?.Invoke(sender, e);
        }

        public void ToHex()
        {
            this.Hex = true;
            Text = Convert.ToInt32(Text).ToString("X2");
        }

        public void ToDec()
        {
            this.Hex = false;
            Text = Convert.ToInt32(Text, 16).ToString();
        }

        //private void Paste_Click(object sender, RoutedEventArgs e)
        //{
        //    if (Clipboard.GetText() is string text && IsValidInput(text))
        //    {
        //        int newValue = Hex ? Convert.ToInt32(text, 16) : Convert.ToInt32(text);
        //        Text = newValue.ToString();
        //    }
        //}

        private void Paste_Click(object sender, RoutedEventArgs e)
        {
            string clipboardText = Clipboard.GetText();
            bool hasValidChars = Regex.IsMatch(clipboardText, "[A-Z0-9]", RegexOptions.IgnoreCase);

            if (!hasValidChars)
            {
                MessageBox.Show("Clipboard contains invalid characters!");
                return;
            }
            if (int.TryParse(clipboardText, out int parsedValue))
            {
                string outputText = Hex ? parsedValue.ToString("X") : parsedValue.ToString();
                text.Text = outputText;
            }
            else
            {
                MessageBox.Show("Clipboard does not contain a valid integer!");
            }
        }

        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(Text);
        }

        private bool IsValidInput(string text)
        {
            if (Hex && int.TryParse(text, System.Globalization.NumberStyles.HexNumber, null, out int hexValue))
            {
                return hexValue <= 0xFF;
            }
            else if (!Hex && int.TryParse(text, out int decValue))
            {
                return decValue <= 255;
            }
            else
            {
                MessageBox.Show("Содержит недопустимые символы!");
                return false;
            }
        }

        private void text_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control && e.Key == Key.V)
            {
                Paste_Click(sender, e); 
                e.Handled = true;
            }
            else if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control && e.Key == Key.C)
            {
                Copy_Click(sender, e);
                e.Handled = true;
            }
            else if (Hex && !(e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.A && e.Key <= Key.F) && e.Key != Key.Delete && e.Key != Key.Back)
            {
                e.Handled = true;
            }
            else if (!Hex && !(e.Key >= Key.D0 && e.Key <= Key.D9) && e.Key != Key.Delete && e.Key != Key.Back)
            {
                e.Handled = true;
            }
        }
        


    }
}
