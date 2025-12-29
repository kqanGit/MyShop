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

namespace MyShop_Frontend.Services
{
    public sealed class WindowsService
    {
        public AuthenticationWindow? AuthWindow { get; private set; }
        public ServerConfigWindow? SvConfigWindow { get; private set; }

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
            AuthWindow ??= new AuthenticationWindow();

            if (SvConfigWindow is null)
            {
                SvConfigWindow = new ServerConfigWindow();
                SvConfigWindow.Closed += (_, _) =>
                {
                    SvConfigWindow = null;

                    GetAppWindow(AuthWindow).Show();
                    AuthWindow.Activate();
                };
            }

            SvConfigWindow.Activate();

            GetAppWindow(AuthWindow).Hide();
        }

        public void CloseServerConfigIfOpen()
        {
            SvConfigWindow?.Close();
        }

        public void ShowMainWindow()
        {
            var mainWindow = new MainWindow();
            mainWindow.Activate();

            if (App.Windows.AuthWindow != null)
            {
                App.Windows.AuthWindow.Close();

                App.Windows.AuthWindow = null;
            }
        }
    }


}
