using Contacts.Services.Authorization;
using Contacts.Services.Contacts;
using Contacts.Services.Repository;
using Contacts.Services.SettingsManager;
using Contacts.Themes;
using Contacts.ViewModels;
using Contacts.Views;
using Prism.Ioc;
using Prism.Unity;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Contacts
{
    public partial class App : PrismApplication
    {
        public App()
        {
        }

        #region --- Ovverides ---
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //Services
            containerRegistry.RegisterInstance<IRepository>(Container.Resolve<Repository>());
            containerRegistry.RegisterInstance<ISettingsManager>(Container.Resolve<SettingsManager>());
            containerRegistry.RegisterInstance<IAuthorization>(Container.Resolve<Authorization>());
            containerRegistry.RegisterInstance<IUserContacts>(Container.Resolve<UserContacts>());

            //Navigation
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<AddEditProfileView, AddEditProfileViewModel>();
            containerRegistry.RegisterForNavigation<MainListView, MainListViewModel>();
            containerRegistry.RegisterForNavigation<SettingsView, SettingsViewModel>();
            containerRegistry.RegisterForNavigation<SignInView, SignInViewModel>();
            containerRegistry.RegisterForNavigation<SignUpView, SignUpViewModel>();

            containerRegistry.RegisterDialog<DialogView, DialogViewModel>();
        }

        protected override void OnInitialized()
        {
            InitializeComponent();
            
            var authorization = Container.Resolve<IAuthorization>();
            var settingsManager = Container.Resolve<ISettingsManager>();

            try
            {
                Resource.Culture = new System.Globalization.CultureInfo(settingsManager.Lang);
            }
            catch
            {
                Resource.Culture = new System.Globalization.CultureInfo(settingsManager.Lang.Substring(0, 2));
            }

            ICollection<ResourceDictionary> mergedDictionaries = PrismApplication.Current.Resources.MergedDictionaries;
            if (mergedDictionaries != null)
            {
                mergedDictionaries.Clear();

                if (settingsManager.NightTheme)
                {
                    mergedDictionaries.Add(new DarkTheme());
                }
                else
                {
                    mergedDictionaries.Add(new LightTheme());
                }
            }

            if (settingsManager.Session)
            {
                //повторная проверка учетных данных, потому что в идеале они могли устареть,
                //если бы, например, авторизация была через Google или просто онлайн
                // + получаем всю инфу про пользователя
                if(authorization.Login(settingsManager.Login, settingsManager.Password))
                {
                    NavigationService.NavigateAsync("/NavigationPage/MainListView");
                }
                else
                {
                    NavigationService.NavigateAsync("/NavigationPage/SignInView");
                }
            }
            else
            {
                NavigationService.NavigateAsync("/NavigationPage/SignInView");
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
