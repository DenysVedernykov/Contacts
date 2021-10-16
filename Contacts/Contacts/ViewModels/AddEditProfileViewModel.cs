using Acr.UserDialogs;
using Contacts.Models;
using Contacts.Services.Authorization;
using Contacts.Services.Contacts;
using Contacts.Services.Repository;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Contacts.ViewModels
{
    class AddEditProfileViewModel : BindableBase, IInitialize
    {
        private PhoneContact editContact;
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
            _pathImage = "user.png";
        }

        public void Initialize(INavigationParameters parameters)
        {
            if (parameters.Count > 0)
            {
                string param = parameters["IsCreateMode"].ToString();

                if (param == "true")
                {
                    IsCreateMode = true;
                    Title = "Add Contact";
                }
                else
                {
                    IsCreateMode = false;
                    Title = "Edit Contact";
                    editContact = parameters["Contact"] as PhoneContact;

                    Nick = editContact.Nick;
                    FullName = editContact.FullName;
                    Description = editContact.Description;
                    Number = editContact.Number;
                    PathImage = editContact.PathImage;
                }
            }
        }

        //settings page
        public string Title { get; set; }
        public bool IsCreateMode { get; set; }
        private bool _IsEnable;
        public bool IsEnable
        {
            get => _IsEnable;
            set
            {
                if (SetProperty(ref _IsEnable, value))
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
                case nameof(Description):
                    IsEnable = Description.Length <= 120;
                    break;
                case nameof(Number):
                    IsEnable = Number.Length <= 20 && Regex.IsMatch(Number.Trim(), @"^[+]?[0-9]{5,20}$", RegexOptions.Singleline);
                    break;
            }
        }

        public ICommand OnSaveContactCommand => new Command(
            execute: () =>
            {
                if (IsCreateMode)
                {
                    var contact = new PhoneContact()
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
                }
                else
                {
                    //update
                    editContact.Nick = _nick;
                    editContact.FullName = _fullName;
                    editContact.Description = _description;
                    editContact.Number = _number;
                    editContact.PathImage = _pathImage;

                    _contacts.Update(editContact);
                }

                NavigationParameters param = new NavigationParameters("Refresh=true");
                _navigationService.GoBackAsync(param);
            },
            canExecute: () =>
            {
                return IsEnable;
            }
        );

        public ICommand OnTapImageContact => new Command(async (obj) =>
        {
            
        });

        public ICommand OnImageFromGallery => new Command(async (obj) =>
        {
            var photo = await MediaPicker.PickPhotoAsync();
            PathImage = photo.FullPath;
        });

        public ICommand OnImageFromCamera => new Command(async (obj) =>
        {
            try
            {
                var photo = await MediaPicker.CapturePhotoAsync(new MediaPickerOptions
                {
                    Title = $"xamarin.{DateTime.Now.ToString("dd.MM.yyyy_hh.mm.ss")}.png"
                });

                var newFile = Path.Combine(FileSystem.AppDataDirectory, photo.FileName);

                using (var stream = await photo.OpenReadAsync())
                using (var newStream = File.OpenWrite(newFile))
                await stream.CopyToAsync(newStream);

                PathImage = photo.FullPath;
            }
            catch (Exception e)
            {
                await UserDialogs.Instance.AlertAsync(new AlertConfig()
                {
                    Title = "Error message",
                    Message = e.Message,
                    OkText = "Ok"
                });
            }
       });
    }
}