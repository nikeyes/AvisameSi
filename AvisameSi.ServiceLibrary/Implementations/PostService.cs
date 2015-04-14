using AvisameSi.ServiceLibrary.Entities;
using AvisameSi.ServiceLibrary.RespositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvisameSi.ServiceLibrary.Implementations
{
    public class PostService
    {
         private IPostRepository _postRepository;

        public PostService(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }


        public void SavePost(PostEntity post)
        {
            _postRepository.SavePost(post);
        }

        public PostEntity GetPost(string postId)
        {
            return _postRepository.GetPost(postId);
        }

        public IEnumerable<PostEntity> GetGlobalTimeline(int start, int numElements)
        {
            return _postRepository.GetGlobalTimeline(start, numElements);
        }
    }
}
