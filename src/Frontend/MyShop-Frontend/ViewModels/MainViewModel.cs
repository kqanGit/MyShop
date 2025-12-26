using MyShop_Frontend.ViewModels.Base;
using MyShop_Frontend.Helpers.Command;
using System.Threading.Tasks;
using MyShop_Frontend.Services;

namespace MyShop_Frontend.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private WindowsService _windowsService = new WindowsService(); 

        // Command xử lý đăng xuất
        public RelayCommand SignOutCommand { get; }

        public MainViewModel()
        {
            SignOutCommand = new RelayCommand(_ => ExecuteSignOut());
        }

        private void ExecuteSignOut()
        {
            // 1. Logic xóa Token (nếu cần)
            // 2. Gọi Service để hiện màn hình Login
            _windowsService.ShowAuthWindow();
            // Lưu ý: Việc đóng MainWindow sẽ được xử lý ở bước 3
        }
    }
}