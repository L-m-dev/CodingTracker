using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseLayer;
using System.Diagnostics;

namespace CodingTrackerApplication
{
    public class Controller
    {
        private int _userId;
        public Stopwatch Stopwatch;
        public CodingSessionRepository CodingSessionRepository { get; set; }
        public Controller(int userId, CodingSessionRepository codingSessionRepository)
        {
            this._userId = userId;
            this.CodingSessionRepository = codingSessionRepository;
            Stopwatch = new Stopwatch();
        }

        public async Task<int> CreateCodingSession(DateTime startTime, DateTime endTime)
        {
            bool validTransaction = true;
            validTransaction = Validation.EndTimeBiggerThanStartTime(startTime, endTime);
            validTransaction = Validation.IsNullOrDefaultDateTime(startTime);
            validTransaction = Validation.IsNullOrDefaultDateTime(endTime);

            if (!validTransaction)
            {
                throw new ArgumentException("Invalid DateTime");
            }
            TimeSpan sessionDuration = endTime - startTime;

            CodingSessionED codingSession = new CodingSessionED(_userId, startTime, endTime, sessionDuration);

            int codingSessionDBId = await CodingSessionRepository.AddCodingSessionAsync(codingSession);

            return codingSessionDBId;

        }


    }
}
