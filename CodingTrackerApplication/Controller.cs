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

        public Dictionary<string, TimeSpan> GetStatisticsList(IEnumerable<CodingSessionED> codingSessionList)
        {

            Dictionary<string, TimeSpan> codingSessionTotalTimeDict = new Dictionary<string, TimeSpan>();
          
            int[] dayRangeList = { 1, 7, 30, 365 };

            for (int i = 0; i < dayRangeList.Length; i++)
            {
                TimeSpan sumOfDuration = TimeSpan.Zero;
                var average = TimeSpan.Zero;
                int dayRange = dayRangeList[i];
                foreach (var codingSession in codingSessionList)
                {
                    sumOfDuration += CalculateSessionDuration(codingSession, dayRange);
                }
                codingSessionTotalTimeDict.Add($"{dayRangeList[i]}DayTotal", sumOfDuration);
                if (i > 0)
                {
                    average = sumOfDuration / dayRange;
                    codingSessionTotalTimeDict.Add($"{dayRangeList[i]}DayAverage", average);
                }
            }
            return codingSessionTotalTimeDict;


        }

        //dayCount is the days to calculate.
        //passing the argument 7 will return how much time was spent last 7 days.
        public TimeSpan CalculateSessionDuration(CodingSessionED codingSessionED, int dayRange)
        {
            DateTime endTimeCalculationBoundary = DateTime.Now;
            DateTime startTimeCalculationBoundary = DateTime.Now.AddDays(-dayRange);
            //if Session started before the period, the minimum boundary CONTINUES being "1 day ago", as set above.
            // if Session started after, the boundary becomes the Session's StartTime
            if (codingSessionED.StartTime >= startTimeCalculationBoundary)
            {
                startTimeCalculationBoundary = codingSessionED.StartTime;
            }
            //if Session ended before boundary period, it shouldn't count.
            if (codingSessionED.EndTime < startTimeCalculationBoundary)
            {
                return TimeSpan.Zero;
            }
            else
            {
                endTimeCalculationBoundary = codingSessionED.EndTime;
            }
            return endTimeCalculationBoundary - startTimeCalculationBoundary;

        }
        public string FormatTimeSpan(TimeSpan time)
        {
            string result = string.Empty;
            if (time.Days > 0)
            {
                if (time.Days == 1) {
                    result += $"{time.Days} day ";
                }
                else
                {
                    result += $"{time.Days} days ";
                }
            }
            result+=$"{time.Hours} hours {time.Minutes} minutes {time.Seconds} seconds ";
            return result;
        }
    }
}
