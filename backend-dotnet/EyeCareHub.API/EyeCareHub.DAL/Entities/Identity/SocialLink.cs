using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeCareHub.DAL.Entities.Identity
{
    public class SocialLink :BaseEntity
    {

        public SocialPlatform Platform { get; set; }

        public string Url { get; set; }

        public string UserId { get; set; }
        public AppUser User { get; set; }
    }
}
