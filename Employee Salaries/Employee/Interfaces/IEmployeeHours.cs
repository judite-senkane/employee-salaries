namespace Employee.Interfaces;

public interface IEmployeeHours
{
    Employee Employee { get; }
    DateTime ContractStart { get; }
    DateTime? ContractEnd { get; }

    void ReportHours(DateTime date, int hours, int minutes);
    void EndEmployeeContract(DateTime endTime);
    EmployeeMonthlyReport[] CalculateMonthlySalary(DateTime startTime, DateTime endTime);
}