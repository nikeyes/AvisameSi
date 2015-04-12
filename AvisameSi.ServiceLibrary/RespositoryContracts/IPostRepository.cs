using AvisameSi.ServiceLibrary.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvisameSi.ServiceLibrary.RespositoryContracts
{
    public interface IPostRepository
    {
        void SavePost(Post post);
        Post GetPost(string postId);
        IEnumerable<Post> GetGlobalTimeline(int start, int numElements);
    }
}
