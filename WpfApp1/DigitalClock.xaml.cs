using System;
using System.Windows.Controls;
using System.Windows.Threading;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для DigitalClock.xaml
    /// </summary>
    public partial class DigitalClock : UserControl
    {
        public DigitalClock()
        {
            InitializeComponent();
            InitializeClock();
        }

        public TimeZoneInfo ActualTimeZone { get; set; } = TimeZoneInfo.Local;

        private DateTime GetCurrentTimeInSelectedTimeZone()
        {
            TimeZoneInfo selectedTimeZone = ActualTimeZone;
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, selectedTimeZone);
        }


        private void InitializeClock()
        {
            DispatcherTimer timer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromSeconds(1),
            };
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            var currentTime = GetCurrentTimeInSelectedTimeZone();
            txtHours.Text = currentTime.ToString("hh");
            txtMinutes.Text = currentTime.ToString("mm");
            txtSeconds.Text = currentTime.ToString("ss");
        }

    }
}
