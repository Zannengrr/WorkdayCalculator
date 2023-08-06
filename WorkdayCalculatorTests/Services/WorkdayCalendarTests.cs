using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkdayCalculator.Interfaces;

namespace WorkdayCalculator.Services.Tests
{
    [TestClass()]
    public class WorkdayCalendarTests
    {
        private readonly IWorkdayCalendar _calendar;

        private (int month, int day) reccuringHoliday = (5, 17);
        private readonly DateTime holiday = new(2004, 5, 27);

        public WorkdayCalendarTests()
        {
            _calendar = new WorkdayCalendar();
        }

        [TestMethod()]
        public void WorkdayCalendarTest()
        {
            Assert.IsNotNull(_calendar);
        }

        [TestMethod()]
        [DataRow(13, 0)]
        public void ReccuringHolidayMonthLargerThen12(int month, int day)
        {
            Assert.ThrowsException<Exception>(() => _calendar.SetRecurringHoliday(month, day));
        }

        [TestMethod()]
        [DataRow(-1, 0)]
        public void ReccuringHolidayMonthNegative(int month, int day)
        {
            Assert.ThrowsException<Exception>(() => _calendar.SetRecurringHoliday(month, day));
        }

        [TestMethod()]
        [DataRow(11, -5)]
        public void ReccuringHolidayDayNegative(int month, int day)
        {
            Assert.ThrowsException<Exception>(() => _calendar.SetRecurringHoliday(month, day));
        }

        [TestMethod()]
        [DataRow(11, 70)]
        public void ReccuringHolidayDayLargerThenAnyDayOfMonth(int month, int day)
        {
            Assert.ThrowsException<Exception>(() => _calendar.SetRecurringHoliday(month, day));
        }

        [TestMethod()]
        [DataRow(2, 30)]
        public void ReccuringHolidayDayLargerThenNumberOfDayInAMonth(int month, int day)
        {
            Assert.ThrowsException<Exception>(() => _calendar.SetRecurringHoliday(month, day));
        }

        //TODO
        [TestMethod()]
        [DataRow(-8, 0, 16, 0)]
        public void StartHourIsANegativeNumber(int startHour, int startMinutes, int stopHour, int stopMinutes)
        {
            Assert.ThrowsException<Exception>(() => _calendar.SetWorkdayStartAndStop(startHour, startMinutes, stopHour, stopMinutes));
        }

        [TestMethod()]
        [DataRow(8, 0, -16, 0)]
        public void StopHourIsANegativeNumber(int startHour, int startMinutes, int stopHour, int stopMinutes)
        {
            Assert.ThrowsException<Exception>(() => _calendar.SetWorkdayStartAndStop(startHour, startMinutes, stopHour, stopMinutes));
        }

        [DataRow(38, 0, 16, 0)]
        public void StartHourIsTooLargeNumber(int startHour, int startMinutes, int stopHour, int stopMinutes)
        {
            Assert.ThrowsException<Exception>(() => _calendar.SetWorkdayStartAndStop(startHour, startMinutes, stopHour, stopMinutes));
        }

        [DataRow(8, 0, 36, 0)]
        public void StopHourIsTooLargeNumber(int startHour, int startMinutes, int stopHour, int stopMinutes)
        {
            Assert.ThrowsException<Exception>(() => _calendar.SetWorkdayStartAndStop(startHour, startMinutes, stopHour, stopMinutes));
        }

        [DataRow(8, -1, 36, 0)]
        public void StartMinuteIsNegativeNumber(int startHour, int startMinutes, int stopHour, int stopMinutes)
        {
            Assert.ThrowsException<Exception>(() => _calendar.SetWorkdayStartAndStop(startHour, startMinutes, stopHour, stopMinutes));
        }

        [DataRow(8, 100, 36, 0)]
        public void StartMinuteIsTooLargeNumber(int startHour, int startMinutes, int stopHour, int stopMinutes)
        {
            Assert.ThrowsException<Exception>(() => _calendar.SetWorkdayStartAndStop(startHour, startMinutes, stopHour, stopMinutes));
        }

        [DataRow(8, 0, 36, 100)]
        public void StopMinuteIsTooLargeNumber(int startHour, int startMinutes, int stopHour, int stopMinutes)
        {
            Assert.ThrowsException<Exception>(() => _calendar.SetWorkdayStartAndStop(startHour, startMinutes, stopHour, stopMinutes));
        }

        [DataRow(8, 0, 36, -100)]
        public void StopMinuteIsNegativeNumber(int startHour, int startMinutes, int stopHour, int stopMinutes)
        {
            Assert.ThrowsException<Exception>(() => _calendar.SetWorkdayStartAndStop(startHour, startMinutes, stopHour, stopMinutes));
        }

        [TestMethod()]
        [DataRow(8, 0, 16, 0)]
        public void GetWorkdayIncrementTest(int startHours, int startMinutes, int endHours, int endMinutes)
        {
            //Arrange
            DateTime start = new(2004, 5, 24, 18, 5, 0);
            decimal incrementInWorkdays = -5.5m;
            DateTime expectedResult = new(2004, 5, 14, 12, 0, 0);

            //Act
            _calendar.SetWorkdayStartAndStop(startHours, startMinutes, endHours, endMinutes);
            _calendar.SetRecurringHoliday(reccuringHoliday.month, reccuringHoliday.day);
            _calendar.SetHoliday(holiday);

            DateTime result = _calendar.GetWorkdayIncrement(start, incrementInWorkdays);

            //Assert
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod()]
        [DataRow(8, 0, 16, 0)]
        public void GetWorkdayIncrementTest2(int startHours, int startMinutes, int endHours, int endMinutes)
        {
            //Arrange
            DateTime start = new(2004, 5, 24, 19, 3, 0);
            decimal incrementInWorkdays = 44.723656m;
            DateTime expectedResult = new(2004, 7, 27, 13, 47, 0);

            //Act
            _calendar.SetWorkdayStartAndStop(startHours, startMinutes, endHours, endMinutes);
            _calendar.SetRecurringHoliday(reccuringHoliday.month, reccuringHoliday.day);
            _calendar.SetHoliday(holiday);

            DateTime result = _calendar.GetWorkdayIncrement(start, incrementInWorkdays);

            //Assert
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod()]
        [DataRow(8, 0, 16, 0)]
        public void GetWorkdayIncrementTest3(int startHours, int startMinutes, int endHours, int endMinutes)
        {
            //Arrange
            DateTime start = new(2004, 5, 24, 18, 3, 0);
            decimal incrementInWorkdays = -6.7470217m;
            DateTime expectedResult = new(2004, 5, 13, 10, 2, 0);

            //Act
            _calendar.SetWorkdayStartAndStop(startHours, startMinutes, endHours, endMinutes);
            _calendar.SetRecurringHoliday(reccuringHoliday.month, reccuringHoliday.day);
            _calendar.SetHoliday(holiday);

            DateTime result = _calendar.GetWorkdayIncrement(start, incrementInWorkdays);

            //Assert
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod()]
        [DataRow(8, 0, 16, 0)]
        public void GetWorkdayIncrementTest4(int startHours, int startMinutes, int endHours, int endMinutes)
        {
            //Arrange
            DateTime start = new(2004, 5, 24, 8, 3, 0);
            decimal incrementInWorkdays = 12.782709m;
            DateTime expectedResult = new(2004, 6, 10, 14, 18, 0);

            //Act
            _calendar.SetWorkdayStartAndStop(startHours, startMinutes, endHours, endMinutes);
            _calendar.SetRecurringHoliday(reccuringHoliday.month, reccuringHoliday.day);
            _calendar.SetHoliday(holiday);

            DateTime result = _calendar.GetWorkdayIncrement(start, incrementInWorkdays);

            //Assert
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod()]
        [DataRow(8, 0, 16, 0)]
        public void GetWorkdayIncrementTest5(int startHours, int startMinutes, int endHours, int endMinutes)
        {
            //Arrange
            DateTime start = new(2004, 5, 24, 7, 3, 0);
            decimal incrementInWorkdays = 8.276628m;
            DateTime expectedResult = new(2004, 6, 4, 10, 12, 0);

            //Act
            _calendar.SetWorkdayStartAndStop(startHours, startMinutes, endHours, endMinutes);
            _calendar.SetRecurringHoliday(reccuringHoliday.month, reccuringHoliday.day);
            _calendar.SetHoliday(holiday);

            DateTime result = _calendar.GetWorkdayIncrement(start, incrementInWorkdays);

            //Assert
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod()]
        [DataRow(8, 0, 16, 0)]
        public void GetWorkdayIncrementTest6(int startHours, int startMinutes, int endHours, int endMinutes)
        {
            //Arrange
            DateTime start = new(2004, 5, 24, 11, 0, 0);
            decimal incrementInWorkdays = -6.7470217m;
            DateTime expectedResult = new(2004, 5, 12, 13, 2, 0);

            //Act
            _calendar.SetWorkdayStartAndStop(startHours, startMinutes, endHours, endMinutes);
            _calendar.SetRecurringHoliday(reccuringHoliday.month, reccuringHoliday.day);
            _calendar.SetHoliday(holiday);

            DateTime result = _calendar.GetWorkdayIncrement(start, incrementInWorkdays);

            //Assert
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod()]
        [DataRow(10, 0, 15, 0)]
        public void GetWorkdayIncrementTest7(int startHours, int startMinutes, int endHours, int endMinutes)
        {
            //Arrange
            DateTime start = new(2004, 5, 24, 11, 0, 0);
            decimal incrementInWorkdays = -6.7470217m;
            DateTime expectedResult = new(2004, 5, 12, 12, 16, 0);

            //Act
            _calendar.SetWorkdayStartAndStop(startHours, startMinutes, endHours, endMinutes);
            _calendar.SetRecurringHoliday(reccuringHoliday.month, reccuringHoliday.day);
            _calendar.SetHoliday(holiday);

            DateTime result = _calendar.GetWorkdayIncrement(start, incrementInWorkdays);

            //Assert
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod()]
        [DataRow(10, 0, 15, 0)]
        public void GetWorkdayIncrementTest8(int startHours, int startMinutes, int endHours, int endMinutes)
        {
            //Arrange
            DateTime start = new(2004, 5, 24, 11, 0, 0);
            decimal incrementInWorkdays = 6.7470217m;
            DateTime expectedResult = new(2004, 6, 2, 13, 44, 0);

            //Act
            _calendar.SetWorkdayStartAndStop(startHours, startMinutes, endHours, endMinutes);
            _calendar.SetRecurringHoliday(reccuringHoliday.month, reccuringHoliday.day);
            _calendar.SetHoliday(holiday);

            DateTime result = _calendar.GetWorkdayIncrement(start, incrementInWorkdays);

            //Assert
            Assert.AreEqual(expectedResult, result);
        }
    }
}