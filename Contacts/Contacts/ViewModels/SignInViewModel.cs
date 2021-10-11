﻿using Acr.UserDialogs;
using Contacts.Services.Authorization;
using Contacts.Services.Repository;
using Contacts.Services.SettingsManager;
using Contacts.Views;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Contacts.ViewModels
{
    class SignInViewModel : BindableBase
    {
        private IRepository _repository;
        private ISettingsManager _settingsManager;
        private IAuthorization auth;
        private INavigationService _navigationService;

        public SignInViewModel(ISettingsManager settingsManager, IRepository repository, INavigationService navigationService)
        {
            _repository = repository;
            _settingsManager = settingsManager;

            _login = _settingsManager.Login;

            auth = new Authorization(repository);

            _navigationService = navigationService;

            _enableButton = false;
        }

        private bool _enableButton;
        public bool EnableButton { get => _enableButton; set => SetProperty(ref _enableButton, value);}

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            bool isEnable = true;
            if ((Login == "") || (Password == ""))
            {
                isEnable = false;
            }

            EnableButton = isEnable;
        }

        //fields user
        private string _login;
        private string _password;

        public string Login { get => _login; set => SetProperty(ref _login, value); }
        public string Password { get => _password; set => SetProperty(ref _password, value); }

        public ICommand OnLoginCommand => new Command(LoginCommand);

        private async void LoginCommand(object obj)
        {
            if(auth.Login(_login, _password))
            {
                _settingsManager.Login = _login;
                _settingsManager.Password = _password;

                await _navigationService.NavigateAsync("/MainListView");

                Login = "";
                Password = "";
            }
            else
            {
                await UserDialogs.Instance.AlertAsync(new AlertConfig()
                {
                    Message = "Invalid login or password!",
                    OkText = "Ok"
                });

                Password = "";
            }
        }
        public ICommand OnRegCommand => new Command(RegCommand);

        private async void RegCommand(object obj)
        {
            await _navigationService.NavigateAsync("SignUpView");
        }
    }
}
