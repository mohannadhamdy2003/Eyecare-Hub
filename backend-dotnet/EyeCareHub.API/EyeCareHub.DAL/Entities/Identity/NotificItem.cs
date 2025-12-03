using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeCareHub.DAL.Entities.Identity
{
    public class NotificItem
    {
        public NotificItem(string Message, string UserAppId)
        {
            this.Message = Message;
            this.UserAppId = UserAppId;
        }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsRead { get; set; } = false;
        public string UserAppId { get; set; }

    }
}
