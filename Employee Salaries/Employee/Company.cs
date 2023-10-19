using Employee.Interfaces;
using Employee.Exceptions;

namespace Employee
{
    public class Company: ICompany
    {
        private readonly IEmployeeHoursRecords _employeeRecords;

        public Company(string name, IEmployeeHoursRecords employeeRecords)
        {
            Name = name;
            _employeeRecords = employeeRecords;
        }

        public string Name { get; }
        public Employee[] Employees => _employeeRecords.GetActiveEmployeesList(null, null)
            .Select(r => r.Employee).ToArray();

        public void AddEmployee(Employee employee, DateTime contractStartDate)
        {
            var existingRecord = _employeeRecords.GetEmployeeById(employee.Id);
            if (existingRecord != null && (existingRecord.ContractEnd > DateTime.Now 
                                       || existingRecord.ContractEnd == null)) 
                throw new DuplicateIdException();

            _employeeRecords.AddEmployee(new EmployeeHours(employee, contractStartDate,
                null, new List<HourReport>()));
        }

        public void RemoveEmployee(int employeeId, DateTime contractEndDate)
        {
            EmployeeHours employeeRecord = _employeeRecords.GetEmployeeById(employeeId);

            if (employeeRecord == null) throw new EmployeeNotFoundException();

            employeeRecord.EndEmployeeContract(contractEndDate);
        }

        public void ReportHours(int employeeId, DateTime dateAndTime, int hours, int minutes)
        {
            var employeeRecord = _employeeRecords.GetEmployeeById(employeeId);
            employeeRecord.ReportHours(dateAndTime, hours, minutes);
        }

        public EmployeeMonthlyReport[] GetMonthlyReport(DateTime periodStartDate, DateTime periodEndDate)
        {
            return _employeeRecords.GetMonthlyReports(periodStartDate, periodEndDate).SelectMany(v => v.Select(r => r)).ToArray();
        }
    }
}
