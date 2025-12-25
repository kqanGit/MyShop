using Microsoft.UI.Xaml;
using MyShop_Frontend.Contracts; 
using MyShop_Frontend.Helpers.Command;
using MyShop_Frontend.Services;
using MyShop_Frontend.Servies;
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

        private readonly IAuthenticationService _authService;
        private bool _isBusy;
        private  WindowsService _windowsService = new WindowsService();
        public bool IsBusy
        {
            get => _isBusy;
            set { _isBusy = value; OnPropertyChanged(nameof(IsBusy)); SignInCommand.RaiseCanExecuteChanged(); }
        }

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
            _authService = new AuthenticationService();


            SignInCommand = new RelayCommand(
                execute: _ => _login(),
                canExecute: _ => !IsBusy && !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password)
            );
        }

        public async Task _login()
        {
            IsBusy = true;
            var token = await _authService.LoginAsync(Username, Password);
            Debug.WriteLine($"Token nhận được: {(token != null ? "Có dữ liệu" : "NULL")}");

            //Waiting for fixing navigation service!
            //if (!string.IsNullOrEmpty(token))
            //{
            //    Debug.WriteLine("Đăng nhập thành công! Token: " + token);
            //    _windowsService.ShowMainWindow();
            //    // TODO: Lưu token và điều hướng sang trang chính
            //    // App.CurrentServices.GetService<INavigationService>().NavigateTo("MainPage");
            //}
            //else
            //{
            //    Debug.WriteLine("Sai tài khoản hoặc mật khẩu");

            //}


            _windowsService.ShowMainWindow();
            IsBusy = false;
        }
    }
}
