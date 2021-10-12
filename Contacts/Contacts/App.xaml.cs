using Contacts.Services.Authorization;
using Contacts.Services.Repository;
using Contacts.Services.SettingsManager;
using Contacts.ViewModels;
using Contacts.Views;
using Prism.Ioc;
using Prism.Unity;
using System;
using Unity.Lifetime;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Contacts
{
    public partial class App : PrismApplication
    {
        public App()
        {
        }

        static Repository repository = new Repository();
        static SettingsManager settingsManager = new SettingsManager();
        static Authorization authorization = new Authorization(repository);

        #region --- Ovverides ---
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //Services
            containerRegistry.RegisterInstance<IRepository>(repository);
            containerRegistry.RegisterInstance<ISettingsManager>(settingsManager);
            containerRegistry.RegisterInstance<IAuthorization>(authorization);

            //Navigation
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<AddEditProfileView, AddEditProfileViewModel>();
            containerRegistry.RegisterForNavigation<MainListView, MainListViewModel>();
            containerRegistry.RegisterForNavigation<SettingsView, SettingsViewModel>();
            containerRegistry.RegisterForNavigation<SignInView, SignInViewModel>();
            containerRegistry.RegisterForNavigation<SignUpView, SignUpViewModel>(); 
        }

        protected override void OnInitialized()
        {
            InitializeComponent();
            //settingsManager.Session = false;
            bool session = settingsManager.Session;
            
            if (session)
            {
                //повторная проверка учетных данных, потому что в идеале они могли устареть,
                //если бы, например, авторизация была через Google или просто онлайн
                // + получаем всю инфу про пользователя
                if(authorization.Login(settingsManager.Login, settingsManager.Password))
                {
                    NavigationService.NavigateAsync($"{nameof(MainListView)}");
                }
                else
                {
                    NavigationService.NavigateAsync($"{nameof(SignInView)}");
                }
            }
            else
            {
                NavigationService.NavigateAsync($"{nameof(SignInView)}");
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        #endregion
    }
}
