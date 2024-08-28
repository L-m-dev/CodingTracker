    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace DatabaseLayer
    {
        public class CodingSession
        {
            public int UserId { get; set; }
            public int CodingSessionId { get; set; }
            public DateTime StartTime { get; set; }

            public DateTime EndTime { get; set; }
            public TimeSpan SessionDuration => EndTime - StartTime;

        public CodingSession(int userId, DateTime startTime, DateTime endTime)
        {
            this.UserId = userId;
            this.StartTime = startTime;
            this.EndTime = endTime;

        }
        public CodingSession() { }    

        }
    }
