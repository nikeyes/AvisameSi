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
        void SavePost(PostEntity post);
        PostEntity GetPost(string postId);
        IEnumerable<PostEntity> GetGlobalTimeline(int start, int numElements);
    }
}
