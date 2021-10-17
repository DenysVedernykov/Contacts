using Contacts.Models;
using Contacts.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contacts.Helper
{
    public static class ContactExtension
    {
        public static PhoneContact ToContact(this PhoneContactViewModel contact)
        {
            return new PhoneContact()
            {
                Id = contact.Id,
                Autor = contact.Autor,
                Nick = contact.Nick,
                FullName = contact.FullName,
                Description = contact.Description,
                Number = contact.Number,
                PathImage = contact.PathImage,
                TimeCreating = contact.TimeCreating
            };
        }

        public static PhoneContactViewModel ToContactViewModel(this PhoneContact contact)
        {
            return new PhoneContactViewModel()
            {
                Id = contact.Id,
                Autor = contact.Autor,
                Nick = contact.Nick,
                FullName = contact.FullName,
                Description = contact.Description,
                Number = contact.Number,
                PathImage = contact.PathImage,
                TimeCreating = contact.TimeCreating
            };
        }
    }
}
