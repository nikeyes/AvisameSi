using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvisameSi.ServiceLibrary.Entities
{
    public class UserEntity
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public UserTokenEntity LoggedAuthToken { get; set; }

    }
}
