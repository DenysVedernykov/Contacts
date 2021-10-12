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

        public async Task<int> Add(Contact contact)
        {
            return await _repository.InsertAsync(contact);
        }

        public async Task<int> Delete(Contact contact)
        {
            return await _repository.DeleteAsync(contact);
        }

        public List<Contact> GetAllContact(string typeSort)
        {
            var result = _repository.GetAllRowsAsync<Contact>();
            if (result == null)
            {
                return null;
            }
            else
            {
                List<Contact> list = result.Result;
                return  list.Where(row => row.Autor == _authorization.Profile.Id).OrderBy(row => row.GetType().GetField(typeSort).GetValue(row)).ToList();
            }
        }

        public async Task<int> Update(Contact contact)
        {
            return await _repository.UpdateAsync(contact);
        }
    }
}
