using SQLite;
using System;

namespace Contacts.Models
{
    public class User : IEntityBase
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public DateTime TimeCreating { get; set; }
    }
}
