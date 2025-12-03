using EyeCareHub.BLL.Interface;
using EyeCareHub.DAL.Entities.Basket;
using EyeCareHub.DAL.Entities.Identity;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EyeCareHub.BLL.Repositories
{
    public class NotificationRep : INotificationRepository
    {
        private readonly ConnectionMultiplexer _redis;
        private readonly IDatabase Database;
        private readonly string KeyPrefix;
        private readonly IConfiguration configuration;

        public NotificationRep(IConnectionMultiplexer redis, IConfiguration configuration)
        {
            _redis = (ConnectionMultiplexer)redis;

            Database = redis.GetDatabase();

            KeyPrefix = configuration.GetSection("NotificationSettings:NotKeyPrefix").Value;
            this.configuration = configuration;
        }

        public async Task<bool> AddNotification(Notification notification)
        {

            var key = GenerateRedisId(notification.Items.UserAppId);
            notification.Id = key;
            var Result = await Database.StringSetAsync(key, JsonSerializer.Serialize(notification), TimeSpan.FromDays(30));
            
            if (!Result) return false;

            return true;
        }

        public async Task<IReadOnlyList<Notification>> GetNotification(string notificId)
        {
            var server = _redis.GetServer(configuration.GetSection("NotificationSettings:Redis").Value, configuration.GetSection("NotificationSettings:Port").Value);

            // var BasketId = GenerateRedisId(notificId);
            var keys = server.Keys(pattern: $"{KeyPrefix}:{notificId}:*");


            //var basket = await Database.StringGetAsync(BaskxetId);

            List<Notification> items = new List<Notification>();

            foreach (var key in keys)
            {
                var json = await Database.StringGetAsync(key);
                if (json.HasValue)
                {
                    var item = JsonSerializer.Deserialize<Notification>(json);
                    items.Add(item);
                }
            }
            return items;
            //return basket.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CustomerBasket>(basket);
        }


        public string GenerateRedisId(string userId)
        {
            string notificationId = Guid.NewGuid().ToString();

            return $"{KeyPrefix}:{userId}:{notificationId}";

        }


    }
}
