using Contacts.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contacts.Services.Contacts
{
    public interface IUserContacts
    {
        Task<int> Add(PhoneContact contact);
        Task<int> Update(PhoneContact contact);
        Task<int> Delete(PhoneContact contact);
        List<PhoneContact> GetAllContact(string typeSort);
    }
}
