using Contacts.Models;
using Contacts.Services.Authorization;
using Contacts.Services.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            var result = _repository.GetAllRowsAsync<PhoneContact>();
            if (result == null)
            {
                return null;
            }
            else
            {
                List<PhoneContact> list = result.Result;
                return list.Where(row => row.Autor == _authorization.Profile.Id).OrderBy(row => row.GetType().GetProperty(typeSort).GetValue(row, null)).ToList();
            }
        }

        public Task<PhoneContact> GetContactById(int id)
        {
            return  _repository.SearchByIdAsync<PhoneContact>(id);
        }
    }
}
