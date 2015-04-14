using AvisameSi.ServiceLibrary.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvisameSi.ServiceLibrary.RespositoryContracts
{
    public interface IAccountRepository
    {
        bool UserExist(string email);
        UserTokenEntity RegisterUser(UserEntity user);
        UserTokenEntity Login(UserEntity user);
        bool IsUserLogged(UserEntity user);
    }
}
