using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace ViccosLite.Services.Tasks
{
    public class TaskThread : IDisposable
    {
        private bool _disposed;
        private Timer _timer;
        private readonly Dictionary<string, Task> _tasks;

        internal TaskThread()
        {
            _tasks = new Dictionary<string, Task>();
            Seconds = 10*60;
        }

        private void Run()
        {
            if (Seconds <= 0)
                return;

            StartedUtc = DateTime.UtcNow;
            IsRunning = true;
            foreach (Task task in _tasks.Values)
            {
                task.Execute();
            }
            IsRunning = false;
        }

        private void TimerHandler(object state)
        {
            _timer.Change(-1, -1);
            Run();
            if (RunOnlyOnce)
            {
                Dispose();
            }
            else
            {
                _timer.Change(Interval, Interval);
            }
        }

        public void Dispose()
        {
            if ((_timer != null) && !_disposed)
            {
                lock (this)
                {
                    _timer.Dispose();
                    _timer = null;
                    _disposed = true;
                }
            }
        }

        public void InitTimer()
        {
            if (_timer == null)
            {
                _timer = new Timer(new TimerCallback(TimerHandler), null, Interval, Interval);
            }
        }

        public void AddTask(Task task)
        {
            if (!_tasks.ContainsKey(task.Name))
            {
                _tasks.Add(task.Name, task);
            }
        }

        public int Seconds { get; set; }
        public DateTime StartedUtc { get; private set; }
        public bool IsRunning { get; private set; }

        public IList<Task> Tasks
        {
            get
            {
                var list = new List<Task>();
                foreach (var task in _tasks.Values)
                {
                    list.Add(task);
                }
                return new ReadOnlyCollection<Task>(list);
            }
        }

        public int Interval
        {
            get { return Seconds*1000; }
        }

        public bool RunOnlyOnce { get; set; }

        
    }
}