namespace Employee.Exceptions
{
    public class NegativeValueException: Exception
    {
        public NegativeValueException() : base("Reported hours or minutes cannot be negative") { }
    }
}
