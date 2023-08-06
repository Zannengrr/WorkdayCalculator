using WorkdayCalculator.Interfaces;

namespace WorkdayCalculator.Services
{
    public class WorkdayCalendar : IWorkdayCalendar
    {
        private readonly HashSet<DateTime> holidays;
        private readonly HashSet<(int month, int day)> recurringHolidays;
        private TimeSpan startWorkingHours;
        private TimeSpan endWorkingHours;
        private TimeSpan workingHours;

        public WorkdayCalendar()
        {
            holidays = new HashSet<DateTime>();
            recurringHolidays = new HashSet<(int, int)>();
        }

        public DateTime GetWorkdayIncrement(DateTime startDate, decimal incrementInWorkdays)
        {
            if(incrementInWorkdays == 0 ) return startDate;

            int sign = Math.Sign(incrementInWorkdays);
            int days = (int)Math.Abs(incrementInWorkdays);
            decimal decimalNumber = Math.Abs(incrementInWorkdays - Math.Truncate(incrementInWorkdays));
            decimal remainingWorkHours = workingHours.Hours * decimalNumber;
            decimal remainingWorkMinutes = Math.Floor((remainingWorkHours - Math.Truncate(remainingWorkHours)) * 60);

            DateTime currentDate = startDate.Date;

            //can be extracted into a method instead of repeating logic with differenet parameters
            if (sign == 1)
            {
                if (startDate.Hour < endWorkingHours.Hours)
                {
                    int hoursInWorkDayLeft = startDate.Hour > startWorkingHours.Hours ? Math.Abs(endWorkingHours.Hours - startDate.Hour) : workingHours.Hours;
                    if (hoursInWorkDayLeft < remainingWorkHours)
                    {
                        remainingWorkHours = 24 - Math.Abs(remainingWorkHours - hoursInWorkDayLeft);
                    }
                    if(startDate.Hour >= startWorkingHours.Hours) remainingWorkMinutes += startDate.Minute;

                }
                else
                {
                    currentDate = GetNextOrPreviousWorkday(currentDate, sign);
                }
            }
            else
            {
                if (startDate.Hour > startWorkingHours.Hours)
                {
                    int hoursInWorkDayLeft = startDate.Hour < endWorkingHours.Hours ? Math.Abs(startWorkingHours.Hours - startDate.Hour) : workingHours.Hours;
                    if (hoursInWorkDayLeft < remainingWorkHours)
                    {
                        remainingWorkHours = 24 + Math.Abs(remainingWorkHours - hoursInWorkDayLeft);
                    }
                    if (startDate.Hour < endWorkingHours.Hours) remainingWorkMinutes -= startDate.Minute;
                }
                else
                {
                    currentDate = GetNextOrPreviousWorkday(currentDate, sign);
                }
            }


            while (days > 0)
            {
                currentDate = GetNextOrPreviousWorkday(currentDate, sign);
                days--;
            }

            if (sign == 1) currentDate += new TimeSpan((int)remainingWorkHours, (int)remainingWorkMinutes, 0) + startWorkingHours;
            else
            {
                currentDate -= new TimeSpan((int)remainingWorkHours, (int)remainingWorkMinutes, 0) - endWorkingHours;
            }

            return currentDate;
        }

        public void SetHoliday(DateTime date)
        {
            holidays.Add(date.Date);
        }

        public void SetRecurringHoliday(int month, int day)
        {
            //validate inputs
            if(!(month is > 0 and <= 12)) throw new Exception("Months must be within 0 - 12");
            if (day < 1 || day > DateTime.DaysInMonth(2003, month)) throw new Exception($"Days must be within month days 1 - {DateTime.DaysInMonth(2003, month)}");

            recurringHolidays.Add((month, day));
        }

        public void SetWorkdayStartAndStop(int startHours, int startMinutes, int stopHours, int stopMinutes)
        {
            if (!(startHours is >= 0 and < 24 && stopHours is >= 0 and < 24)) 
                throw new Exception($"Hours must be within 0 - 24 - Your input: startHours = {startHours}\n endHours = {stopHours}");
            if (!(startMinutes is >= 0 and < 60 && stopMinutes is >= 0 and < 60)) 
                throw new Exception($"Minutes must be in range 0 - 59 Your input: startMinutes = {startMinutes}\n stopMinutes = {stopMinutes}");

            //maybe add validation that stop hours must be greater then end hours, but that is not specified

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
                return false;
            }

            foreach (var (month, day) in recurringHolidays)
            {
                if (date.Month == month && date.Day == day)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
