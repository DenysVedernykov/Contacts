using Contacts.Models;
using Contacts.Services.Repository;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Contacts.ViewModels
{
    class AddEditProfileViewModel : BindableBase
    {
        //settings page
        public string Title { get; set; }
        public bool IsCreateMode { get; set; }

        private IRepository _repository;
        public AddEditProfileViewModel(IRepository repository)
        {
            _repository = repository;

            IsCreateMode = true;

            // ИСПРАВИТЬ
            Title = "Add Contact";
            _pathImage = "user.png";
        }

        //fields contact
        private string _nick;
        private string _fullName;
        private string _description;
        private string _number;
        private string _pathImage;

        public string Nick { get => _nick; set => SetProperty(ref _nick, value); }
        public string FullName { get => _fullName; set => SetProperty(ref _fullName, value); }
        public string Description { get => _description; set => SetProperty(ref _description, value); }
        public string Number { get => _number; set => SetProperty(ref _number, value); }
        public string PathImage { get => _pathImage; set => SetProperty(ref _pathImage, value); }

        public ICommand OnAddContactCommand => new Command(AddContactCommand);

        private async void AddContactCommand(object obj)
        {
            if (IsCreateMode)
            {
                var contact = new Contact()
                {
                    Autor = 1, // ИСПРАВИТЬ
                    Nick = _fullName,
                    FullName = _fullName,
                    Description = _description,
                    Number = _number,
                    PathImage = _pathImage,
                    TimeCreating = DateTime.Now
                };

                await _repository.InsertAsync(contact);

                Nick = "";
                FullName = "";
                Description = "";
                Number = "";
                PathImage = "user.png";
            }
            else
            {
                //update
            }
        }
    }
}