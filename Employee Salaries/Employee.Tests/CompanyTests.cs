using Employee.Exceptions;
using Employee.Interfaces;
using FluentAssertions;
using Moq;
using Moq.AutoMock;

namespace Employee.Tests
{
    [TestClass]
    public class CompanyTests
    {
        private const string DEFAULT_COMPANY_NAME = "Default Company";
        private ICompany _company;

        private const int DEFAULT_ID = 194578;
        private const string DEFAULT_FULL_NAME = "Jānis Vītols";
        private const decimal DEFAULT_HOURLY_SALARY = 10.25m;
        private readonly DateTime _defaultContractStart = new (2023, 01, 01);
        private AutoMocker _mocker;

        [TestInitialize]
        public void Setup()
        {
            _mocker = new AutoMocker();
            _mocker.Use(DEFAULT_COMPANY_NAME);
            _company = _mocker.CreateInstance<Company>();
        }
        
        [TestMethod]
        public void AddEmployee_WithValidInformation_newEmployeeAdded()
        {
            Employee employee = GetEmployee();
            DateTime startTime = _defaultContractStart;

            _company.AddEmployee(employee, startTime);

            _mocker.GetMock<IEmployeeHoursRecords>().Verify(r => r.AddEmployee(It.IsAny<EmployeeHours>()), Times.Once);
        }

        [TestMethod]
        public void AddEmployee_WithDuplicateEmployeeId_ThrowsDuplicateIdException()
        {
            Employee employee = GetEmployee();

            DateTime startTime = _defaultContractStart;
            var employeeHours = new EmployeeHours(employee, startTime, null,
                new List<HourReport>());
            _mocker.GetMock<IEmployeeHoursRecords>().Setup(s => s.GetEmployeeById(employee.Id))
                .Returns(employeeHours);

            Action action = () => _company.AddEmployee(employee, startTime);
            action.Should().Throw<DuplicateIdException>();
        }

        [TestMethod]
        public void RemoveEmployee_WithExistingEmployee_EmployeeRemoved()
        {
            Employee employee = GetEmployee();
            DateTime startTime = _defaultContractStart;
            DateTime contractEnd = new DateTime(2023, 08, 31);

            EmployeeHours employeeRecord = new EmployeeHours(employee, startTime, null, new List<HourReport>());

            _mocker.GetMock<IEmployeeHoursRecords>().Setup(e => e.GetEmployeeById(DEFAULT_ID)).Returns(employeeRecord);

           _company.RemoveEmployee(employee.Id, contractEnd);

           employeeRecord.ContractEnd.Should().Be(contractEnd);
        }

        [TestMethod]
        public void RemoveEmployee_WithNonExistingEmployee_ThrowsEmployeeNotFoundException()
        {
            DateTime contractEnd = new DateTime(2023, 08, 31);

            Action action = () => _company.RemoveEmployee(DEFAULT_ID, contractEnd);
            action.Should().Throw<EmployeeNotFoundException>();

        }

        [TestMethod]
        public void ReportHours_WithValidInformation_HoursReported()
        {
            Employee employee = GetEmployee();

            DateTime startTime = _defaultContractStart;
            var hoursList = new List<HourReport>();
            var employeeHours = new EmployeeHours(employee, startTime,
                null, hoursList);

            _mocker.GetMock<IEmployeeHoursRecords>().Setup(s =>
                s.GetEmployeeById(employee.Id)).Returns(employeeHours);

            var reportDate = new DateTime(2023, 01, 15, 9, 00, 00);

            _company.ReportHours(employee.Id, reportDate, 2, 30);
            hoursList.Should().HaveCount(1);
        }

        private Employee GetEmployee()
        {
            return new Employee() {Id = DEFAULT_ID, FullName = DEFAULT_FULL_NAME, HourlySalary = DEFAULT_HOURLY_SALARY};
        }
    }
}