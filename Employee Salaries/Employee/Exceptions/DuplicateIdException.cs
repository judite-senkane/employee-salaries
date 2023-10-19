namespace Employee.Exceptions
{
    public class DuplicateIdException : Exception
    {
        public DuplicateIdException() : base("Employee with this ID already exists") { }
    }
}
