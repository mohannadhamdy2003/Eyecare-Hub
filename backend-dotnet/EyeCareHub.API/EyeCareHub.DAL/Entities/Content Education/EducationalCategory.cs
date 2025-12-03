using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeCareHub.DAL.Entities.Content_Education
{
    public class EducationalCategory : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Article> Contents { get; set; } = new List<Article>();
    }
}
