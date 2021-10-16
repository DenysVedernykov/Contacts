using Acr.UserDialogs;
using Contacts.Services.SettingsManager;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Contacts.ViewModels
{
    class SettingsViewModel : BindableBase
    {
        private bool _IsChecked1;
        private bool _IsChecked2;
        private bool _IsChecked3;
        private bool _IsToggled;
        private int _SelectedIndex;
        private List<string> _Items;

        public bool IsChecked1 { get => _IsChecked1; set => SetProperty(ref _IsChecked1, value); }
        public bool IsChecked2 { get => _IsChecked2; set => SetProperty(ref _IsChecked2, value); }
        public bool IsChecked3 { get => _IsChecked3; set => SetProperty(ref _IsChecked3, value); }
        public bool IsToggled { get => _IsToggled; set => SetProperty(ref _IsToggled, value); }
        public int SelectedIndex { get => _SelectedIndex; set => SetProperty(ref _SelectedIndex, value); }
        public List<string> Items { get => _Items; set => SetProperty(ref _Items, value); }

        private Dictionary<string, string> langs = new Dictionary<string, string>
        {
            { "English", "en" },
            { "Русский", "ru" },
            { "Українська", "ua" }
        };

        private ISettingsManager _settingsManager;
        public SettingsViewModel(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager;

            if (_settingsManager.Sort == "Nick")
            {
                _IsChecked1 = true;
            }
            else if (_settingsManager.Sort == "FullName")
            {
                _IsChecked2 = true;
            }
            else if (_settingsManager.Sort == "TimeCreating")
            {
                _IsChecked3 = true;
            }

            IsToggled = _settingsManager.NightTheme;

            Items = new List<string>(langs.Select(x => x.Key).ToList());

            SelectedIndex = langs.ToList().FindIndex(x => x.Value == _settingsManager.Lang);
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            switch (args.PropertyName)
            {
                case nameof(IsChecked1):
                    if (IsChecked1)
                    {
                        _settingsManager.Sort = "Nick";
                    }
                    break;
                case nameof(IsChecked2):
                    if (IsChecked2)
                    {
                        _settingsManager.Sort = "FullName";
                    }
                    break;
                case nameof(IsChecked3):
                    if (IsChecked3)
                    {
                        _settingsManager.Sort = "TimeCreating";
                    }
                    break;
                case nameof(IsToggled):
                    _settingsManager.NightTheme = IsToggled;
                    break;
                case nameof(SelectedIndex):
                    _settingsManager.Lang = langs[Items[SelectedIndex]];
                    break;
            }
        }
    }
}
