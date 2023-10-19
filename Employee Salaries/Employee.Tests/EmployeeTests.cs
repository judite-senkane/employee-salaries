using FluentAssertions;

namespace Employee.Tests
{
    [TestClass]
    public class EmployeeTests
    {
        private const int DEFAULT_ID = 1;
        private const string DEFAULT_FULL_NAME = "Jānis Vītoliņš";
        private const decimal DEFAULT_HOURLY_SAlARY = 10.25m;

        [TestMethod]
        public void EmployeeIdSet_EmployeeIdReturned()
        {
            var employee = new Employee();
            employee.Id = DEFAULT_ID;
            employee.Id.Should().Be(DEFAULT_ID);
        }

        [TestMethod]
        public void EmployeeFullNameSet_EmployeeNameReturned()
        {
            var employee = new Employee();
            employee.FullName = DEFAULT_FULL_NAME;
            employee.FullName.Should().Be(DEFAULT_FULL_NAME);
        }

        [TestMethod]
        public void EmployeeHourlySalarySet_EmployeeSalaryReturned()
        {
            var employee = new Employee();
            employee.HourlySalary = DEFAULT_HOURLY_SAlARY;
            employee.HourlySalary.Should().Be(DEFAULT_HOURLY_SAlARY);
        }
    }
}
