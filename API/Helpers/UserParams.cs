namespace API.Helpers
{
    public class UserParams
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? CurrentUsername { get; set; }
        public string? Gender { get; set; }
        public int MinAge { get; set; }
        public int MaxAge { get; set; }

        public string orderBy { get; set; } = "lastActive";
    }
}
