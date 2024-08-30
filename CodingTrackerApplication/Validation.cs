using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingTrackerApplication
{
    public static class Validation
    {
        public static bool EndTimeBiggerThanStartTime(DateTime start, DateTime end)
        {
            return (start > end);
        }

        public static bool IsNullOrDefaultDateTime(DateTime value)
        {
            if (value == null || value.Date == DateTime.MinValue)
            {
                return true;
            }
            return false;

        }
        public static bool AreDatesEqual(DateTime start, DateTime end)
        {
            if (start.Equals(end))
            {
                return true;
            }
            return false;
        }
    }
}


