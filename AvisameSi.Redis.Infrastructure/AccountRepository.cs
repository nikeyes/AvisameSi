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
        private const String REDIS_USER_DATA = "user" + REDIS_CHAR_CREATE_KEYS;
        private const String REDIS_USER_DATA_EMAIL = "email";
        private const String REDIS_USER_DATA_PASSWORD = "password";
        private const String REDIS_USER_DATA_LOGGED_AUTH = "logged_auth_token";
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

        public string RegisterUser(string email, string password)
        {
            IDatabase redis = _redisConn.GetDatabase();
            long userId = redis.StringIncrement(REDIS_SEQUENCE_USERS_ID);
            redis.HashSet(REDIS_USERS_LIST, email, userId.ToString());
            HashEntry[] hashEntryArray = new HashEntry[] {
                new HashEntry(REDIS_USER_DATA_EMAIL, email),
                new HashEntry(REDIS_USER_DATA_PASSWORD, password)
            };

            redis.HashSet(REDIS_USER_DATA + userId, hashEntryArray);

            redis.SortedSetAdd(REDIS_USERS_BY_CREATIONDATE, email, DateTime.Now.Ticks);

            string token = CreateLoggedUserToken(userId.ToString());
            return token;
        }


        public string Login(string email, string password)
        {
            string token = String.Empty;
            IDatabase redis = _redisConn.GetDatabase();
            string userId = redis.HashGet(REDIS_USERS_LIST, email);
            if (!string.IsNullOrEmpty(userId))
            {
                string storedPassword = redis.HashGet(REDIS_USER_DATA + userId, REDIS_USER_DATA_PASSWORD);
                if (password == storedPassword)
                {
                    token = CreateLoggedUserToken(userId);
                }
            }
            return token;
        }

        public bool IsUserLogged(string email, string token)
        {
            bool result = false;
            IDatabase redis = _redisConn.GetDatabase();
            string userId = redis.HashGet(REDIS_USERS_LIST, email);
            String loggedToken = redis.HashGet(REDIS_USER_DATA + userId, REDIS_USER_DATA_LOGGED_AUTH);
            if (token == loggedToken)
            {
                result = true;
            }
            return result;

        }

        private String CreateLoggedUserToken(string userId)
        {
            IDatabase redis = _redisConn.GetDatabase();

            String token = Guid.NewGuid().ToString();
            redis.HashSet(REDIS_USER_DATA + userId, REDIS_USER_DATA_LOGGED_AUTH, token);
            return token;

        }
    }
}
