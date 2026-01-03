using Microsoft.Extensions.DependencyInjection;
using MyShop_Frontend.Contracts.Services;
using MyShop_Frontend.Helpers.Command;
using MyShop_Frontend.Models;
using MyShop_Frontend.ViewModels.Base;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyShop_Frontend.ViewModels.Customers
{
    public class CustomerViewModel : ViewModelBase
    {
        private readonly ICustomerService _customerService;
        
        public ObservableCollection<Customer> Customers { get; } = new();

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set { _isLoading = value; OnPropertyChanged(nameof(IsLoading)); }
        }

        private string _searchText = "";
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged(nameof(SearchText));
                    PageIndex = 1; // Reset to first page on search
                    _ = LoadCustomersAsync();
                }
            }
        }

        private int _pageIndex = 1;
        public int PageIndex
        {
            get => _pageIndex;
            set 
            { 
                if (_pageIndex != value) 
                { 
                    _pageIndex = value; 
                    OnPropertyChanged(nameof(PageIndex)); 
                    OnPropertyChanged(nameof(ShowingStatus)); 
                } 
            }
        }

        private int _pageSize = 10;
        public int PageSize
        {
            get => _pageSize;
            set
            {
                if (_pageSize == value) return;
                _pageSize = value;
                PageIndex = 1;
                OnPropertyChanged(nameof(PageSize));
                OnPropertyChanged(nameof(TotalPages));
                OnPropertyChanged(nameof(ShowingStatus));
                _ = LoadCustomersAsync();
            }
        }

        public int TotalRecords { get; set; }
        public int TotalPages => TotalRecords == 0 ? 0 : (int)Math.Ceiling((double)TotalRecords / PageSize);
        
        public string ShowingStatus => $"Showing {Customers.Count} of {TotalRecords} customers (Page {PageIndex} of {TotalPages})";

        public ICommand LoadCustomersCommand { get; }
        public ICommand NextPageCommand { get; }
        public ICommand PreviousPageCommand { get; }

        public CustomerViewModel()
        {
            _customerService = App.Services.GetRequiredService<ICustomerService>();

            LoadCustomersCommand = new RelayCommand(async _ => await LoadCustomersAsync());
            
            NextPageCommand = new RelayCommand(async _ => 
            { 
                PageIndex++; 
                await LoadCustomersAsync(); 
            }, _ => PageIndex < TotalPages && !IsLoading);

            PreviousPageCommand = new RelayCommand(async _ => 
            { 
                PageIndex--; 
                await LoadCustomersAsync(); 
            }, _ => PageIndex > 1 && !IsLoading);

            // Load data on initialization
            _ = LoadCustomersAsync();
        }

        private async Task LoadCustomersAsync()
        {
            if (IsLoading) return;

            IsLoading = true;
            (NextPageCommand as RelayCommand)?.RaiseCanExecuteChanged();
            (PreviousPageCommand as RelayCommand)?.RaiseCanExecuteChanged();

            try
            {
                var result = await _customerService.GetCustomersAsync(PageIndex, PageSize, SearchText);
                
                Customers.Clear();
                foreach (var customer in result.Items)
                {
                    Customers.Add(customer);
                }

                TotalRecords = result.TotalRecords;
                OnPropertyChanged(nameof(TotalRecords));
                OnPropertyChanged(nameof(TotalPages));
                OnPropertyChanged(nameof(ShowingStatus));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading customers: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
                (NextPageCommand as RelayCommand)?.RaiseCanExecuteChanged();
                (PreviousPageCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }
    }
}
