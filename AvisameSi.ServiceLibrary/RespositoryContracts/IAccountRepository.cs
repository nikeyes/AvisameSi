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
        string RegisterUser(string email, string password);
        string Login(string email, string password);
        bool IsUserLogged(string email, string token);
    }
}
