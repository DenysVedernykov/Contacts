using Acr.UserDialogs;
using Contacts.Services.Authorization;
using Contacts.Services.SettingsManager;
using Prism.Mvvm;
using Prism.Navigation;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace Contacts.ViewModels
{
    class SignInViewModel : BindableBase
    {
        private ISettingsManager _settingsManager;
        private IAuthorization _authorization;
        private INavigationService _navigationService;

        public SignInViewModel(ISettingsManager settingsManager, IAuthorization authorization, INavigationService navigationService)
        {
            _settingsManager = settingsManager;
            _authorization = authorization;
            _navigationService = navigationService;

            _enableButton = false;
            _login = _settingsManager.Login;
        }

        private bool _enableButton;
        public bool EnableButton { get => _enableButton; set => SetProperty(ref _enableButton, value);}

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            switch (args.PropertyName)
            {
                case nameof(Login):
                case nameof(Password):
                    EnableButton = !string.IsNullOrWhiteSpace(Login) && !string.IsNullOrWhiteSpace(Password);
                    break;
            }
        }

        //fields user
        private string _login;
        private string _password;

        public string Login { get => _login; set => SetProperty(ref _login, value); }
        public string Password { get => _password; set => SetProperty(ref _password, value); }

        public ICommand OnLoginCommand => new Command(LoginCommand);
        private async void LoginCommand(object obj)
        {

            if(_authorization.Login(_login, _password))
            {
                _settingsManager.Session = true;

                _settingsManager.Login = _login;
                _settingsManager.Password = _password;

                await _navigationService.NavigateAsync("/NavigationPage/MainListView");

                Login = "";
                Password = "";
            }
            else
            {
                await UserDialogs.Instance.AlertAsync(new AlertConfig()
                {
                    Message = Resource.ResourceManager.GetString("InvalidLoginOrPass", Resource.Culture),
                    OkText = Resource.ResourceManager.GetString("Ok", Resource.Culture)
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
