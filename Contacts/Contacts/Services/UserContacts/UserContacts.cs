using Contacts.Models;
using Contacts.Services.Authorization;
using Contacts.Services.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contacts.Services.Contacts
{
    public class UserContacts : IUserContacts
    {
        IRepository _repository;
        IAuthorization _authorization;
        public UserContacts(IRepository repository, IAuthorization authorization)
        {
            _repository = repository;
            _authorization = authorization;
        }

        public async Task<int> Add(PhoneContact contact)
        {
            return await _repository.InsertAsync(contact);
        }

        public async Task<int> Update(PhoneContact contact)
        {
            return await _repository.UpdateAsync(contact);
        }

        public async Task<int> Delete(PhoneContact contact)
        {
            return await _repository.DeleteAsync(contact);
        }

        public List<PhoneContact> GetAllContact(string typeSort)
        {
            List<PhoneContact> result = null;

            var all = _repository.GetAllRowsAsync<PhoneContact>();
            if (all != null){
                //сортировка по полю
                result = all.Result.
                    Where(row => row.Autor == _authorization.Profile.Id).
                    OrderBy(row => row.GetType().GetProperty(typeSort).GetValue(row, null)).ToList();
            }

            return result;
        }
    }
}
