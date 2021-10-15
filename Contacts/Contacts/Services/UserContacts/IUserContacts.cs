using Contacts.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contacts.Services.Contacts
{
    public interface IUserContacts
    {
        Task<int> Add(PhoneContact contact);
        Task<int> Update(PhoneContact contact);
        Task<int> Delete(PhoneContact contact);
        List<PhoneContact> GetAllContact(string typeSort);
        Task<PhoneContact> GetContactById(int id);
    }
}
