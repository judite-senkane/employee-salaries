using Employee.Exceptions;
using Employee.Interfaces;

namespace Employee
{
    public class EmployeeHoursRecords : IEmployeeHoursRecords
    {
        private readonly List<EmployeeHours> _employeeHoursRecords;

        public EmployeeHoursRecords(List<EmployeeHours> employeeHoursRecords)
        {
            _employeeHoursRecords = employeeHoursRecords;
        }

        public void AddEmployee(EmployeeHours employeeHours) => _employeeHoursRecords.Add(employeeHours);

        public EmployeeHours GetEmployeeById(int id)
        {
            EmployeeHours employeeHours = _employeeHoursRecords.FirstOrDefault(e => e.Employee.Id == id);

            return employeeHours;
        }

        public List<EmployeeHours> GetActiveEmployeesList(DateTime? startTime, DateTime? endTime)
        {
            if (startTime == null) startTime = DateTime.Now;
            if (endTime == null) endTime = DateTime.Now;
            if (endTime < startTime) throw new InvalidReportDatesException();

            return _employeeHoursRecords.Where(r => r.ContractStart <= startTime
                && (r.ContractEnd >= endTime || r.ContractEnd == null)).ToList();
        }

        public EmployeeMonthlyReport[][] GetMonthlyReports(DateTime startTime, DateTime endTime)
        {
            if (endTime < startTime) throw new InvalidReportDatesException();

            List<EmployeeMonthlyReport[]> result = new ();

            var activeEmployees = GetActiveEmployeesList(startTime, endTime);

            foreach (EmployeeHours employee in activeEmployees)
            {
                result.Add(employee.CalculateMonthlySalary(startTime, endTime));
            }

            return result.ToArray();
        }
    }
}
