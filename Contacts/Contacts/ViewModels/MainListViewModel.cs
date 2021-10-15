using Acr.UserDialogs;
using Contacts.Models;
using Contacts.Services.Authorization;
using Contacts.Services.Contacts;
using Contacts.Services.SettingsManager;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Contacts.ViewModels
{
    class MainListViewModel: BindableBase
    {
        private IDialogService _dialogService;
        private IUserContacts _contacts;
        private IAuthorization _authorization;
        private ISettingsManager _settingsManager;
        private INavigationService _navigationService;

        public MainListViewModel(IDialogService dialogService, IUserContacts contacts, IAuthorization authorization, ISettingsManager settingsManager, INavigationService navigationService)
        {
            _dialogService = dialogService;
            _contacts = contacts;
            _authorization = authorization;
            _settingsManager = settingsManager;
            _navigationService = navigationService;

            Items = new ObservableCollection<Contact>();
            GetItemsCommand();
        }

        public ObservableCollection<Contact> Items { get; }

        private Contact _selectedItem;
        public Contact SelectedItem { get => _selectedItem; set => SetProperty(ref _selectedItem, value); }

        private bool _isRefreshing;
        public bool IsRefreshing { get => _isRefreshing; set => SetProperty(ref _isRefreshing, value); }

        private bool _isEmpty;
        public bool IsEmpty { get => _isEmpty; set => SetProperty(ref _isEmpty, value); }

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            switch (args.PropertyName)
            {
                case nameof(SelectedItem):
                    var param = new DialogParameters();
                    param.Add("Id", _selectedItem.Id);

                    _dialogService.ShowDialog("DialogView", param);
                    break;
            }
        }

        public ICommand OnRefreshCommand => new Command(GetItemsCommand);
        private void GetItemsCommand()
        {
            IsRefreshing = true;

            Items.Clear();

            List<Contact> items = _contacts.GetAllContact(_settingsManager.Sort);
            foreach (var item in items)
            {
                Items.Add(item);
            }

            IsEmpty = items.Count == 0;
            IsRefreshing = false;
        }

        public ICommand OnExitCommand => new Command(ExitCommand);
        private async void ExitCommand(object obj)
        {
            _authorization.LogOut();
            _settingsManager.Session = false;
            _settingsManager.Login = "";
            _settingsManager.Password = "";

            await _navigationService.NavigateAsync("/NavigationPage/SignInView");
        }

        public ICommand OnOpenSettingsCommand => new Command(OpenSettingsCommand);
        private async void OpenSettingsCommand(object obj)
        {
            await _navigationService.NavigateAsync("SettingsView");
        }

        public ICommand OnOpenAddContactCommand => new Command(OpenAddContactCommand);
        private async void OpenAddContactCommand(object obj)
        {
            NavigationParameters param = new NavigationParameters("IsCreateMode=true");

            await _navigationService.NavigateAsync("AddEditProfileView", param);
        }
    }
}
