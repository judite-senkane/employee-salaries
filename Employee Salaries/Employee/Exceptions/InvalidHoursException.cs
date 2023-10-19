namespace Employee.Exceptions
{
    public class InvalidHoursException: Exception
    {
        public InvalidHoursException(): base("Hours cannot be reported outside of contract") { }
    }
}
