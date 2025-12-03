using EyeCareHub.DAL.Entities.Basket;
using EyeCareHub.DAL.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeCareHub.BLL.Interface
{
    public interface INotificationRepository
    {
        Task<IReadOnlyList<Notification>> GetNotification(string notificId);
        Task<bool> AddNotification(Notification notification);

    }
}
