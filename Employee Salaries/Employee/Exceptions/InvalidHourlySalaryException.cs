namespace Employee.Exceptions
{
    public class InvalidHourlySalaryException: Exception
    {
        public InvalidHourlySalaryException() : base("Hourly salary cannot be negative"){ }
    }
}
