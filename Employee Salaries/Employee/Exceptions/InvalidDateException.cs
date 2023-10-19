namespace Employee.Exceptions
{
    public class InvalidDateException: Exception
    {
        public InvalidDateException() : base("Contract end date cannot be before contract start date") { }
    }
}
