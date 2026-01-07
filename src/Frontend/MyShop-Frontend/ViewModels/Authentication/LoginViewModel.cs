using Microsoft.Extensions.DependencyInjection;
using MyShop_Frontend.Contracts;
using MyShop_Frontend.Contracts.Dtos;
using MyShop_Frontend.Contracts.Services;
using MyShop_Frontend.Helpers.Command;
using MyShop_Frontend.Services;
using MyShop_Frontend.ViewModels.Base;
using System;
using System.Diagnostics;
using System.Net.Http;
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
                AuthResponseDto? result = await _authService.LoginAsync(Username, Password);
                Debug.WriteLine($"Token nhận được: {(result != null ? "Có dữ liệu" : "NULL")}");

                if (result != null && !string.IsNullOrWhiteSpace(result.Token))
                {
                    // Lưu Remember Me
                    _tokenStore.SetRememberMe(RememberMe, Username, Password);

                    Debug.WriteLine("Đăng nhập thành công! Token: " + result.Token);
                    _windowsService.ShowMainWindow();
                }
                else
                {
                    ErrorMessage = "Sai tài khoản hoặc mật khẩu.";
                }
            }
            catch (HttpRequestException ex)
            {
                // AuthenticationService đã include: "Login failed: <code> <reason>\n<body>"
                var msg = ex.Message ?? "";

                if (msg.Contains("401") || msg.Contains("Unauthorized", StringComparison.OrdinalIgnoreCase))
                {
                    ErrorMessage = "Sai tài khoản hoặc mật khẩu.";
                }
                else if (msg.Contains("404") || msg.Contains("Not Found", StringComparison.OrdinalIgnoreCase))
                {
                    ErrorMessage = "Không tìm thấy API đăng nhập. Hãy kiểm tra lại Server Config (base URL) và route /api/auth/login.";
                }
                else if (msg.Contains("SSL", StringComparison.OrdinalIgnoreCase) || msg.Contains("certificate", StringComparison.OrdinalIgnoreCase))
                {
                    ErrorMessage = "Lỗi chứng chỉ SSL/HTTPS. Nếu dùng localhost hãy thử http://localhost:<port>/ trong Server Config.";
                }
                else if (msg.Contains("No such host", StringComparison.OrdinalIgnoreCase) ||
                         msg.Contains("Name or service not known", StringComparison.OrdinalIgnoreCase) ||
                         msg.Contains("host is unknown", StringComparison.OrdinalIgnoreCase))
                {
                    ErrorMessage = "Không phân giải được địa chỉ server. Hãy kiểm tra lại host/IP trong Server Config.";
                }
                else if (msg.Contains("refused", StringComparison.OrdinalIgnoreCase) ||
                         msg.Contains("actively refused", StringComparison.OrdinalIgnoreCase))
                {
                    ErrorMessage = "Không kết nối được server (connection refused). Kiểm tra backend đang chạy và đúng port.";
                }
                else if (msg.Contains("timeout", StringComparison.OrdinalIgnoreCase))
                {
                    ErrorMessage = "Kết nối bị timeout. Kiểm tra mạng hoặc server đang chậm.";
                }
                else if (msg.Contains("502") || msg.Contains("503"))
                {
                    ErrorMessage = "Server đang lỗi (502/503). Vui lòng thử lại sau hoặc kiểm tra backend.";
                }
                else
                {
                    // Hiển thị message gốc để debug chính xác (vì đã có response body)
                    ErrorMessage = msg;
                }

                Debug.WriteLine($"HTTP Error: {ex}");
            }
            catch (InvalidOperationException ex)
            {
                // Các lỗi parse JSON, thiếu token ...
                ErrorMessage = ex.Message;
                Debug.WriteLine($"Invalid Operation: {ex}");
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
