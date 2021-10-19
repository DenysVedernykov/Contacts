using Acr.UserDialogs;
using Contacts.Models;
using Contacts.Services.Authorization;
using Contacts.Services.Contacts;
using Contacts.Services.Repository;
using Contacts.Services.SettingsManager;
using Contacts.Themes;
using Contacts.ViewModels;
using Contacts.Views;
using Prism.Ioc;
using Prism.Unity;
using System;
using System.Collections.Generic;
using System.Globalization;
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
            //var androidLocale = Java.Util.Locale.Default;
            //var netLanguage = androidLocale.ToString().Replace("_", "-");

            //settingsManager.Lang = "ru-RU";
            try
            {
                Resource.Culture = new System.Globalization.CultureInfo(settingsManager.Lang);
            }
            catch
            {
                Resource.Culture = new System.Globalization.CultureInfo(settingsManager.Lang.Substring(0,2));
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

            //settingsManager.Session = false;
            bool session = settingsManager.Session;
            //settingsManager.Sort = "Nick";
            //var all = repository.GetAllRowsAsync<Contact>();
            //foreach(var i in all.Result)
            //{
            //    repository.DeleteAsync(i);
            //}

            if (session)
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
