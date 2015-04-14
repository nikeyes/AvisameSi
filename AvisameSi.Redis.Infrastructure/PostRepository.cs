using AvisameSi.ServiceLibrary.Entities;
using AvisameSi.ServiceLibrary.RespositoryContracts;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvisameSi.Redis.Infrastructure
{
    public class PostRepository: IPostRepository
    {
        private const String REDIS_CHAR_CREATE_KEYS = ":";
        private const String REDIS_SEQUENCE_POST_ID = "sequence_post_id";
        private const String REDIS_POST_DATA = "post" + REDIS_CHAR_CREATE_KEYS;
        private const String REDIS_POST_DATA_EMAIL = "email";
        private const String REDIS_POST_DATA_TIME = "time";
        private const String REDIS_POST_DATA_MESSAGE = "message";
        private const String REDIS_GLOBAL_TIMELINE_LIST = "global_timeline";

        private ConnectionMultiplexer _redisConn;

        public PostRepository(ConnectionMultiplexer redisConn)
        {
            _redisConn = redisConn;
        }

        public void SavePost(PostEntity post)
        {
            CultureInfo culture = new CultureInfo("en-US");

            IDatabase redis = _redisConn.GetDatabase();
            long postId = redis.StringIncrement(REDIS_SEQUENCE_POST_ID);

            HashEntry[] hashEntryArray = new HashEntry[] {
                new HashEntry(REDIS_POST_DATA_EMAIL, post.Email),
                new HashEntry(REDIS_POST_DATA_TIME, post.Time.ToString(culture.DateTimeFormat)),
                new HashEntry(REDIS_POST_DATA_MESSAGE, post.Message),
            };

            redis.HashSet(REDIS_POST_DATA + postId, hashEntryArray);

            redis.ListLeftPush(REDIS_GLOBAL_TIMELINE_LIST, postId.ToString());
            redis.ListTrim(REDIS_GLOBAL_TIMELINE_LIST, 0, 1000);

        }


        public PostEntity GetPost(string postId)
        {
            CultureInfo culture = new CultureInfo("en-US");

            IDatabase redis = _redisConn.GetDatabase();
            Dictionary<RedisValue, RedisValue> hashValues = redis.HashGetAll(REDIS_POST_DATA + postId).ToDictionary();
            return new PostEntity()
            {
                Email = hashValues[REDIS_POST_DATA_EMAIL],
                Time = DateTime.Parse(hashValues[REDIS_POST_DATA_TIME], culture.DateTimeFormat),
                Message = hashValues[REDIS_POST_DATA_MESSAGE]
            };

        }

        public IEnumerable<PostEntity> GetGlobalTimeline(int start, int numElements)
        {
            CultureInfo culture = new CultureInfo("en-US");

            List<PostEntity> postsList = new List<PostEntity>();
            IDatabase redis = _redisConn.GetDatabase();
            
            RedisValue[] listValues = redis.ListRange(REDIS_GLOBAL_TIMELINE_LIST, start, start+numElements);
            
            for (int i=0; i < listValues.Length; i++)
            {
                postsList.Add(GetPost(listValues[i]));
            }

            return postsList;
        }
    }
}
