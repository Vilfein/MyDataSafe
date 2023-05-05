using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using System.Windows.Threading;

namespace MyDataSafe.Windows
{
    /// <summary>
    /// Interaction logic for PlayerWindow.xaml
    /// </summary>
    public partial class PlayerWindow : Window
    {

        DispatcherTimer DT = new DispatcherTimer();
        public PlayerWindow(string filePath)
        {
            Uri uri;

            InitializeComponent();
          //  ME.Source = new Uri(@"C:\Users\Vasek\source\repos\MyDataSafe\MyDataSafe\bin\Debug\net7.0-windows\TempFiles\ocean.wmv");

           if(File.Exists(filePath))
            {
               
                uri = new Uri(filePath);
                ME.Source = uri;
            }
             

           // MessageBox.Show("Zvolený soubor: " + ME.Source);


            foreach (UIElement item in (Content as Grid).Children)
            {
                item.Focusable = false;
            }

            PlayBtn.Click += (s, e) =>
            {
                DT.Start();
                ME.Play();

            };

            PauseBtn.Click += (s, e) =>
            {
                DT.Stop();
                ME.Pause();
            };

            StopBtn.Click += (s, e) =>
            {
                DT.Stop();
                ME.Stop();
            };
            DT.Interval = TimeSpan.FromMilliseconds(1);
            DT.Tick += (s, e) =>
            {
                Durat.Value = ME.Position.TotalMilliseconds;
            };

            VolSlider.ValueChanged += (s, e) =>
            {
                ME.Volume = e.NewValue;
            };

        }

        private void OnOpened(object sender, EventArgs e)
        {
            Durat.Maximum = ME.NaturalDuration.TimeSpan.TotalMilliseconds;
            TtE.Content = ME.NaturalDuration.TimeSpan.Seconds;
        }

        private void MediaStop(object sender, EventArgs e)
        {
            DT.Stop();
            ME.Stop();
        }

        private void TimeJump(object sender, KeyEventArgs e)
        {
            var Start = new DateTime(2020, 1, 1, ME.Position.Hours, ME.Position.Minutes, ME.Position.Seconds);

            if (e.Key == Key.Right)
                ME.Position = new TimeSpan(Start.Hour, Start.Minute, Start.AddSeconds(3).Second);


            else if (e.Key == Key.Left)
                ME.Position = new TimeSpan(Start.Hour, Start.Minute, Start.AddSeconds(-3).Second);


            else if (e.Key == Key.Up)
            {
                ME.Volume += 0.05;
                VolSlider.Value += 0.05;
            }


            else if (e.Key == Key.Down)
            {
                ME.Volume -= 0.05;
                VolSlider.Value -= 0.05;
            }

        }

        private void Durat_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                var Start = new DateTime(2020, 1, 1, ME.Position.Hours, ME.Position.Minutes, ME.Position.Seconds);
                var End = new DateTime(2020, 1, 1, 0, ME.Position.Hours, ME.NaturalDuration.TimeSpan.Seconds);
                TtE.Content = End - Start;
            }
            catch { }
        }


    }
}
