using WorkdayCalculator.Interfaces;

namespace WorkdayCalculator.Services
{
    public class WorkdayCalendarV2 : IWorkdayCalendar
    {
        private readonly HashSet<DateTime> holidays;
        private readonly HashSet<(int month, int day)> recurringHolidays;
        private TimeSpan startWorkingHours;
        private TimeSpan endWorkingHours;
        private TimeSpan workingHours;

        public WorkdayCalendarV2()
        {
            holidays = new HashSet<DateTime>();
            recurringHolidays = new HashSet<(int, int)>();
        }

        //TODO try to solve it using just math
        public DateTime GetWorkdayIncrement(DateTime startDate, decimal incrementInWorkdays)
        {
            int sign = Math.Sign(incrementInWorkdays);
            int days = (int)Math.Abs(incrementInWorkdays);
            decimal restDecimal = Math.Abs(incrementInWorkdays - Math.Truncate(incrementInWorkdays));
            decimal restHoursDecimal = workingHours.Hours * restDecimal;
            decimal restMinutesDecimal = (restHoursDecimal - Math.Truncate(restHoursDecimal)) * 60;

            int hoursRemaining = (int)restHoursDecimal;
            int minutesRemaining = (int)restMinutesDecimal;
            int startWorkdayHours = sign == 1 ? startWorkingHours.Hours : endWorkingHours.Hours;
            int endWorkdayHours = sign == 1 ? endWorkingHours.Hours : startWorkingHours.Hours;

            DateTime currentDate = startDate.Date;

            int hoursTillEndOfDay = endWorkdayHours - startDate.Hour;

            if (hoursTillEndOfDay > 0)
            {
                hoursRemaining -= (hoursRemaining - hoursTillEndOfDay) + hoursTillEndOfDay > hoursRemaining ? 24 : 0;
                minutesRemaining += hoursTillEndOfDay <= workingHours.Minutes ? startDate.Minute : 0;
            }
            else
            {
                currentDate = GetNextOrPreviousWorkday(currentDate, sign);
            }

            while (days > 0)
            {
                currentDate = GetNextOrPreviousWorkday(currentDate, sign);
                days--;
            }

            TimeSpan spillover = new((hoursRemaining + sign * startWorkdayHours) * sign, minutesRemaining * sign, 0);
            currentDate += spillover;

            return currentDate;
        }

        public void SetHoliday(DateTime date)
        {
            holidays.Add(date.Date);
        }

        public void SetRecurringHoliday(int month, int day)
        {
            //validate inputs
            if (!(month is > 0 and <= 12)) throw new Exception("Months must be within 0 - 12");
            if (day < 1 || day > DateTime.DaysInMonth(2003, month)) throw new Exception($"Days must be within month days 1 - {DateTime.DaysInMonth(2003, month)}");
            recurringHolidays.Add((month, day));
        }

        public void SetWorkdayStartAndStop(int startHours, int startMinutes, int stopHours, int stopMinutes)
        {
            if (!(startHours is >= 0 and < 24 && stopHours is >= 0 and < 24))
                throw new Exception($"Hours must be within 0 - 24 - Your input: startHours = {startHours}\n endHours = {stopHours}");
            if (!(startMinutes is >= 0 and < 60 && stopMinutes is >= 0 and < 60))
                throw new Exception($"Minutes must be in range 0 - 59 Your input: startMinutes = {startMinutes}\n stopMinutes = {stopMinutes}");

            startWorkingHours = new TimeSpan(startHours, startMinutes, 0);
            endWorkingHours = new TimeSpan(stopHours, stopMinutes, 0);
            workingHours = endWorkingHours - startWorkingHours;
        }

        private DateTime GetNextOrPreviousWorkday(DateTime date, int sign)
        {
            DateTime nextDate = date;
            do
            {
                nextDate = nextDate.AddDays(sign);
            }
            while (!IsWorkday(nextDate));

            return nextDate;
        }

        private bool IsWorkday(DateTime date)
        {
            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) return false;

            if (holidays.Contains(date))
            {
                Console.WriteLine("Holdiays found");
                return false;
            }

            foreach (var holiday in recurringHolidays)
            {
                if (date.Month == holiday.month && date.Day == holiday.day)
                {
                    Console.WriteLine("Holdiays MATHC FOUND");
                    return false;
                }
            }

            return true;
        }
    }
}
