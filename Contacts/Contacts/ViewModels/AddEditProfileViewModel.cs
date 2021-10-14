using Contacts.Models;
using Contacts.Services.Authorization;
using Contacts.Services.Contacts;
using Contacts.Services.Repository;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        private IUserContacts _contacts;
        private IAuthorization _authorization;
        private INavigationService _navigationService;
        public AddEditProfileViewModel(IUserContacts contacts, IAuthorization authorization, INavigationService navigationService)
        {
            _contacts = contacts;
            _authorization = authorization;
            _navigationService = navigationService;

            IsEnable = false;
            IsCreateMode = true;

            if (IsCreateMode)
            {
                Title = "Add Contact";
            }
            else
            {
                Title = "Edit Contact";
            }

            _pathImage = "user.png";
        }

        private bool _isEnable;
        public bool IsEnable
        {
            get => _isEnable;
            set
            {
                if (SetProperty(ref _isEnable, value))
                {
                    RaisePropertyChanged(nameof(OnSaveContactCommand));
                }
            }
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

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            switch (args.PropertyName)
            {
                case nameof(Nick):
                    IsEnable = !string.IsNullOrWhiteSpace(Nick) && !string.IsNullOrWhiteSpace(FullName);
                    break;
                case nameof(FullName):
                    IsEnable = !string.IsNullOrWhiteSpace(Nick) && !string.IsNullOrWhiteSpace(FullName);
                    break;
            }
        }

        public ICommand OnSaveContactCommand => new Command(
            execute: () =>
            {
                if (IsCreateMode)
                {
                    var contact = new Contact()
                    {
                        Autor = _authorization.Profile.Id,
                        Nick = _nick,
                        FullName = _fullName,
                        Description = _description,
                        Number = _number,
                        PathImage = _pathImage,
                        TimeCreating = DateTime.Now
                    };

                    _contacts.Add(contact);

                    _navigationService.GoBackAsync();
                }
                else
                {
                    //update
                }

                Nick = "";
                FullName = "";
                Description = "";
                Number = "";
                PathImage = "user.png";
            },
            canExecute: () =>
            {
                return IsEnable;
            }
        );
    }
}