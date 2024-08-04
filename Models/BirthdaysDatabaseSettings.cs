namespace BirthdayCalendarMVC.Models
{
    public class BirthdaysDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string PersonsCollectionName { get; set; } = null!;
    }
}
