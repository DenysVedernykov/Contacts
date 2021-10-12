using Contacts.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contacts.Services.Contacts
{
    public interface IUserContacts
    {
        Task<int> Add(Contact contact);
        Task<int> Update(Contact contact);
        Task<int> Delete(Contact contact);
        List<Contact> GetAllContact(string typeSort);
    }
}
