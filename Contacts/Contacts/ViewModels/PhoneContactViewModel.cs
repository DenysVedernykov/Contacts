using Prism.Mvvm;
using System;
using System.Windows.Input;

namespace Contacts.ViewModels
{
    public class PhoneContactViewModel : BindableBase
    {
        private int _Id;
        private int _Autor;
        private string _Nick;
        private string _FullName;
        private string _Description;
        private string _Number;
        private string _PathImage;
        private DateTime _TimeCreating;

        public int Id
        {
            get => _Id;
            set => SetProperty(ref _Id, value);
        }
        public int Autor
        {
            get => _Autor;
            set => SetProperty(ref _Autor, value);
        }

        public string Nick
        {
            get => _Nick;
            set => SetProperty(ref _Nick, value);
        }
        public string FullName
        {
            get => _FullName;
            set => SetProperty(ref _FullName, value);
        }
        public string Description
        {
            get => _Description;
            set => SetProperty(ref _Description, value);
        }
        public string Number
        {
            get => _Number;
            set => SetProperty(ref _Number, value);
        }
        public string PathImage
        {
            get => _PathImage;
            set => SetProperty(ref _PathImage, value);
        }
        public DateTime TimeCreating
        {
            get => _TimeCreating;
            set => SetProperty(ref _TimeCreating, value);
        }

        private ICommand _DeleteCommand;
        public ICommand DeleteCommand
        {
            get => _DeleteCommand;
            set => SetProperty(ref _DeleteCommand, value);
        }

        private ICommand _EditCommand;
        public ICommand EditCommand
        {
            get => _EditCommand;
            set => SetProperty(ref _EditCommand, value);
        }
    }
}
