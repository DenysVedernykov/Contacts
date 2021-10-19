using Contacts.Models;
using Contacts.Services.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
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

        private User SearchUserByLogin(string login)
        {
            User result = null;

            Task<List<User>> all = _repository.GetAllRowsAsync<User>();
            if(all != null)
            {
                if(all.Result != null)
                {
                    result = all.Result.Where(row => row.Login == login).FirstOrDefault();
                }
            }

            return result;
        }
        public bool CheckLoginForUse(string login)
        {
            bool result = false;

            User user = SearchUserByLogin(login);
            if (user == null)
            {
                result = true;
            }

            return result;
        }

        public bool LoginMatching(string login)
        {
            bool result = true;

            if (login.Length < 4 || login.Length > 16)
            {
                result = false;
            }
            else
            {
                //убираем пробелы, включаем однострочный режим, ищем цифру
                if (Regex.IsMatch(login.Trim(), @"^[0-9]", RegexOptions.Singleline))
                {
                    result = false;
                }
            }

            return result;
        }

        public bool PasswordMatching(string password)
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

        public bool Registration(string login, string password)
        {
            bool result = false;

            User user = SearchUserByLogin(login);
            if (user == null)
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

            User user = SearchUserByLogin(login);
            if (user != null)
            {
                if (user.Password == password)
                {
                    _status = true;
                    _profile = user;
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
