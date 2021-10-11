using Contacts.Models;
using Contacts.Services.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contacts.Services.Authorization
{
    public class Authorization : IAuthorization
    {
        private IRepository _repository;
        public Authorization(IRepository repository)
        {
            _repository = repository;
        }

        private bool _status;
        private User _profile;

        public bool Status { get => _status; }
        public User Profile { get =>_profile; }

        public bool Reg(string login, string password)
        {
            bool result = false;

            Task<User> user = _repository.SearchUserByLoginAsync<User>(login);
            if (user.Result == null)
            {
                var newUser = new User()
                {
                    Login = login,
                    Password = password,
                    TimeCreating = DateTime.Now
                };

                _repository.InsertAsync(newUser);

                result = true;
            }
            
            return result;
        }
        public bool Login(string login, string password)
        {
            _status = false;
            _profile = null;

            Task<User> user = _repository.SearchUserByLoginAsync<User>(login);
            if (user.Result != null)
            {
                if (user.Result.Password == password)
                {
                    _status = true;
                    _profile = user.Result;
                }
            }

            return _status;
        }

        public void LogOut()
        {
            _status = false;
            _profile = null;
        }
    }
}
