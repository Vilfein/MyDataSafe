using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace MyDataSafe.Windows
{
    /// <summary>
    /// Interaction logic for Loading.xaml
    /// </summary>
    public partial class Loading : Window
    {
        DispatcherTimer timer;

        double angle = 0d;
        public Loading()
        {
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            InitializeComponent();
            timer.Tick += (s, e) =>
            {
                angle += 10d;

                imagen.RenderTransform = new RotateTransform(angle);
            };


            // Create source
            BitmapImage myBitmapImage = new BitmapImage();

            // BitmapImage.UriSource must be in a BeginInit/EndInit block
            myBitmapImage.BeginInit();
            myBitmapImage.UriSource = new Uri(@"pack://application:,,,/Icons/loading-process.png");

            myBitmapImage.DecodePixelWidth = 512;
            myBitmapImage.DecodePixelHeight = 512;
            myBitmapImage.EndInit();
            //set image source
            imagen.Source = myBitmapImage;

            timer.Start();
        }
    }
}
