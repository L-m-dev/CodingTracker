    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace DatabaseLayer
    {
        public class CodingSessionED
        {
            public int UserId { get; set; }
            public int CodingSessionId { get; set; }
            public DateTime StartTime { get; set; }

            public DateTime EndTime { get; set; }
            public TimeSpan SessionDuration{ get; set; }

        // Constructor with auto-calculated TimeSpan
        public CodingSessionED(int userId, DateTime startTime, DateTime endTime)
        {
            this.UserId = userId;
            this.StartTime = startTime;
            this.EndTime = endTime;
            this.SessionDuration = endTime - startTime;

        // Constructor passing a TimeSpan product of calculating a stopwatch function
        } public CodingSessionED(int userId, DateTime startTime, DateTime endTime, TimeSpan sessionDuration)
        {
            this.UserId = userId;
            this.StartTime = startTime;
            this.EndTime = endTime;
            this.SessionDuration = sessionDuration;

        }
        public CodingSessionED() { }    

        }
    }
