using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Text;


using Prism.AppModel;
using Contacts.Services.Contacts;
using Contacts.Models;

namespace Contacts.ViewModels
{
    public class DialogViewModel : BindableBase, IDialogAware
    {
        private IUserContacts _contacts;
        public DialogViewModel(IUserContacts contacts)
        {
            _contacts = contacts;
            CloseCommand = new DelegateCommand(() => RequestClose(null));
        }

        //fields contact
        private string _nick;
        private string _fullName;
        private string _description;
        private string _number;
        private string _pathImage;

        public string Nick { get => _nick; set => SetProperty(ref _nick, value); }
        public string FullName { get => _fullName; set => SetProperty(ref _fullName, value); }
        public string Description { get => _description; set => SetProperty(ref _description, value); }
        public string Number { get => _number; set => SetProperty(ref _number, value); }
        public string PathImage { get => _pathImage; set => SetProperty(ref _pathImage, value); }

        public DelegateCommand CloseCommand { get; }
        public event Action<IDialogParameters> RequestClose;

        public bool CanCloseDialog() => true;

        public void OnDialogClosed()
        {

        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            var res = _contacts.GetContactById(Convert.ToInt32(parameters["Id"].ToString()));
            if (res != null)
            {
                if(res.Result != null)
                {
                    Nick = res.Result.Nick;
                    FullName = res.Result.FullName;
                    Description = res.Result.Description;
                    Number = res.Result.Number;
                    PathImage = res.Result.PathImage;
                }
            }
        }
    }
}
