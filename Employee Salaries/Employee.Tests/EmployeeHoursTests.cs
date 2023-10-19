using Employee.Exceptions;
using FluentAssertions;

namespace Employee.Tests
{
    [TestClass]
    public class EmployeeHoursTests
    {
        private EmployeeHours _employeeHours;
        private  readonly DateTime _defaultStartTime = new (2023, 01, 01);
        private readonly Employee _defaultEmployee = new () {Id = 567935, FullName = "Jānis Vītols", HourlySalary = 10.25m};
        private readonly List<HourReport> _employeeHoursList = new ();

        [TestMethod] 
        public void EmployeeHoursConstructor_WithValidEmployee_EmployeeCreated()
        {
            EmployeeHours createdEmployee =
                new EmployeeHours(_defaultEmployee, _defaultStartTime, null, _employeeHoursList);
            createdEmployee.Employee.Id.Should().Be(_defaultEmployee.Id);
            createdEmployee.Employee.FullName.Should().Be(_defaultEmployee.FullName);
            createdEmployee.Employee.HourlySalary.Should().Be(_defaultEmployee.HourlySalary);
            createdEmployee.ContractStart.Should().Be(_defaultStartTime);
        }

        [TestMethod]
        public void EmployeeHoursConstructor_WithEmptyName_ThrowsInvalidNameException()
        {
            var createdEmployee = new Employee() { Id = 357, HourlySalary = 10.25m };
            
            Action action = () => new EmployeeHours(createdEmployee, _defaultStartTime, null, _employeeHoursList);
            action.Should().Throw<InvalidNameException>();
        }

        [TestMethod]
        public void EmployeeHoursConstructor_WithNegativeHourlySalary_ThrowsInvalidHourlySalaryException()
        {
            var createdEmployee = new Employee() { Id = 357, FullName = "Jānis Vītols", HourlySalary = -6.9m };

            Action action = () => new EmployeeHours(createdEmployee, _defaultStartTime, null, _employeeHoursList);
            action.Should().Throw<InvalidHourlySalaryException>();
        }

        [TestMethod]
        public void EmployeeHoursConstructor_WithInvalidEndContractDate_ThrowsInvalidDateException()
        {
            var createdEmployee = new Employee() { Id = 357, FullName = "Jānis Vītols", HourlySalary = 10.25m };
            var endDate = new DateTime(2022, 12, 31);
            Action action = () => new EmployeeHours(createdEmployee, _defaultStartTime, endDate, _employeeHoursList);
            action.Should().Throw<InvalidDateException>();
        }

        [TestMethod]
        public void ReportHours_WithHoursAfterContractEnd_ThrowsInvalidHoursException()
        {
            var employee = new EmployeeHours(_defaultEmployee, _defaultStartTime, new DateTime(2023, 12, 31),
                _employeeHoursList);
            Action action = () => employee.ReportHours(new DateTime(2024, 12, 01, 9, 15, 00), 5, 30);
            _employeeHoursList.Should().HaveCount(0);
            action.Should().Throw<InvalidHoursException>();
        }


        [TestInitialize]
        public void Setup()
        {
            _employeeHours = new EmployeeHours(_defaultEmployee, _defaultStartTime, null, _employeeHoursList);
        }

        [TestMethod]
        public void ReportHours_WithValidInformation_HoursReported()
        {
            _employeeHours.ReportHours(new DateTime(2023,01,01,9,15,00), 5, 30);
            _employeeHoursList.Should().HaveCount(1);
        }

        [TestMethod]
        public void ReportHours_WithHoursBeforeContractStart_ThrowsInvalidHoursException()
        {
           Action action =() => _employeeHours.ReportHours(new DateTime(2022, 12, 01, 9, 15, 00), 5, 30);
            _employeeHoursList.Should().HaveCount(0);
            action.Should().Throw<InvalidHoursException>();
        }

        [TestMethod]
        public void ReportHours_WithNegativeHours_ThrowsInvalidHoursException()
        {
            Action action = () => _employeeHours.ReportHours(new DateTime(2023, 12, 01, 9, 15, 00), -5, 30);
            _employeeHoursList.Should().HaveCount(0);
            action.Should().Throw<NegativeValueException>();
        }

        [TestMethod]
        public void ReportHours_WithNegativeMinutes_ThrowsInvalidHoursException()
        {
            Action action = () => _employeeHours.ReportHours(new DateTime(2023, 12, 01, 9, 15, 00), 5, -30);
            _employeeHoursList.Should().HaveCount(0);
            action.Should().Throw<NegativeValueException>();
        }

        [TestMethod]
        public void ReportHours_WithOverlappingStartTime_ThrowsHoursAlreadyReportedException()
        {
            _employeeHoursList.Add(new HourReport(_employeeHours, new DateTime(2023,01,01, 9,00,00), 5, 30));
            Action action = () => _employeeHours.ReportHours(new DateTime(2023, 01, 01, 11, 00, 00), 7, 30);
            _employeeHoursList.Should().HaveCount(1);
            action.Should().Throw<HoursAlreadyReportedException>();
        }

        [TestMethod]
        public void ReportHours_WithOverlappingEndTime_ThrowsHoursAlreadyReportedException()
        {
            _employeeHoursList.Add(new HourReport(_employeeHours, new DateTime(2023, 01, 01, 11, 00, 00), 5, 30));
            Action action = () => _employeeHours.ReportHours(new DateTime(2023, 01, 01, 9, 00, 00), 3, 30);
            _employeeHoursList.Should().HaveCount(1);
            action.Should().Throw<HoursAlreadyReportedException>();
        }

        [TestMethod]
        public void EndEmployeeContract_WithValidDate_EmployeeEndContractDateSet()
        {
            var endDate = new DateTime(2023, 12, 31);
            _employeeHours.EndEmployeeContract(endDate);
            _employeeHours.ContractEnd.Should().Be(endDate);
        }

        [TestMethod]
        public void EndEmployeeContract_WithInvalidDate_ThrowsInvalidDateException()
        {
            var endDate = new DateTime(2022, 12, 31);
            Action action = () => _employeeHours.EndEmployeeContract(endDate);
            action.Should().Throw<InvalidDateException>();
        }

        [TestMethod]
        public void CalculateMonthlySalary_WithValidInformation_ReportReceived()
        {
            _employeeHoursList.Add(
                new HourReport(_employeeHours, new DateTime(2023, 01, 01, 9, 00, 00), 5, 30));
            _employeeHoursList.Add(new HourReport(_employeeHours, new DateTime(2023, 01, 02, 9, 00, 00), 4, 59));
            _employeeHoursList.Add(new HourReport(_employeeHours, new DateTime(2023, 01, 03, 9, 00, 00), 4, 17));
           var result = _employeeHours.CalculateMonthlySalary(new DateTime(2023, 01, 01), new DateTime(2023, 01, 31));

           result.Should().HaveCount(1);
            result[0].Salary.Should().Be(151.36m);
        }

        [TestMethod]
        public void CalculateMonthlySalary_WithInvalidInformation_ThrowsInvalidReportDatesException()
        {
            _employeeHoursList.Add(
                new HourReport(_employeeHours, new DateTime(2023, 01, 01, 9, 00, 00), 5, 30));
            _employeeHoursList.Add(new HourReport(_employeeHours, new DateTime(2023, 01, 02, 9, 00, 00), 4, 59));
            _employeeHoursList.Add(new HourReport(_employeeHours, new DateTime(2023, 01, 03, 9, 00, 00), 4, 17));
            Action action = () => _employeeHours.CalculateMonthlySalary(new DateTime(2023, 01, 01), new DateTime(2022, 01, 31));
            action.Should().Throw<InvalidReportDatesException>();
        }

        [TestMethod]
        public void CalculateMonthlySalary_WithValidInformationOverSeveralMonths_CorrectReportReceived()
        {
            _employeeHoursList.Add(
                new HourReport(_employeeHours, new DateTime(2023, 01, 01, 9, 00, 00), 5, 30));
            _employeeHoursList.Add(new HourReport(_employeeHours, new DateTime(2023, 02, 02, 9, 00, 00), 4, 59));
            _employeeHoursList.Add(new HourReport(_employeeHours, new DateTime(2023, 03, 03, 9, 00, 00), 4, 17));
            var result =_employeeHours.CalculateMonthlySalary(new DateTime(2023, 01, 01), new DateTime(2023, 03, 31));

            result.Should().HaveCount(3);

            result[0].Salary.Should().BeApproximately(56.375m, 0.005m);
            result[1].Salary.Should().BeApproximately(51.079m, 0.005m);
            result[2].Salary.Should().BeApproximately(43.904m, 0.005m);
        }
    }
}
