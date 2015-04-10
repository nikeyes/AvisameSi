using AvisameSi.ServiceLibrary.RespositoryContracts;
using System;

namespace AvisameSi.ServiceLibrary.Implementations
{
    public class AvisameSiService
    {
        private IAccountRepository _accountRepository;

        public AvisameSiService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public void Register(string email, string password)
        {
            if (string.IsNullOrEmpty(email))
                throw new ArgumentNullException("username");
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");

            

            var exists = _accountRepository.UserExist(email);
            if (exists)
            {
                throw new Exception("The user already exists");
            }

            _accountRepository.RegisterUser(email, password);
            
        }

        public bool Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email))
                throw new ArgumentNullException("username");
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");

            return _accountRepository.Login(email, password);
        }
    }
}
