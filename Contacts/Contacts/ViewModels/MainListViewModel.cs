using Acr.UserDialogs;
using Contacts.Helper;
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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
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

            Items = new ObservableCollection<PhoneContactViewModel>();
            GetItemsCommand();
        }

        public ICommand OnExitCommand => new Command(ExitCommand);
        public ICommand OnRefreshCommand => new Command(GetItemsCommand);
        public ICommand OnOpenSettingsCommand => new Command(OpenSettingsCommand);
        public ICommand OnOpenAddContactCommand => new Command(OpenAddContactCommand);

        public ObservableCollection<PhoneContactViewModel> Items { get; }

        private bool _isEmpty;
        public bool IsEmpty { get => _isEmpty; set => SetProperty(ref _isEmpty, value); }

        private bool _isRefreshing;
        public bool IsRefreshing { get => _isRefreshing; set => SetProperty(ref _isRefreshing, value); }

        private PhoneContact _selectedItem;
        public PhoneContact SelectedItem { get => _selectedItem; set => SetProperty(ref _selectedItem, value); }

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

        private void GetItemsCommand()
        {
            IsRefreshing = true;
            
            var editCommand = new Command(EditCommand);
            var deleteCommand = new Command(DeleteCommand);

            Items.Clear();

            List<PhoneContact> data = _contacts.GetAllContact(_settingsManager.Sort);
            List<PhoneContactViewModel> list2 = data.Select(x => x.ToContactViewModel()).ToList();

            foreach (var contact in list2)
            {
                contact.DeleteCommand = deleteCommand;
                contact.EditCommand = editCommand;

                Items.Add(contact);
            }

            IsEmpty = data.Count == 0;
            IsRefreshing = false;
        }

        private async void ExitCommand(object obj)
        {
            _authorization.LogOut();
            _settingsManager.Session = false;
            _settingsManager.Login = "";
            _settingsManager.Password = "";

            await _navigationService.NavigateAsync("/NavigationPage/SignInView");

            //await Share.RequestAsync(new ShareFileRequest
            //{
            //    Title = "FN:",
            //    File = new ShareFile(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ContactsBook.db3"))
            //});
        }

        private async void OpenSettingsCommand(object obj)
        {
            await _navigationService.NavigateAsync("SettingsView");
        }

        private async void OpenAddContactCommand(object obj)
        {
            NavigationParameters param = new NavigationParameters("IsCreateMode=true");

            await _navigationService.NavigateAsync("AddEditProfileView", param);
        }

        private void EditCommand(object obj)
        {
            
        }

        private void DeleteCommand(object obj)
        {
            
        }
    }
}
