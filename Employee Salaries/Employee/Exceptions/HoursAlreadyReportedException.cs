namespace Employee.Exceptions
{
    public class HoursAlreadyReportedException: Exception
    {
        public HoursAlreadyReportedException() : base(
            "There are hours already reported during your specified time period") {}
    }
}
