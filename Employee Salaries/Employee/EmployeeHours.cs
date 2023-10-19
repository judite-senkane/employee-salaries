using Employee.Exceptions;
using Employee.Interfaces;

namespace Employee
{
    public class EmployeeHours : IEmployeeHours
    {
        private readonly List<HourReport> _employeeHoursReport;

        public EmployeeHours (Employee employee, DateTime contractStart, DateTime? contractEnd, List<HourReport> hourReportList)
        {
            if (string.IsNullOrEmpty(employee.FullName)) throw new InvalidNameException();
            if (employee.HourlySalary < 0) throw new InvalidHourlySalaryException();
            if (contractEnd < contractStart) throw new InvalidDateException();

            Employee = employee;
            ContractStart = contractStart;
            if (contractEnd != null) ContractEnd = contractEnd;
            _employeeHoursReport = hourReportList;
        }

        public Employee Employee { get; }
        public DateTime ContractStart { get; }
        public DateTime? ContractEnd { get; private set; }

        public void ReportHours(DateTime date, int hours, int minutes)
        {
            if (date < ContractStart || date > ContractEnd) throw new InvalidHoursException();
            if (hours < 0) throw new NegativeValueException();
            if (minutes < 0) throw new NegativeValueException();
            if (CheckIfReported(date, hours, minutes)) throw new HoursAlreadyReportedException();

            _employeeHoursReport.Add(new HourReport (this,date, hours, minutes));
        }

        public void EndEmployeeContract(DateTime endTime)
        {
            if (endTime < ContractStart) throw new InvalidDateException();
            ContractEnd = endTime;
        }

        public EmployeeMonthlyReport[] CalculateMonthlySalary(DateTime startTime, DateTime endTime)
        {
            if (endTime < startTime) throw new InvalidReportDatesException();

            var result = new List<EmployeeMonthlyReport>();
            var period = startTime;

            while (period.Month <= endTime.Month)
            {
                var monthlySalary = _employeeHoursReport.Where(h =>
                    h.Date.Month == period.Month).Select(r => r.DaySalary).Sum();
                monthlySalary = Math.Round(monthlySalary, 2);

                result.Add(new EmployeeMonthlyReport()
                {
                    EmployeeId = Employee.Id,
                    Year = period.Year,
                    Month = period.Month,
                    Salary = monthlySalary
                });
                period = period.AddMonths(1);
            }

            return result.ToArray();
        }

        private bool CheckIfReported(DateTime date, int hours, int minutes)
        {
            DateTime endTime = date.AddHours(hours).AddMinutes(minutes);

            return _employeeHoursReport.Any(h => (
                h.Date.Date == date.Date && (date < h.Date.AddHours(h.Hours).AddMinutes(h.Minutes) 
                                             || endTime > h.Date)));
        }
    }
}
