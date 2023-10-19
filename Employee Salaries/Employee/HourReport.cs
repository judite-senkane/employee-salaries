using Employee.Interfaces;

namespace Employee
{
    public class HourReport
    {
        private IEmployeeHours _employeeHours;

        public HourReport(
            IEmployeeHours employee, DateTime 
                date, int hours, int minutes)
        {
            _employeeHours = employee;
            Date = date;
            Hours = hours;
            Minutes = minutes;
        }

        public DateTime Date { get; }
        public int Hours { get; }
        public int Minutes { get; }

        public decimal DaySalary
        {
            get
            {
                var hourSalary = _employeeHours.Employee.HourlySalary;
                var hours = (decimal)Hours + ((decimal)Minutes / 60);
                return hourSalary * hours;
            }
        }
    }
}
