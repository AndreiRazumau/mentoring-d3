using System;

namespace AOP.Demo
{
    public class DemoContext
    {
        private bool _isCompleted = false;

        public string Name { get; set; }

        public DateTime Time { get; set; }

        public long Number { get; set; }

        public DemoContext(string name, DateTime time, long number = 100)
        {
            this.Name = name;
            this.Time = time;
            this.Number = number;
        }

        public void SetCompleted()
        {
            this._isCompleted = true;
        }
    }
}
