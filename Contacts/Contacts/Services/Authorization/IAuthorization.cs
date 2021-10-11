using Contacts.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contacts.Services.Authorization
{
    public interface IAuthorization
    {
        bool Status { get; }
        User Profile { get;}

        bool Reg(string login, string password);
        bool Login(string login, string password);
        void LogOut();
    }
}
