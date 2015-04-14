using AvisameSi.ServiceLibrary.RespositoryContracts;
using System;
using StackExchange.Redis;
using AvisameSi.ServiceLibrary.Entities;


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

        public UserTokenEntity RegisterUser(UserEntity user)
        {
            IDatabase redis = _redisConn.GetDatabase();
            long userId = redis.StringIncrement(REDIS_SEQUENCE_USERS_ID);
            redis.HashSet(REDIS_USERS_LIST, user.Email, userId.ToString());
            HashEntry[] hashEntryArray = new HashEntry[] {
                new HashEntry(REDIS_USER_DATA_EMAIL, user.Email),
                new HashEntry(REDIS_USER_DATA_PASSWORD, user.Password)
            };

            redis.HashSet(REDIS_USER_DATA + userId, hashEntryArray);

            redis.SortedSetAdd(REDIS_USERS_BY_CREATIONDATE, user.Email, DateTime.Now.Ticks);

            UserTokenEntity token = CreateLoggedUserToken(userId.ToString());
            return  token;
        }


        public UserTokenEntity Login(UserEntity user)
        {
            UserTokenEntity token = null;
            IDatabase redis = _redisConn.GetDatabase();
            string userId = redis.HashGet(REDIS_USERS_LIST, user.Email);
            if (!string.IsNullOrEmpty(userId))
            {
                string storedPassword = redis.HashGet(REDIS_USER_DATA + userId, REDIS_USER_DATA_PASSWORD);
                if (user.Password == storedPassword)
                {
                    token = CreateLoggedUserToken(userId);
                }
            }
            return token;
        }

        public bool IsUserLogged(UserEntity user)
        {
            bool result = false;
            IDatabase redis = _redisConn.GetDatabase();
            string userId = redis.HashGet(REDIS_USERS_LIST, user.Email);
            String loggedToken = redis.HashGet(REDIS_USER_DATA + userId, REDIS_USER_DATA_LOGGED_AUTH);
            if (user.LoggedAuthToken.GetTokenString() == loggedToken)
            {
                result = true;
            }
            return result;

        }

        private UserTokenEntity CreateLoggedUserToken(string userId)
        {
            IDatabase redis = _redisConn.GetDatabase();
            UserTokenEntity userToken = new UserTokenEntity();
            redis.HashSet(REDIS_USER_DATA + userId, REDIS_USER_DATA_LOGGED_AUTH, userToken.GetTokenString());
            return userToken;

        }
    }
}
