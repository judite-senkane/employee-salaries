using Employee.Interfaces;
using FluentAssertions;
using Moq.AutoMock;

namespace Employee.Tests
{
    [TestClass]
    public class HourReportTests
    {
        private HourReport _hourReport;
        private Employee _defaultEmployee;
        private const int DEFAULT_ID = 1;
        private const string DEFAULT_FULL_NAME = "Jānis Vītoliņš";
        private const decimal DEFAULT_HOURLY_SALARY = 10.25m;
        private readonly DateTime _defaultDayTime = new DateTime(2023, 01, 01, 13,30,00);
        private const int DEFAULT_HOURS = 2;
        private const int DEFAULT_MINUTES = 30;
        private AutoMocker _mocker;

        [TestInitialize]
        public void SetUp()
        {
            _mocker = new AutoMocker();
            _defaultEmployee = new Employee
            {
                Id = DEFAULT_ID, 
                FullName = DEFAULT_FULL_NAME, 
                HourlySalary = DEFAULT_HOURLY_SALARY
            };
            var defaultEmployeeHoursMock = _mocker.GetMock<IEmployeeHours>();
            defaultEmployeeHoursMock.Setup(h => h.Employee).Returns(_defaultEmployee);

            _hourReport = new HourReport(defaultEmployeeHoursMock.Object, _defaultDayTime, DEFAULT_HOURS, DEFAULT_MINUTES);
        }
        [TestMethod]
        public void HourReportConstructor_WithValidInformation_HourReportCreated()
        {
            _hourReport.Date.Should().Be(_defaultDayTime);
            _hourReport.Hours.Should().Be(DEFAULT_HOURS);
            _hourReport.Minutes.Should().Be(DEFAULT_MINUTES);
        }

        [TestMethod]
        public void DaySalaryGet_CorrectSalaryReturned()
        {
            var salary = _hourReport.DaySalary;
            salary.Should().Be(25.625m);
        }
    }
}
