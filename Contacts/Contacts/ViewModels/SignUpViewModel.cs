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
    class SignUpViewModel : BindableBase
    {
        private ISettingsManager _settingsManager;
        private IAuthorization _authorization;
        private INavigationService _navigationService;

        public SignUpViewModel(ISettingsManager settingsManager, IAuthorization authorization, INavigationService navigationService)
        {
            _settingsManager = settingsManager;
            _authorization = authorization;
            _navigationService = navigationService;

            _enableButton = false;
        }

        private bool _enableButton;
        public bool EnableButton { get => _enableButton; set => SetProperty(ref _enableButton, value); }

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            switch (args.PropertyName)
            {
                case nameof(Login):
                case nameof(Password):
                case nameof(ConfirmPassword):
                    EnableButton = !string.IsNullOrWhiteSpace(Login) && !string.IsNullOrWhiteSpace(Password) && !string.IsNullOrWhiteSpace(ConfirmPassword);
                    break;
            }
        }

        //fields user
        private string _login;
        private string _password;
        private string _confirmPassword;

        public string Login { get => _login; set => SetProperty(ref _login, value); }
        public string Password { get => _password; set => SetProperty(ref _password, value); }
        public string ConfirmPassword { get => _confirmPassword; set => SetProperty(ref _confirmPassword, value); }

        public ICommand OnRegCommand => new Command(RegCommand);
        private async void RegCommand(object obj)
        {
            if (_authorization.LoginMatching(Login))
            {
                if (Password == ConfirmPassword)
                {
                    if (_authorization.PasswordMatching(Password))
                    {
                        if (_authorization.CheckLoginForUse(Login))
                        {
                            _authorization.Registration(Login, Password);
                            _settingsManager.Login = Login;

                            await _navigationService.NavigateAsync("/NavigationPage/SignInView");
                        }
                        else
                        {
                            await UserDialogs.Instance.AlertAsync(new AlertConfig()
                            {
                                Message = Resource.ResourceManager.GetString("LoginTaken", Resource.Culture),
                                OkText = Resource.ResourceManager.GetString("Ok", Resource.Culture)
                            });
                        }
                    }
                    else
                    {
                        await UserDialogs.Instance.AlertAsync(new AlertConfig()
                        {
                            Message = Resource.ResourceManager.GetString("PasswordNoMatching", Resource.Culture),
                            OkText = Resource.ResourceManager.GetString("Ok", Resource.Culture)
                        });
                    }
                }
                else
                {
                    await UserDialogs.Instance.AlertAsync(new AlertConfig()
                    {
                        Message = Resource.ResourceManager.GetString("FieldsMustMatch", Resource.Culture),
                        OkText = Resource.ResourceManager.GetString("Ok", Resource.Culture)
                    });
                }
            }
            else
            {
                await UserDialogs.Instance.AlertAsync(new AlertConfig()
                {
                    Message = Resource.ResourceManager.GetString("LoginNoMAtching", Resource.Culture),
                    OkText = Resource.ResourceManager.GetString("Ok", Resource.Culture)
                });
            }
        }
    }
}
