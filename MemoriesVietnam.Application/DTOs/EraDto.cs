using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoriesVietnam.Application.DTOs
{
    public class EraDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int YearStart { get; set; }
        public int YearEnd { get; set; }
        public string Description { get; set; }
    }

    public class CreateEraDto
    {
        public string Name { get; set; }
        public int YearStart { get; set; }
        public int YearEnd { get; set; }
        public string Description { get; set; }
    }

    public class UpdateEraDto
    {
        public string Name { get; set; }
        public int YearStart { get; set; }
        public int YearEnd { get; set; }
        public string Description { get; set; }
    }
}
