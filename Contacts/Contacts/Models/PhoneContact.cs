using SQLite;
using System;

namespace Contacts.Models
{
    public class PhoneContact: IEntityBase
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int Autor { get; set; }
        public string Nick { get; set; }
        public string FullName { get; set; }
        public string Description { get; set; }
        public string Number { get; set; }
        public string PathImage { get; set; }
        public DateTime TimeCreating { get; set; }
    }
}