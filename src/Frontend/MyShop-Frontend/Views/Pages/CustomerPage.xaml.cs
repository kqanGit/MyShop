using Microsoft.UI.Xaml.Controls;
using MyShop_Frontend.ViewModels.Customers;
using System;

namespace MyShop_Frontend.Views.Pages
{
    public sealed partial class CustomerPage : Page
    {
        public CustomerPage()
        {
            this.InitializeComponent();
            ViewModel = App.GetService<CustomerViewModel>();
            DataContext = ViewModel;
            ViewModel.RequestOpenDialog += async (s, e) => 
            {
                CustomerDialog.XamlRoot = this.XamlRoot; 
                await CustomerDialog.ShowAsync();
            };
        }

        public CustomerViewModel ViewModel { get; }
    }
}
