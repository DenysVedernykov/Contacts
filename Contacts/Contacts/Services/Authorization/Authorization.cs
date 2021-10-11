using Contacts.Models;
using Contacts.Services.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
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

        public bool CheckForUse(string login)
        {
            bool result = false;

            Task<User> user = _repository.SearchUserByLoginAsync<User>(login);
            if (user.Result == null)
            {
                result = true;
            }

            return result;
        }

        public bool loginMatching(string login)
        {
            bool result = true;

            if (login.Length < 4 || login.Length > 16)
            {
                result = false;
            }
            else
            {
                //убираем пробелы, включаем однострочный режим, ищем цифру
                if (!Regex.IsMatch(login.Trim(), @"^[0-9]", RegexOptions.Singleline))
                {
                    result = false;
                }
            }

            return result;
        }

        public bool passwordMatching(string password)
        {
            bool result = true;

            if (password.Length < 8 || password.Length > 16)
            {
                result = false;
            }
            else
            {
                //убираем пробелы, включаем однострочный режим, ищем минимум одну заглавную букву, одну строчную букву и одну цифру
                if (!(Regex.IsMatch(password.Trim(), @"[0-9]", RegexOptions.Singleline) 
                    && Regex.IsMatch(password.Trim(), @"[a-z]", RegexOptions.Singleline)
                    && Regex.IsMatch(password.Trim(), @"[A-Z]", RegexOptions.Singleline)))
                {
                    result = false;
                }
            }

            return result;
        }

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
        public async Task<bool> ChangePassword(string newPassword)
        {
            string tmp = _profile.Password;

            _profile.Password = newPassword;
            int res = await _repository.UpdateAsync(_profile);

            if (res > 0)
            {
                return true;
            }
            else
            {
                //в случаи неудачи возврат старого пароля
                _profile.Password = tmp;
                return false;
            }
        }

        public void LogOut()
        {
            _status = false;
            _profile = null;
        }
    }
}
