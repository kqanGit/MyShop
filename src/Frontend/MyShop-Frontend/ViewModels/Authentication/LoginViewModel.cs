using Microsoft.Extensions.DependencyInjection;
using MyShop_Frontend.Contracts;
using MyShop_Frontend.Contracts.Services;
using MyShop_Frontend.Helpers.Command;
using MyShop_Frontend.Services;
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
        private readonly ITokenStore _tokenStore;
        private readonly WindowsService _windowsService;

        public RelayCommand SignInCommand { get; }

        public LoginViewModel()
        {
            _authService = App.Services.GetRequiredService<IAuthenticationService>();
            _tokenStore = App.Services.GetRequiredService<ITokenStore>();
            _windowsService = App.Windows;

            SignInCommand = new RelayCommand(
                execute: _ => _ = LoginAsync(),
                canExecute: _ => !IsBusy
                                 && !string.IsNullOrWhiteSpace(Username)
                                 && !string.IsNullOrWhiteSpace(Password)
            );

            // Load saved credentials nếu Remember Me được bật
            LoadSavedCredentials();
        }

        private void LoadSavedCredentials()
        {
            if (_tokenStore.GetRememberMe())
            {
                RememberMe = true;
                Username = _tokenStore.GetSavedUsername() ?? "";
                Password = _tokenStore.GetSavedPassword() ?? "";
            }
        }

        // ===== Helper =====
        private bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? name = null)
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(name ?? string.Empty);
            return true;
        }

        // ===== Properties =====
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
                {
                    SignInCommand.RaiseCanExecuteChanged();
                    if (!string.IsNullOrEmpty(value) && HasError)
                        ClearError();
                }
            }
        }

        private string _password = "";
        public string Password
        {
            get => _password;
            set
            {
                if (SetProperty(ref _password, value))
                {
                    SignInCommand.RaiseCanExecuteChanged();
                    if (!string.IsNullOrEmpty(value) && HasError)
                        ClearError();
                }
            }
        }
                
        private bool _rememberMe;
        public bool RememberMe
        {
            get => _rememberMe;
            set => SetProperty(ref _rememberMe, value);
        }

        private string? _errorMessage;
        public string? ErrorMessage
        {
            get => _errorMessage;
            set
            {
                SetProperty(ref _errorMessage, value);  
                OnPropertyChanged(nameof(HasError));
            }
        }

        public bool HasError => !string.IsNullOrEmpty(ErrorMessage);

        private void ClearError()
        {
            ErrorMessage = null;
        }

        // ===== Login Method =====
        public async Task LoginAsync()
        {
            if (IsBusy) return;

            // Validation
            if (string.IsNullOrWhiteSpace(Username))
            {
                ErrorMessage = "Vui lòng nhập tên đăng nhập.";
                return;
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Vui lòng nhập mật khẩu.";
                return;
            }

            IsBusy = true;
            ErrorMessage = null;

            try
            {
                var token = await _authService.LoginAsync(Username, Password);
                Debug.WriteLine($"Token nhận được: {(token != null ? "Có dữ liệu" : "NULL")}");

                if (!string.IsNullOrWhiteSpace(token))
                {
                    // Lưu Remember Me
                    _tokenStore.SetRememberMe(RememberMe, Username, Password);

                    Debug.WriteLine("Đăng nhập thành công! Token: " + token);
                    _windowsService.ShowMainWindow();
                }
                else
                {
                    ErrorMessage = "Sai tài khoản hoặc mật khẩu.";
                    Debug.WriteLine(ErrorMessage);
                }
            }
            catch (System.Net.Http.HttpRequestException ex)
            {
                if (ex.Message.Contains("401") || ex.Message.Contains("Unauthorized"))
                {
                    ErrorMessage = "Sai tài khoản hoặc mật khẩu.";
                }
                else if (ex.Message.Contains("502") || ex.Message.Contains("503"))
                {
                    ErrorMessage = "Không thể kết nối đến server. Vui lòng kiểm tra cấu hình.";
                }
                else
                {
                    ErrorMessage = "Lỗi kết nối. Vui lòng thử lại sau.";
                }
                Debug.WriteLine($"HTTP Error: {ex}");
            }
            catch (Exception ex)
            {
                ErrorMessage = "Đã xảy ra lỗi. Vui lòng thử lại.";
                Debug.WriteLine($"Lỗi hệ thống: {ex}");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
