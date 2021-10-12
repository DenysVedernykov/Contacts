using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace Contacts.Services.SettingsManager
{
    public class SettingsManager : ISettingsManager
    {
        public bool Session
        {
            get => Preferences.Get(nameof(Session), false);
            set => Preferences.Set(nameof(Session), value);
        }
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
        public string Sort
        {
            get => Preferences.Get(nameof(Sort), "Nick");
            set => Preferences.Set(nameof(Sort), value);
        }
        public bool NightTheme
        {
            get => Preferences.Get(nameof(NightTheme), false);
            set => Preferences.Set(nameof(NightTheme), value);
        }
        public string Lang
        {
            get => Preferences.Get(nameof(Lang), "en");
            set => Preferences.Set(nameof(Lang), value);
        }
    }
}
