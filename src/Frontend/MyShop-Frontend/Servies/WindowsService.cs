using Microsoft.UI;
using Microsoft.UI.Xaml;
using MyShop_Frontend.Views.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Windowing;
using WinRT.Interop;

namespace MyShop_Frontend.Servies
{
    public sealed class WindowsService
    {
        public AuthenticationWindow? AuthWindow { get; private set; }
        public ServerConfigWindow? SvConfigWindow { get; private set; }

        // Lấy AppWindow từ XAML Window (cách ổn định cho WinUI 3 / Windows App SDK)
        private static AppWindow GetAppWindow(Window window)
        {
            var hwnd = WindowNative.GetWindowHandle(window);
            var windowId = Win32Interop.GetWindowIdFromWindow(hwnd);
            return AppWindow.GetFromWindowId(windowId);
        } // dùng WindowNative + Win32Interop + AppWindow.GetFromWindowId :contentReference[oaicite:2]{index=2}

        public void ShowAuthWindow()
        {
            AuthWindow ??= new AuthenticationWindow();

            // nếu trước đó đã Hide thì Show lại
            GetAppWindow(AuthWindow).Show();
            App.Windows.AuthWindow = AuthWindow;
            AuthWindow.Activate();
        }

        public void ShowServerConfigWindow()
        {
            // đảm bảo AuthWindow đã có để còn Hide
            AuthWindow ??= new AuthenticationWindow();

            if (SvConfigWindow is null)
            {
                SvConfigWindow = new ServerConfigWindow();
                SvConfigWindow.Closed += (_, _) =>
                {
                    SvConfigWindow = null;

                    // khi config đóng -> hiện lại auth
                    GetAppWindow(AuthWindow).Show();
                    AuthWindow.Activate();
                };
            }

            SvConfigWindow.Activate();

            // mở config xong -> ẩn auth
            GetAppWindow(AuthWindow).Hide();
        }

        public void CloseServerConfigIfOpen()
        {
            SvConfigWindow?.Close(); // Closed event sẽ tự Show lại AuthWindow
        }

        public void ShowMainWindow()
        {
            var mainWindow = new MainWindow();
            mainWindow.Activate();

            // Đóng cửa sổ hiện tại (AuthWindow)
            if (App.Windows.AuthWindow != null)
            {
                App.Windows.AuthWindow.Close();

                // 3. Giải phóng bộ nhớ bằng cách gán null sau khi đóng
                App.Windows.AuthWindow = null;
            }
        }
    }


}
