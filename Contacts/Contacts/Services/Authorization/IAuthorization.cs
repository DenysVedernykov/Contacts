using Contacts.Models;

namespace Contacts.Services.Authorization
{
    public interface IAuthorization
    {
        bool Status { get; }
        User Profile { get;}

        bool CheckLoginForUse(string login);
        bool LoginMatching(string login);
        bool PasswordMatching(string password);
        bool Registration(string login, string password);
        bool Login(string login, string password);
        void LogOut();
    }
}
