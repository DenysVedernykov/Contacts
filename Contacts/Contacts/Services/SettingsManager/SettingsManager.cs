using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace Contacts.Services.SettingsManager
{
    public class SettingsManager : ISettingsManager
    {
        public string Login 
        {
            get => Preferences.Get(nameof(Login), "");
            set => Preferences.Set(nameof(Login), value);
        }
        public string Password 
        {
            get => Preferences.Get(nameof(Password), "");
            set => Preferences.Set(nameof(Password), value);
        }
    }
}
