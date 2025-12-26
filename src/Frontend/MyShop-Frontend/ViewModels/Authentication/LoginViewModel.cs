using Microsoft.Extensions.DependencyInjection;
using MyShop_Frontend.Contracts;
using MyShop_Frontend.Helpers.Command;
using MyShop_Frontend.ViewModels.Base;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace MyShop_Frontend.ViewModels.Authentication
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly IAuthenticationService _authService;

        // dùng App.Windows (không new)
        // Nếu App.Windows của bạn là static new WindowsService() thì vẫn ok.
        // Nếu App.Windows resolve từ DI thì cũng ok miễn Services đã init.
        private readonly Services.WindowsService _windowsService;

        public RelayCommand SignInCommand { get; }

        public LoginViewModel()
        {
            // Resolve từ DI (đúng với AddHttpClient + AuthenticationService mới)
            _authService = App.Services.GetRequiredService<IAuthenticationService>();
            _windowsService = App.Windows;

            SignInCommand = new RelayCommand(
                execute: _ => _ = _login(),
                canExecute: _ => !IsBusy
                                 && !string.IsNullOrWhiteSpace(Username)
                                 && !string.IsNullOrWhiteSpace(Password)
            );
        }

        // ===== helper =====
        private bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? name = null)
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(name ?? string.Empty);
            return true;
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                if (SetProperty(ref _isBusy, value))
                    SignInCommand.RaiseCanExecuteChanged();
            }
        }

        private string _username = "";
        public string Username
        {
            get => _username;
            set
            {
                if (SetProperty(ref _username, value))
                    SignInCommand.RaiseCanExecuteChanged();
            }
        }

        private string _password = "";
        public string Password
        {
            get => _password;
            set
            {
                if (SetProperty(ref _password, value))
                    SignInCommand.RaiseCanExecuteChanged();
            }
        }

        private string? _errorMessage;
        public string? ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        // giữ tên _login() để View hiện tại gọi không cần sửa nhiều
        public async Task _login()
        {
            if (IsBusy) return;

            IsBusy = true;
            ErrorMessage = null;

            try
            {
                var token = await _authService.LoginAsync(Username, Password);
                Debug.WriteLine($"Token nhận được: {(token != null ? "Có dữ liệu" : "NULL")}");

                if (!string.IsNullOrWhiteSpace(token))
                {
                    Debug.WriteLine("Đăng nhập thành công! Token: " + token);
                    _windowsService.ShowMainWindow();
                }
                else
                {
                    ErrorMessage = "Sai tài khoản hoặc mật khẩu.";
                    Debug.WriteLine(ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                Debug.WriteLine($"Lỗi hệ thống: {ex}");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
