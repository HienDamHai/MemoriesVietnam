namespace MemoriesVietnam.Models.DTOs
{
    public class EraDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int YearStart { get; set; }
        public int YearEnd { get; set; }
        public string? Description { get; set; }
    }

    public class CreateEraDto
    {
        public string Name { get; set; } = string.Empty;
        public int YearStart { get; set; }
        public int YearEnd { get; set; }
        public string? Description { get; set; }
    }

    public class UpdateEraDto
    {
        public string? Name { get; set; }
        public int? YearStart { get; set; }
        public int? YearEnd { get; set; }
        public string? Description { get; set; }
    }
}
