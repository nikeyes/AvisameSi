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
        void RegisterUser(string email, string password);
        bool Login(string email, string password);
    }
}
