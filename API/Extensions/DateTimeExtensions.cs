namespace API.Extensions
{
    public static class DateTimeExtensions
    {
        public static int GetAge(this DateOnly dob)
        {
            return  DateTime.Today.Year - dob.Year;
        }
    }
}
