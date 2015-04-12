using AvisameSi.ServiceLibrary.RespositoryContracts;
using System;

namespace AvisameSi.ServiceLibrary.Implementations
{
    public class AccountService
    {
        private IAccountRepository _accountRepository;

        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public string Register(string email, string password)
        {
            if (string.IsNullOrEmpty(email))
                throw new ArgumentNullException("email");
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");

            

            var exists = _accountRepository.UserExist(email);
            if (exists)
            {
                throw new Exception("The user already exists");
            }

            string token = _accountRepository.RegisterUser(email, password);
            return token;
            
        }

        public string Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email))
                throw new ArgumentNullException("email");
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");

            return _accountRepository.Login(email, password);
        }

        public bool IsUserLogged(String email, String token)
        {
            if (string.IsNullOrEmpty(email))
                throw new ArgumentNullException("email");
            if (string.IsNullOrEmpty(token))
                throw new ArgumentNullException("token");

            return _accountRepository.IsUserLogged(email, token);
        }
    }
}
