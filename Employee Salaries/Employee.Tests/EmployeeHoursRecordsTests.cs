using Employee.Exceptions;
using Employee.Interfaces;
using FluentAssertions;
using Moq.AutoMock;

namespace Employee.Tests
{
    [TestClass]
    public class EmployeeHoursRecordsTests
    {
        private EmployeeHoursRecords _records;
        private List<EmployeeHours> _employeeList;
        private const int DEFAULT_ID = 1;
        private const string DEFAULT_EMPLOYEE_NAME = "Jānis Vītols";
        private const decimal DEFAULT_HOURLY_SALARY = 10.25m;
        private readonly DateTime _defaultContractStart = new DateTime(2023, 01, 01);
        private AutoMocker _mocker;

        [TestInitialize]
        public void Setup()
        {
            _mocker = new AutoMocker();
            _employeeList = new List<EmployeeHours>();
            _records = new EmployeeHoursRecords(_employeeList);
        }

        [TestMethod]
        public void AddEmployee_WithValidEmployee_EmployeeAdded()
        {
            var employeeHours = GetEmployee();
            _records.AddEmployee(employeeHours);
            _employeeList.Should().HaveCount(1);
        }

        [TestMethod]
        public void GetEmployeeById_WithValidEmployee_EmployeeHoursRecordReturned()
        {
            var employeeHours = GetEmployee();
            _employeeList.Add(employeeHours);
            _records.GetEmployeeById(DEFAULT_ID).Should().NotBeNull();
        }

        [TestMethod]
        public void GetEmployeeById_WithNonExistingId_ResultShouldBeNull()
        {
            _records.GetEmployeeById(DEFAULT_ID).Should().BeNull();
        }


        [TestMethod]
        public void GetActiveEmployeesList_WithValidEmployee_EmployeeHListReturned()
        {
            var employeeHours = GetEmployee();
            _employeeList.Add(employeeHours);
            _records.GetActiveEmployeesList(null, null).Should().HaveCount(1);
        }

        [TestMethod]
        public void GetActiveEmployeesList_WithEmployeeWithContractEnded_ListShouldBeEmpty()
        {
            var employeeHours = new EmployeeHours(new Employee
            {
                Id = DEFAULT_ID, 
                FullName = DEFAULT_EMPLOYEE_NAME, 
                HourlySalary = DEFAULT_HOURLY_SALARY
            }, 
                _defaultContractStart, DateTime.Now.AddDays(-3), new List<HourReport>());

            _employeeList.Add(employeeHours);
            _records.GetActiveEmployeesList(null, null).Should().HaveCount(0);
        }

        [TestMethod]
        public void GetActiveEmployeesList_WithInvalidEndDate_ThrowsInvalidReportDatesException()
        {
            var employeeHours = GetEmployee();
            _employeeList.Add(employeeHours);
            var startTime = new DateTime(2023, 01, 01);
            var endTime = new DateTime(2022, 12, 31);
            Action action = () => _records.GetActiveEmployeesList(startTime, endTime);
            action.Should().Throw<InvalidReportDatesException>();
        }

        [TestMethod]
        public void GetMonthlyReports_WithOneEmployee_ReportReceived()
        {
            //Arrange
            var employeeHours = GetEmployee();
            _employeeList.Add(employeeHours);

            var employeeHourReport = new EmployeeMonthlyReport[]
                { new EmployeeMonthlyReport
                {
                    EmployeeId = DEFAULT_ID, 
                    Year = 2023, 
                    Month = 1, 
                    Salary = 156m
                } };
            var startTime = new DateTime(2023, 01, 01);
            var endTime = new DateTime(2023, 01, 31);
            _mocker.GetMock<IEmployeeHours>().Setup(e => e.CalculateMonthlySalary(startTime, endTime)).Returns(employeeHourReport);

            //Act
            var result = _records.GetMonthlyReports(startTime, endTime);

            //Assert
            result.Should().HaveCount(1);
            result[0].Should().HaveCount(1);
        }

        [TestMethod]
        public void GetMonthlyReports_WithMultipleEmployees_ReportReceived()
        {
            //Arrange
            var employeeHours = GetEmployee();
            _employeeList.Add(employeeHours);
            _employeeList.Add(new EmployeeHours(new Employee
            {
                Id = 2, 
                FullName = "Kristaps Liepiņš", 
                HourlySalary = 9.45m
            }, new DateTime(2023,01,01), 
                null, new List<HourReport>()));

            var employeeHourReport1 = new EmployeeMonthlyReport[]
            { new EmployeeMonthlyReport
            {
                EmployeeId = DEFAULT_ID,
                Year = 2023,
                Month = 1,
                Salary = 156m
            } };
            var employeeHourReport2 = new EmployeeMonthlyReport[]
            { new EmployeeMonthlyReport
            {
                EmployeeId = 2,
                Year = 2023,
                Month = 1,
                Salary = 234m
            } };
            var startTime = new DateTime(2023, 01, 01);
            var endTime = new DateTime(2023, 01, 31);

            _mocker.GetMock<IEmployeeHours>().Setup(e => e.CalculateMonthlySalary(startTime, endTime)).Returns(employeeHourReport1);
            _mocker.GetMock<IEmployeeHours>().Setup(e => e.CalculateMonthlySalary(startTime, endTime))
                .Returns(employeeHourReport2);

            //Act
            var result = _records.GetMonthlyReports(startTime, endTime);

            //Assert
            result.Should().HaveCount(2);
            result[0].Should().HaveCount(1);
            result[1].Should().HaveCount(1);
        }

        [TestMethod]
        public void GetMonthlyReports_WithInvalidEndDate_ThrowsInvalidReportDatesException()
        {
            var employeeHours = GetEmployee();
            _employeeList.Add(employeeHours);
            var startTime = new DateTime(2023, 01, 01);
            var endTime = new DateTime(2022, 12, 31);

            Action action = () => _records.GetMonthlyReports(startTime, endTime);
            action.Should().Throw<InvalidReportDatesException>();
        }


        private EmployeeHours GetEmployee()
        {
            return new EmployeeHours(
                new Employee
                    { Id = DEFAULT_ID, 
                        FullName = DEFAULT_EMPLOYEE_NAME, 
                        HourlySalary = DEFAULT_HOURLY_SALARY
                    },
                _defaultContractStart, null, new List<HourReport>());
        }
    }
}
