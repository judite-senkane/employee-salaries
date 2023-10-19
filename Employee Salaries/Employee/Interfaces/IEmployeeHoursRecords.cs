namespace Employee.Interfaces;

public interface IEmployeeHoursRecords
{
    void AddEmployee(EmployeeHours employeeHours);
    EmployeeHours GetEmployeeById(int id);
    List<EmployeeHours> GetActiveEmployeesList(DateTime? startTime, DateTime? endTime);
    EmployeeMonthlyReport[][] GetMonthlyReports(DateTime startTime, DateTime endTime);
}