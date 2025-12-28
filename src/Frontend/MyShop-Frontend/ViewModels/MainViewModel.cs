using Microsoft.Extensions.DependencyInjection;
using MyShop_Frontend.Contracts.Services;
using MyShop_Frontend.Helpers.Command;
using MyShop_Frontend.Services;
using MyShop_Frontend.ViewModels.Base;

namespace MyShop_Frontend.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly WindowsService _windowsService;
        private readonly ITokenStore _tokenStore;

        // Command xử lý đăng xuất
        public RelayCommand SignOutCommand { get; }

        // === Thông tin người dùng ===
        private string _username = "User";
        public string Username
        {
            get => _username;
            set { _username = value; OnPropertyChanged(nameof(Username)); }
        }

        private string _userRole = "Staff";
        public string UserRole
        {
            get => _userRole;
            set { _userRole = value; OnPropertyChanged(nameof(UserRole)); }
        }

        public string RoleDisplayName => UserRole switch
        {
            "1" => "Quản trị viên",
            "2" => "Quản lý",
            "3" => "Nhân viên",
            _ => "Người dùng"
        };

        public MainViewModel()
        {
            _windowsService = App.Windows;
            _tokenStore = App.Services.GetRequiredService<ITokenStore>();

            SignOutCommand = new RelayCommand(_ => ExecuteSignOut());

            // Load thông tin user từ TokenStore
            LoadUserInfo();
        }

        private void LoadUserInfo()
        {
            var username = _tokenStore.GetUsername();
            var role = _tokenStore.GetRole();

            Username = string.IsNullOrEmpty(username) ? "User" : username;
            UserRole = string.IsNullOrEmpty(role) ? "Staff" : role;
            
            OnPropertyChanged(nameof(RoleDisplayName));
        }

        private void ExecuteSignOut()
        {
            _tokenStore.Clear();
            _windowsService.ShowAuthWindow();
        }
    }
}