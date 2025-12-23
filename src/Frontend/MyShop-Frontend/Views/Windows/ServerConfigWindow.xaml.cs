using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MyShop_Frontend.Views.Windows
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ServerConfigWindow : Window
    {
        public ServerConfigWindow()
        {
            this.InitializeComponent();
            Title = "MyShop - Server Config";
        }

        private void TestConnection_Click(object sender, RoutedEventArgs e)
        {
            StatusText.Text = "Connection successful";
        }

        private void ResetDefault_Click(object sender, RoutedEventArgs e)
        {
            ServerAddressBox.Text = "192.168.1.15";
            PortBox.Text = "8";
            DnsBox.Text = "8.8.8.8";
            SaveAsDefaultCheckBox.IsChecked = false;

            StatusText.Text = "Reset to default";
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            StatusText.Text = "Saved";
        }
        private void BackToLogin_Click(object sender, RoutedEventArgs e)
        {
            App.Windows.ShowAuthWindow();
            this.Close();
        }
    }

}
