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


        public void SavePost(Post post)
        {
            _postRepository.SavePost(post);
        }

        public Post GetPost(string postId)
        {
            return _postRepository.GetPost(postId);
        }

        public IEnumerable<Post> GetGlobalTimeline(int start, int numElements)
        {
            return _postRepository.GetGlobalTimeline(start, numElements);
        }
    }
}
