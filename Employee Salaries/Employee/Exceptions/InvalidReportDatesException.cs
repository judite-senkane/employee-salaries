namespace Employee.Exceptions
{
    public class InvalidReportDatesException: Exception
    {
        public InvalidReportDatesException() : base("Report end date cannot be before report start date") { }
    }
}
