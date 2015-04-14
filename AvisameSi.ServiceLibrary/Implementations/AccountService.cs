using AvisameSi.ServiceLibrary.Entities;
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

        public UserTokenEntity Register(UserEntity user)
        {
            if (string.IsNullOrEmpty(user.Email))
                throw new ArgumentNullException("email");
            if (string.IsNullOrEmpty(user.Password))
                throw new ArgumentNullException("password");

            

            var exists = _accountRepository.UserExist(user.Email);
            if (exists)
            {
                throw new Exception("The user already exists");
            }

            return _accountRepository.RegisterUser(user);
            
        }

        public UserTokenEntity Login(UserEntity user)
        {
            if (string.IsNullOrEmpty(user.Email))
                throw new ArgumentNullException("email");
            if (string.IsNullOrEmpty(user.Password))
                throw new ArgumentNullException("password");

            return _accountRepository.Login(user);
        }

        public bool IsUserLogged(UserEntity user)
        {
            if (string.IsNullOrEmpty(user.Email))
                throw new ArgumentNullException("email");
            if (!user.LoggedAuthToken.IsValid())
                throw new ArgumentNullException("token");

            return _accountRepository.IsUserLogged(user);
        }
    }
}
