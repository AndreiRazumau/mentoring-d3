using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace GameOfLife
{
    class AdWindow : Window
    {
        private readonly DispatcherTimer adTimer;
        private int imgNmb;     // the number of the image currently shown
        private string link;    // the URL where the currently shown ad leads to


        public AdWindow(Window owner)
        {
            Random rnd = new Random();
            this.Owner = owner;
            this.Width = 350;
            this.Height = 100;
            this.ResizeMode = ResizeMode.NoResize;
            this.WindowStyle = WindowStyle.ToolWindow;
            this.Title = "Support us by clicking the ads";
            this.Cursor = Cursors.Hand;
            this.ShowActivated = false;
            MouseDown += this.OnClick;

            this.imgNmb = rnd.Next(1, 3);
            ChangeAds(this, new EventArgs());

            // Run the timer that changes the ad's image 
            this.adTimer = new DispatcherTimer();
            this.adTimer.Interval = TimeSpan.FromSeconds(3);
            this.adTimer.Tick += this.ChangeAds;
            this.adTimer.Start();
        }

        private void OnClick(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start(this.link);
            Close();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
        }

        public void Unsubscribe()
        {
            this.adTimer.Tick -= this.ChangeAds;
        }

        private void ChangeAds(object sender, EventArgs eventArgs)
        {

            ImageBrush myBrush = new ImageBrush();

            switch (this.imgNmb)
            {
                case 1:
                    myBrush.ImageSource =
                        new BitmapImage(new Uri("ad1.jpg", UriKind.Relative));
                    this.Background = myBrush;
                    this.link = "http://example.com";
                    this.imgNmb++;
                    break;
                case 2:
                    myBrush.ImageSource =
                        new BitmapImage(new Uri("ad2.jpg", UriKind.Relative));
                    this.Background = myBrush;
                    this.link = "http://example.com";
                    this.imgNmb++;
                    break;
                case 3:
                    myBrush.ImageSource =
                        new BitmapImage(new Uri("ad3.jpg", UriKind.Relative));
                    this.Background = myBrush;
                    this.link = "http://example.com";
                    this.imgNmb = 1;
                    break;
            }

        }
    }
}