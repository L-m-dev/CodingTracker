using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingTrackerApplication
{
    public class CodingSession
    {
        private int _id;
        private DateTime _startTime;
        private DateTime _endTime;
        private TimeSpan _sessionDuration;

        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }
        public DateTime StartTime
        {
            get{return _startTime;}
            set { _startTime = value;
                UpdateSessionDuration();
            }
        }
        public DateTime EndTime
        {
            get { return _endTime; }
            set { _endTime = value;
                UpdateSessionDuration();
            }
        }

        public TimeSpan SessionDuration
        {
            get { return _sessionDuration;   }
        }

        private void UpdateSessionDuration()
        {
            _sessionDuration = _endTime - _startTime;
        }
    }
}
