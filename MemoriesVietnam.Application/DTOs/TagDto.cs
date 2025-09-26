using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoriesVietnam.Application.DTOs
{
    public class TagDto
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public string Slug { get; set; } = "";
        public DateTime CreatedAt { get; set; }
    }

    public class CreateTagDto
    {
        public string Name { get; set; } = "";
        public string Slug { get; set; } = "";
    }

    public class UpdateTagDto : CreateTagDto
    {
        public string Id { get; set; } = "";
    }

}
