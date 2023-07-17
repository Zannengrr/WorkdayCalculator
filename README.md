# WorkdayCalculator

Task was to implement a calculator for work days in .NET Core.

The calculator takes a date (DateTime), a number of work days to add or subtract (decimal),
and returns a new date that is within business hours on a work day. Work days are defined as
any day from Monday to Friday that isn’t a holiday.

The following interface was implemented as a part of the task:
```
public interface IWorkdayCalendar
{
  void SetHoliday(DateTime date);
  void SetRecurringHoliday(int month, int day);
  void SetWorkdayStartAndStop(int startHours, int startMinutes,
  int stopHours, int stopMinutes);
  DateTime GetWorkdayIncrement(DateTime startDate, decimal
  incrementInWorkdays);
}
```
The core of the solution will be this method:
```
public DateTime GetWorkdayIncrement(Date startDate, decimal IncrementInWorkdays)
```
The method returns a DateTime between the business hours defined in
SetWorkdayStartAndStop, even when the startDate is outside of business hours. For instance, if
business hours are 8 to 16, 15:07 + 0,25 work days will be 09:07, and 04:00 + 0,5 work days
will be 12:00.

The class can be used like this:
```
IWorkdayCalendar calendar = new WorkdayCalendar();
calendar.SetWorkdayStartAndStop(8, 0, 16, 0);
calendar.SetRecurringHoliday(5, 17);
calendar.SetHoliday(new DateTime(2004, 5, 27));
string format = "dd-MM-yyyy HH:mm";
var start = new DateTime(2004, 5, 24, 18, 5, 0);
decimal increment = -5.5m;
var incrementedDate = calendar.GetWorkdayIncrement(start, increment);
Console.WriteLine(
start.ToString(format) +
" with an addition of " +
increment +
" work days is " +
incrementedDate.ToString(format));
```
