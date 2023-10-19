using FluentAssertions;

namespace Employee.Tests
{
    [TestClass]
    public class EmployeeMonthlyReportTests
    {
        private EmployeeMonthlyReport _employeeMonthlyReport;
        private const int DEFAULT_EMPLOYEE_ID = 1;
        private const int DEFAULT_MONTH = 1;
        private const int DEFAULT_YEAR = 2013;
        private const decimal DEFAULT_SALARY = 2000;

        [TestInitialize]
        public void Setup()
        {
            _employeeMonthlyReport = new EmployeeMonthlyReport();
        }

        [TestMethod]
        public void EmployeeIdSet_EmployeeIdReturned()
        {
            _employeeMonthlyReport.EmployeeId = DEFAULT_EMPLOYEE_ID;
            _employeeMonthlyReport.EmployeeId.Should().Be(DEFAULT_EMPLOYEE_ID);
        }

        [TestMethod]
        public void MonthSet_MonthReturned()
        {
            _employeeMonthlyReport.Month = DEFAULT_MONTH;
            _employeeMonthlyReport.Month.Should().Be(DEFAULT_MONTH);
        }

        [TestMethod]
        public void YearSet_YearReturned()
        {
            _employeeMonthlyReport.Year = DEFAULT_YEAR;
            _employeeMonthlyReport.Year.Should().Be(DEFAULT_YEAR);
        }

        [TestMethod]
        public void SalarySet_SalaryReturned()
        {
            _employeeMonthlyReport.Salary = DEFAULT_SALARY;
            _employeeMonthlyReport.Salary.Should().Be(DEFAULT_SALARY);
        }
    }
}
