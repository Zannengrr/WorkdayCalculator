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
            int sign = Math.Sign(incrementInWorkdays);
            int days = (int)Math.Abs(incrementInWorkdays);
            decimal restDecimal = Math.Abs(incrementInWorkdays - Math.Truncate(incrementInWorkdays));
            decimal restHours = workingHours.Hours * restDecimal;
            decimal restMinutes = Math.Floor((restHours - Math.Truncate(restHours)) * 60);

            DateTime currentDate = startDate.Date;

            if (sign == 1)
            {
                if (startDate.Hour < endWorkingHours.Hours)
                {
                    int hoursInWorkDayLeft = startDate.Hour > startWorkingHours.Hours ? endWorkingHours.Hours - startDate.Hour : workingHours.Hours;
                    if (hoursInWorkDayLeft < restHours)
                    {
                        restHours = Math.Abs(restHours - hoursInWorkDayLeft) + 24;
                    }
                    if(startDate.Hour >= startWorkingHours.Hours) restMinutes += startDate.Minute;

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
                    int hoursInWorkDayLeft = startDate.Hour < endWorkingHours.Hours ? startWorkingHours.Hours - startDate.Hour : workingHours.Hours;
                    if (hoursInWorkDayLeft < restHours)
                    {
                        restHours = Math.Abs(restHours - hoursInWorkDayLeft) + 24;
                    }
                    if(startDate.Hour <= endWorkingHours.Hours) restMinutes = 60 - restMinutes;
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

            if(sign == 1) currentDate += new TimeSpan((int)restHours,(int)restMinutes, 0) + startWorkingHours;
            else currentDate += new TimeSpan(sign*(int)restHours, sign*(int)restMinutes, 0) + endWorkingHours;

            return currentDate;
        }

        public void SetHoliday(DateTime date)
        {
            holidays.Add(date.Date);
        }

        public void SetRecurringHoliday(int month, int day)
        {
            recurringHolidays.Add((month, day));
        }

        public void SetWorkdayStartAndStop(int startHours, int startMinutes, int stopHours, int stopMinutes)
        {
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
