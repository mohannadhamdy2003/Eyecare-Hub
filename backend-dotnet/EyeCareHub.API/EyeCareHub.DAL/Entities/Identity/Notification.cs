using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeCareHub.DAL.Entities.Identity
{
    public class Notification
    {
        public string Id { get; set; }
        public NotificItem Items { get; set; }
    }
}
