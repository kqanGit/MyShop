using MyShop_Frontend.Helpers.MockData;
using MyShop_Frontend.Models;
using MyShop_Frontend.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MyShop_Frontend.ViewModels.Customers
{
    public class CustomerViewModel : ViewModelBase
    {
        private List<Customer> _allCustomers;
        private string _searchText;

        public ObservableCollection<Customer> Customers { get; set; }

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged(nameof(SearchText));
                    FilterCustomers();
                }
            }
        }

        public CustomerViewModel()
        {
            _allCustomers = Customer_MockData.GetCustomers();
            Customers = new ObservableCollection<Customer>(_allCustomers);
        }

        private void FilterCustomers()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                Customers.Clear();
                foreach (var customer in _allCustomers)
                {
                    Customers.Add(customer);
                }
            }
            else
            {
                var filtered = _allCustomers.Where(c => 
                    (c.FullName?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (c.Phone?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ?? false)).ToList();

                Customers.Clear();
                foreach (var customer in filtered)
                {
                    Customers.Add(customer);
                }
            }
        }
    }
}
