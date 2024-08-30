using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace CodingTrackerApplication
{
    public class CodingSessionStopwatch
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public TimeSpan SessionDuration { get; set; }

        private Stopwatch _stopwatch;

        public CodingSessionStopwatch()
        {
        
        }
        public void StartCodingSessionStopWatch()
        {
            StartTime = DateTime.Now;
            _stopwatch = new Stopwatch();
            _stopwatch.Start();
        }

        public void StopCodingSessionStopWatch()
        {
            EndTime = DateTime.Now;
            _stopwatch.Stop();
            SessionDuration = _stopwatch.Elapsed;

        }

        public void ResetCodingSessionStopWatch()
        {
            StartTime = DateTime.MinValue;
            EndTime = DateTime.MinValue;
            _stopwatch = null;
        }


    }
}
