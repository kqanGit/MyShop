using Microsoft.UI.Xaml;
using MyShop_Frontend.Helpers.Command;
using MyShop_Frontend.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop_Frontend.ViewModels.Authentication
{
    public class LoginViewModel : ViewModelBase
    {
        private string _username = "";
        public string Username
        {
            get => _username;
            set
            {
                if (_username != value)
                {
                    _username = value;
                    OnPropertyChanged(nameof(Username));
                    SignInCommand.RaiseCanExecuteChanged();
                }
            }
        }
        private string _password = "";
        public string Password
        {
            get => _password;
            set
            {
                if (_password != value)
                {
                    _password = value;
                    OnPropertyChanged(nameof(Password));
                    SignInCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public RelayCommand SignInCommand { get; }

        public LoginViewModel()
        {
            SignInCommand = new RelayCommand(
                execute: _ => _login(),
                canExecute: _ => !string.IsNullOrWhiteSpace(Username)   
                && !string.IsNullOrWhiteSpace(Password)
            );
        }

        private void _login()
        {
            return;
        }
    }
}
