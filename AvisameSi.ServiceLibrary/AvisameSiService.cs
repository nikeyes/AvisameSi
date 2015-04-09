using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvisameSi.ServiceLibrary
{
    public class AvisameSiService
    {
        ConnectionMultiplexer _rediConn;
        public AvisameSiService(ConnectionMultiplexer redisConn)
        {
            _rediConn = redisConn;
        }

        public void Register(string username, string password)
        {
            if (string.IsNullOrEmpty(username))
                throw new ArgumentNullException("username");
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");

            IDatabase redis = _rediConn.GetDatabase();
            
                var exists = !string.IsNullOrEmpty(redis.HashGet("users", username));
                if (exists)
                {
                    throw new Exception("The user already exists");
                }

                var userId = redis.StringIncrement("next_user_id");
                redis.HashSet("users", username, userId.ToString());
            HashEntry[] hashEntryArray = new HashEntry[] {
                new HashEntry("username", username),
                new HashEntry("password", password)
            };
                redis.HashSet("user:" + userId, hashEntryArray);

                redis.SortedSetAdd("users_by_time", username, DateTime.Now.Ticks );
            
        }

        public bool Login(string username, string password)
        {
            return false;
            /*if (string.IsNullOrEmpty(username))
                throw new ArgumentNullException("username");
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");

            using (var redis = RedisManager.Current.GetClient())
            {
                var userId = redis.Hashes.HGetString("users", username);
                if (!string.IsNullOrEmpty(userId))
                {
                    var storedPassword = redis.Hashes.HGetString("user:" + userId, "password");
                    return password == storedPassword;
                }
            }

            return false;*/
        }
    }
}
