using Contacts.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Contacts.Services.Repository
{
    public class Repository : IRepository
    {
        private Lazy<SQLiteAsyncConnection> _database;

        public Repository()
        {
             _database = new Lazy<SQLiteAsyncConnection>(() =>
            {
                var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ContactsBook.db3");
                var database = new SQLiteAsyncConnection(path);

                database.CreateTableAsync<User>().Wait();
                database.CreateTableAsync<Contact>().Wait();

                return database;
            });
        }
        public Task<int> DeleteAsync<T>(T entity) where T : IEntityBase, new()
        {
            return _database.Value.DeleteAsync(entity);
        }

        public Task<List<T>> GetAllRowsAsync<T>() where T : IEntityBase, new()
        {
            return _database.Value.Table<T>().ToListAsync();
        }

        public Task<int> InsertAsync<T>(T entity) where T : IEntityBase, new()
        {
            return _database.Value.InsertAsync(entity);
        }

        public Task<int> UpdateAsync<T>(T entity) where T : IEntityBase, new()
        {
            return _database.Value.UpdateAsync(entity);
        }

        public Task<T> SearchByIdAsync<T>(int Id) where T : IEntityBase, new()
        {
            return _database.Value.Table<T>().Where(i => i.Id == Id).FirstOrDefaultAsync();
        }
        public Task<User> SearchUserByLoginAsync<T>(string login) where T : IEntityBase, new()
        {
            return _database.Value.Table<User>().Where(row => row.Login == login).FirstOrDefaultAsync();
        }
    }
}