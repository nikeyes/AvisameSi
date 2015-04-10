using AvisameSi.ServiceLibrary.RespositoryContracts;
using System;
using StackExchange.Redis;


namespace AvisameSi.Redis.Infrastructure
{
    public class AccountRepository: IAccountRepository
    {
        private const String REDIS_CHAR_CREATE_KEYS = ":";
        private const String REDIS_SEQUENCE_USERS_ID = "sequence_user_id";
        private const String REDIS_USERS_LIST = "users";
        private const String REDIS_USER_LIST = "user";
        private const String REDIS_USER_DATA_USERNAME = "username";
        private const String REDIS_USER_DATA_PASSWORD = "password";
        private const String REDIS_USERS_BY_CREATIONDATE = "users_by_creationdate";
        

        private ConnectionMultiplexer _redisConn;

        public AccountRepository(ConnectionMultiplexer redisConn)
        {
            _redisConn = redisConn;
        }

        public bool UserExist(string email)
        {
            IDatabase redis = _redisConn.GetDatabase();
            bool exists = !String.IsNullOrEmpty(redis.HashGet(REDIS_USERS_LIST, email));
            return exists;
        }

        public void RegisterUser(string email, string password)
        {
            IDatabase redis = _redisConn.GetDatabase();
            long userId = redis.StringIncrement(REDIS_SEQUENCE_USERS_ID);
            redis.HashSet(REDIS_USERS_LIST, email, userId.ToString());
            HashEntry[] hashEntryArray = new HashEntry[] {
                new HashEntry(REDIS_USER_DATA_USERNAME, email),
                new HashEntry(REDIS_USER_DATA_PASSWORD, password)
            };

            redis.HashSet(REDIS_USER_LIST + REDIS_CHAR_CREATE_KEYS + userId, hashEntryArray);

            redis.SortedSetAdd(REDIS_USERS_BY_CREATIONDATE, email, DateTime.Now.Ticks);
        }


        public bool Login(string email, string password)
        {
            bool result = false;
            IDatabase redis = _redisConn.GetDatabase();
            String userId = redis.HashGet(REDIS_USERS_LIST, email);
            if (!string.IsNullOrEmpty(userId))
            {
                String storedPassword = redis.HashGet(REDIS_USER_LIST + REDIS_CHAR_CREATE_KEYS + userId, "password");
                result = password == storedPassword;
            }
            return result;
        }
    }
}
