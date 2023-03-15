using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для Clock.xaml
    /// </summary>
    public partial class Clock : UserControl
    {
        const double NumberSystem = 60;
        const double baseAngleNumberSystem = 360 / NumberSystem;
        const double baseAngleHour = 30;

        public Clock()
        {
            InitializeComponent();
            InitializeClock();
            InitializeHourMarks();
            InitializeMinuteMarks();
        }

        public TimeZoneInfo ActualTimeZone { get; set; } = TimeZoneInfo.Local;

        private void InitializeClock()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }



        private void Timer_Tick(object sender, EventArgs e)
        {
            Debug.WriteLine(ActualTimeZone);
            DateTime currentTime = GetCurrentTimeInSelectedTimeZone();

            var rotateSecondArrow = new RotateTransform();
            var rotateMinuteArrow = new RotateTransform();
            var rotateHourArrow = new RotateTransform();

            rotateSecondArrow.Angle = baseAngleNumberSystem * currentTime.Second;
            SecondArrow.RenderTransform = rotateSecondArrow;

            rotateMinuteArrow.Angle = (currentTime.Minute * baseAngleNumberSystem) + (rotateSecondArrow.Angle / 60.0);
            MinuteArrow.RenderTransform = rotateMinuteArrow;

            rotateHourArrow.Angle = (currentTime.Hour - 12) * baseAngleHour + rotateMinuteArrow.Angle / 12;
            HourArrow.RenderTransform = rotateHourArrow;
        }

        private DateTime GetCurrentTimeInSelectedTimeZone()
        {
            TimeZoneInfo selectedTimeZone = ActualTimeZone;
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, selectedTimeZone);
        }

        private void InitializeMinuteMarks()
        {
            for (int i = 0; i < 60; i++)
            {
                if (i % 5 == 0)
                {
                    continue;
                }

                Border tick = CreateTickBrush(Brushes.Red, this.Width / 60);
                RotateTransform rotateTick = new RotateTransform(i * 6);
                tick.RenderTransform = rotateTick;
                ClockFace.Children.Add(tick);
            }
        }

        private void InitializeHourMarks()
        {
            for (int i = 0; i < 12; i++)
            {
                Border tick = CreateTickBrush(Brushes.Black, this.Width / 75);
                RotateTransform rotateTick = new RotateTransform(i * 30);
                tick.RenderTransform = rotateTick;
                ClockFace.Children.Add(tick);

                Border number = CreateNumberBrush(i);
                ClockFace.Children.Add(number);
            }
        }

        private Border CreateTickBrush(Brush color, double width)
        {
            Border tick = new Border()
            {
                Height = this.Height / 300,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                RenderTransformOrigin = new Point(0.5, 0.5)
            };

            Border tickLine = new Border()
            {
                Width = width,
                Background = color,
                HorizontalAlignment = HorizontalAlignment.Right,
                BorderBrush = Brushes.Black
            };

            tick.Child = tickLine;
            return tick;
        }

        private Border CreateNumberBrush(int i)
        {
            Border border = new Border();
            border.HorizontalAlignment = HorizontalAlignment.Center;
            border.VerticalAlignment = VerticalAlignment.Stretch;
            border.RenderTransformOrigin = new Point(0.5, 0.5);

            TransformGroup transformGroup = new TransformGroup();
            RotateTransform rotateTransform = new RotateTransform();
            rotateTransform.Angle = 30 * i;
            transformGroup.Children.Add(rotateTransform);

            border.RenderTransform = transformGroup;

            TextBlock textBlock = new TextBlock();
            textBlock.Text = $"{i}";
            textBlock.FontSize = 30;
            textBlock.RenderTransformOrigin = new Point(0.5, 0.5);
            textBlock.VerticalAlignment = VerticalAlignment.Top;
            textBlock.HorizontalAlignment = HorizontalAlignment.Stretch;

            TransformGroup textBlockTransformGroup = new TransformGroup();
            RotateTransform textBlockRotateTransform = new RotateTransform();
            textBlockRotateTransform.Angle = -30 * i;
            textBlockTransformGroup.Children.Add(textBlockRotateTransform);

            textBlock.RenderTransform = textBlockTransformGroup;

            border.Child = textBlock;

            return border;
        }

        private void slc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
