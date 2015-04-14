using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvisameSi.ServiceLibrary.Entities
{
    public class UserTokenEntity
    {
        private readonly string _token;
        
        public UserTokenEntity()
        {
            _token = Guid.NewGuid().ToString();
        }

        public UserTokenEntity(string token)
        {
            _token = token;
        }

        public bool IsValid()
        {
            return !String.IsNullOrWhiteSpace(_token);
        }

        public string GetTokenString()
        {
            return _token;
        }

    }
}
