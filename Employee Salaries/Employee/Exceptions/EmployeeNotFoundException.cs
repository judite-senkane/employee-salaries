namespace Employee.Exceptions
{
    public class EmployeeNotFoundException: Exception
    {
        public EmployeeNotFoundException() : base("Employee with this id is not in the records") { }
    }
}
