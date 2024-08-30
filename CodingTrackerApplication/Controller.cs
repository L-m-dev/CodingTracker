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
            bool invalidTransaction = false;

            //should return false in each of those.
            invalidTransaction = Validation.EndTimeBiggerThanStartTime(startTime, endTime);
            invalidTransaction = Validation.IsNullOrDefaultDateTime(startTime);
            invalidTransaction = Validation.IsNullOrDefaultDateTime(endTime);
            invalidTransaction = Validation.AreDatesEqual(startTime, endTime);

            // if invalidTransaction == true, should stop.
            if (invalidTransaction)
            {
                throw new ArgumentException("Invalid DateTime Arguments.");
            }

            TimeSpan sessionDuration = endTime - startTime;

            CodingSessionED codingSession = new CodingSessionED(_userId, startTime, endTime, sessionDuration);

            int codingSessionDBId = await CodingSessionRepository.AddCodingSessionAsync(codingSession);

            return codingSessionDBId;

        }
        public async Task<IEnumerable<CodingSessionED>> GetAllCodingSessionById(int id)
        {

            var codingSessionList = await CodingSessionRepository.GetByUserIdAsync(id);
            return codingSessionList;
        }


    }
}
