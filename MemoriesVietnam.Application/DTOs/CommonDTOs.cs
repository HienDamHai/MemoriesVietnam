namespace MemoriesVietnam.Models.DTOs
{
    public class ApiResponse<T>
    {
        public T? Data { get; set; }
        public string Message { get; set; } = string.Empty;
        public bool Success { get; set; } = true;
    }

    public class PaginatedResponse<T>
    {
        public List<T> Data { get; set; } = new();
        public int Total { get; set; }
        public int Page { get; set; }
        public int Limit { get; set; }
        public int TotalPages { get; set; }
    }

    public class PaginationQuery
    {
        public int Page { get; set; } = 1;
        public int Limit { get; set; } = 10;
    }

    public class ArticleQuery : PaginationQuery
    {
        public string? EraId { get; set; }
        public string? Search { get; set; }
    }

    public class ProductQuery : PaginationQuery
    {
        public string? CategoryId { get; set; }
        public string? Search { get; set; }
    }
}
