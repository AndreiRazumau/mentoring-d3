using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;

namespace GameOfLife
{
    public partial class MainWindow : Window
    {
        private Grid mainGrid;
        DispatcherTimer timer;
        private int genCounter;
        private AdWindow[] adWindow;
        private static List<long> timing = new List<long>();

        public MainWindow()
        {
            InitializeComponent();
            this.mainGrid = new Grid(this.MainCanvas);

            this.timer = new DispatcherTimer();
            this.timer.Tick += this.OnTimer;
            this.timer.Interval = TimeSpan.FromMilliseconds(10);
        }

        private void StartAd()
        {
            {
                this.adWindow = new AdWindow[2];
                for (int i = 0; i < 2; i++)
                {
                    if (this.adWindow[i] == null)
                    {
                        this.adWindow[i] = new AdWindow(this);
                        this.adWindow[i].Closed += this.AdWindowOnClosed;
                        this.adWindow[i].Top = this.Top + (330 * i) + 70;
                        this.adWindow[i].Left = this.Left + 240;
                        this.adWindow[i].Show();
                    }
                }
            }
        }

        private void AdWindowOnClosed(object sender, EventArgs eventArgs)
        {
            for (int i = 0; i < 2; i++)
            {
                this.adWindow[i].Closed -= this.AdWindowOnClosed;
                this.adWindow[i] = null;
            }
        }

        private void Button_OnClick(object sender, EventArgs e)
        {
            if (!this.timer.IsEnabled)
            {
                this.timer.Start();
                this.ButtonStart.Content = "Stop";
                StartAd();
            }
            else
            {
                this.timer.Stop();
                this.ButtonStart.Content = "Start";
            }
        }

        private void OnTimer(object sender, EventArgs e)
        {
            var a = new Stopwatch();
            a.Start();
            this.mainGrid.Update();

            a.Stop();
            timing.Add(a.ElapsedMilliseconds);
            this.genCounter++;
            this.lblGenCount.Content = "Generations: " + this.genCounter;
        }

        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            this.mainGrid.Clear();
        }
    }
}
