using Contacts.Services.Authorization;
using Contacts.Services.Repository;
using Contacts.Services.SettingsManager;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
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

        public SignInViewModel(ISettingsManager settingsManager, IRepository repository)
        {
            _repository = repository;
            _settingsManager = settingsManager;
            auth = new Authorization(repository);
        }

        //fields user
        private string _login;
        private string _password;

        public string Login { get => _login; set => SetProperty(ref _login, value); }
        public string Password { get => _password; set => SetProperty(ref _password, value); }

        public ICommand OnLoginCommand => new Command(LoginCommand);

        private void LoginCommand(object obj)
        {
            if(auth.Login(_login, _password))
            {
                _settingsManager.Login = _login;
                _settingsManager.Password = _password;
            }
        }
        public ICommand OnRegCommand => new Command(RegCommand);

        private void RegCommand(object obj)
        {
            
        }
    }
}
