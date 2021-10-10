using Contacts.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contacts.Services.Repository
{
    public interface IRepository
    {
        Task<int> InsertAsync<T>(T entity) where T : IEntityBase, new();
        Task<int> UpdateAsync<T>(T entity) where T : IEntityBase, new();
        Task<int> DeleteAsync<T>(T entity) where T : IEntityBase, new();
        Task<List<T>> GetAllRowsAsync<T>() where T : IEntityBase, new();
    }
}