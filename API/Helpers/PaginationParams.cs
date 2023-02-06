namespace API.Helpers
{
    public class PaginationParams
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? CurrentUsername { get; set; }
        public string? Gender { get; set; }
    }
}
