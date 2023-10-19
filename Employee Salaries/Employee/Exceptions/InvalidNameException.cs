namespace Employee.Exceptions
{
    public class InvalidNameException : Exception
    {
        public InvalidNameException() : base("Employee name cannot be null or empty") { }
    }
}
