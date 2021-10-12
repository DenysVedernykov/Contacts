using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contacts.Models
{
    public class Contact: IEntityBase
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