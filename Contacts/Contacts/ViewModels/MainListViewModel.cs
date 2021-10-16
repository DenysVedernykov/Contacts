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
    class MainListViewModel: BindableBase, INavigationAware
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
        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            
        }
        public void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.Count > 0)
            {
                string param = parameters["Refresh"].ToString();

                if (param == "true")
                {
                    GetItemsCommand();
                }
            }
        }

        public ICommand OnItemTapped => new Command(ItemTapped);
        public ICommand OnExitCommand => new Command(ExitCommand);
        public ICommand OnRefreshCommand => new Command(GetItemsCommand);
        public ICommand OnOpenSettingsCommand => new Command(OpenSettingsCommand);
        public ICommand OnOpenAddContactCommand => new Command(OpenAddContactCommand);

        public ObservableCollection<PhoneContactViewModel> Items { get; }

        private bool _isEmpty;
        public bool IsEmpty { get => _isEmpty; set => SetProperty(ref _isEmpty, value); }

        private bool _isRefreshing;
        public bool IsRefreshing { get => _isRefreshing; set => SetProperty(ref _isRefreshing, value); }

        private PhoneContactViewModel _selectedItem;
        public PhoneContactViewModel SelectedItem { get => _selectedItem; set => SetProperty(ref _selectedItem, value); }

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

        private void ItemTapped(object obj)
        {
            if (_selectedItem != null)
            {
                var param = new DialogParameters();
                param.Add("Id", _selectedItem.Id);

                _dialogService.ShowDialog("DialogView", param);
            }
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

        private void OpenAddContactCommand(object obj)
        {
            NavigationParameters param = new NavigationParameters("IsCreateMode=true");

            _navigationService.NavigateAsync("AddEditProfileView", param);
        }

        private void EditCommand(object obj)
        {
            PhoneContactViewModel tmp = obj as PhoneContactViewModel;
            PhoneContact contact = tmp.ToContact();

            NavigationParameters param = new NavigationParameters("IsCreateMode=false");
            param.Add("Contact", contact);

            _navigationService.NavigateAsync("AddEditProfileView", param);
        }

        private async void DeleteCommand(object obj)
        {
            PhoneContactViewModel tmp = obj as PhoneContactViewModel;
            PhoneContact contact = tmp.ToContact();

            var confirmConfig = new ConfirmConfig()
            {
                Message = "do you confirm the deletion?",
                OkText = "Ok",
                CancelText = "Cancel"
            };

            bool confirm = await UserDialogs.Instance.ConfirmAsync(confirmConfig);

            if (confirm)
            {
                await _contacts.Delete(contact);
                GetItemsCommand();
            }
        }
    }
}
