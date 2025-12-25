using MyShop_Frontend.Helpers;
using MyShop_Frontend.Models;
using MyShop_Frontend.ViewModels.Base;
using System.Collections.ObjectModel;

namespace MyShop_Frontend.ViewModels.Customers
{
    public class CustomerViewModel : ViewModelBase
    {
        public ObservableCollection<Customer> Customers { get; set; }

        public CustomerViewModel()
        {
            Customers = new ObservableCollection<Customer>(MockData.GetCustomers());
        }
    }
}
