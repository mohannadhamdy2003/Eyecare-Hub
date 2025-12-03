using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeCareHub.DAL.Entities.Identity
{
    public class DoctorRating:BaseEntity
    {
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }   
        public string UserId { get; set; }
        public AppUser User { get; set; }
        public decimal Value { get; set; }  
        public string Comments { get; set; }
        public DateTime RatedAt { get; set; } = DateTime.Now;
    }
}
