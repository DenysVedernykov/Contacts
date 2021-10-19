using Contacts.Services.SettingsManager;
using Contacts.Themes;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Unity;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace Contacts.ViewModels
{
    class SettingsViewModel : BindableBase
    {
        public string Nick { get; }
        public string FullName { get; }
        public string TimeCreating { get; }

        private bool _isChecked1;
        private bool _isChecked2;
        private bool _isChecked3;
        private bool _isToggled;
        private int _selectedIndex;
        private List<string> _items;

        public bool IsChecked1 { get => _isChecked1; set => SetProperty(ref _isChecked1, value); }
        public bool IsChecked2 { get => _isChecked2; set => SetProperty(ref _isChecked2, value); }
        public bool IsChecked3 { get => _isChecked3; set => SetProperty(ref _isChecked3, value); }
        public bool IsToggled { get => _isToggled; set => SetProperty(ref _isToggled, value); }
        public int SelectedIndex { get => _selectedIndex; set => SetProperty(ref _selectedIndex, value); }
        public List<string> Items { get => _items; set => SetProperty(ref _items, value); }

        private Dictionary<string, string> langs = new Dictionary<string, string>
        {
            { "English", "en-US" },
            { "Русский", "ru-RU" },
            { "Українська", "uk-UA" }
        };

        private ISettingsManager _settingsManager;
        private INavigationService _navigationService;
        public SettingsViewModel(ISettingsManager settingsManager, INavigationService navigationService)
        {
            Nick = Resource.ResourceManager.GetString("Nick", Resource.Culture);
            FullName = Resource.ResourceManager.GetString("FullName", Resource.Culture);
            TimeCreating = Resource.ResourceManager.GetString("TimeCreating", Resource.Culture);

            _settingsManager = settingsManager;
            _navigationService = navigationService;

            if (_settingsManager.Sort == "Nick")
            {
                _isChecked1 = true;
            }
            else if (_settingsManager.Sort == "FullName")
            {
                _isChecked2 = true;
            }
            else if (_settingsManager.Sort == "TimeCreating")
            {
                _isChecked3 = true;
            }

            IsToggled = _settingsManager.NightTheme;

            Items = new List<string>(langs.Select(x => x.Key).ToList());

            _selectedIndex = langs.ToList().FindIndex(x => x.Value == _settingsManager.Lang);
        }

        public ICommand OnRefresh => new Command(Refresh);
        private void Refresh(object obj)
        {
            ICollection<ResourceDictionary> mergedDictionaries = PrismApplication.Current.Resources.MergedDictionaries;
            if (mergedDictionaries != null)
            {
                mergedDictionaries.Clear();

                if (IsToggled)
                {
                    mergedDictionaries.Add(new DarkTheme());
                }
                else
                {
                    mergedDictionaries.Add(new LightTheme());
                }
            }

            NavigationParameters param = new NavigationParameters("OpenView=Settings");
            _navigationService.NavigateAsync("/NavigationPage/MainListView", param);
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
                    if (_selectedIndex > -1)
                    {
                        _settingsManager.Lang = langs[Items[SelectedIndex]];

                        Resource.Culture = new System.Globalization.CultureInfo(_settingsManager.Lang);
                    }
                    break;
            }
        }
    }
}
