using Contacts.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contacts.Services.Authorization
{
    public interface IAuthorization
    {
        bool Status { get; }
        User Profile { get;}

        bool CheckForUse(string login);
        bool loginMatching(string login);
        bool passwordMatching(string password);
        bool Reg(string login, string password);
        bool Login(string login, string password);
        Task<bool> ChangePassword(string newPassword);
        void LogOut();
    }
}
