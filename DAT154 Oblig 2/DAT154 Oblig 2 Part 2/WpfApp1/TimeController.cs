using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Windows;

namespace WpfApp1
{
    public class MyTickArgs
    {
        public double Time { get; }
        public MyTickArgs(double time)
        {
            this.Time = time;
        }
    }
    class TimeController
    {
        private double counter;
        private double rate;
        private bool running; 
        private DispatcherTimer timer;
        public delegate void myTick(object sender, MyTickArgs e);
        public event myTick MYTICK;

        public TimeController()
        {
            this.counter = 0;
            this.rate = 0.01;
            this.timer = new DispatcherTimer();
            this.timer.Interval = TimeSpan.FromMilliseconds(16);
            this.timer.Tick += timerTick;
            this.running = true;
            this.timer.Start();
        }

        private void timerTick(object sender, EventArgs e)
        {
            this.counter += rate;
            MYTICK?.Invoke(sender, new MyTickArgs(counter));
        }
        public void StartStopTimer()
        {
            if(running)
            {
                this.running = false;
                this.timer.Stop();
            } else
            {
                this.running = true;
                this.timer.Start();
            }
            
        }
        public void increaseRate()
        {
            this.rate += 0.01;
        }
        public void decreaseRate()
        {
            this.rate -= 0.01;
        }
    }
}
