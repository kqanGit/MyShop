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

        private string _searchPhone = "";
        public string SearchPhone
        {
            get => _searchPhone;
            set
            {
                if (_searchPhone != value)
                {
                    _searchPhone = value;
                    OnPropertyChanged(nameof(SearchPhone));
                    PageIndex = 1;
                    _ = LoadCustomersAsync();
                }
            }
        }

        private string _searchName = "";
        public string SearchName
        {
            get => _searchName;
            set
            {
                if (_searchName != value)
                {
                    _searchName = value;
                    OnPropertyChanged(nameof(SearchName));
                    PageIndex = 1;
                    _ = LoadCustomersAsync();
                }
            }
        }

        // Form Properties
        private string _formFullName = "";
        public string FormFullName { get => _formFullName; set { _formFullName = value; OnPropertyChanged(nameof(FormFullName)); } }

        private string _formPhone = "";
        public string FormPhone { get => _formPhone; set { _formPhone = value; OnPropertyChanged(nameof(FormPhone)); } }

        private string _formAddress = "";
        public string FormAddress { get => _formAddress; set { _formAddress = value; OnPropertyChanged(nameof(FormAddress)); } }

        private bool _isEditing;
        private int _editingCustomerId;

        private int _pageIndex = 1;
        public int PageIndex
        {
            get => _pageIndex;
            set { _pageIndex = value; OnPropertyChanged(nameof(PageIndex)); }
        }

        private int _pageSize = 10;
        public int PageSize
        {
            get => _pageSize;
            set { _pageSize = value; OnPropertyChanged(nameof(PageSize)); }
        }

        public int TotalRecords { get; set; }
        public int TotalPages { get; set; }
        
        public string ShowingStatus => $"Showing {Customers.Count} of {TotalRecords} customers (Page {PageIndex} of {TotalPages})";

        public ICommand NextPageCommand { get; }
        public ICommand PreviousPageCommand { get; }
        public ICommand OpenAddDialogCommand { get; }
        public ICommand OpenEditDialogCommand { get; }
        public ICommand SaveCustomerCommand { get; }

        public event EventHandler? RequestOpenDialog;

        public CustomerViewModel(ICustomerService customerService)
        {
            _customerService = customerService;
            NextPageCommand = new RelayCommand(_ => NextPage(), _ => CanNextPage());
            PreviousPageCommand = new RelayCommand(_ => PreviousPage(), _ => CanPreviousPage());

            OpenAddDialogCommand = new RelayCommand(_ => OpenAddDialog());
            OpenEditDialogCommand = new RelayCommand<Customer>(OpenEditDialog);
            SaveCustomerCommand = new RelayCommand(async _ => await SaveCustomerAsync());
            
            _ = LoadCustomersAsync();
        }

        private void OpenAddDialog()
        {
            _isEditing = false;
            FormFullName = "";
            FormPhone = "";
            FormAddress = "";
            RequestOpenDialog?.Invoke(this, EventArgs.Empty);
        }

        private void OpenEditDialog(Customer customer)
        {
            if (customer == null) return;
            _isEditing = true;
            _editingCustomerId = customer.CustomerId;
            FormFullName = customer.FullName;
            FormPhone = customer.Phone;
            FormAddress = customer.Address;
            RequestOpenDialog?.Invoke(this, EventArgs.Empty);
        }

        private async Task SaveCustomerAsync()
        {
            try
            {
                if (_isEditing)
                {
                    var customerToUpdate = new Customer
                    {
                        CustomerId = _editingCustomerId,
                        FullName = FormFullName,
                        Phone = FormPhone,
                        Address = FormAddress,
                        // Maintain existing values if needed, or API handles partial updates? 
                        // DTO from API will map these. Ideally we should perhaps preserve other fields if PUT replaces all.
                        // But for now, simple object creation.
                    };
                    await _customerService.UpdateCustomerAsync(customerToUpdate);
                }
                else
                {
                    var newCustomer = new Customer
                    {
                        FullName = FormFullName,
                        Phone = FormPhone,
                        Address = FormAddress
                    };
                    await _customerService.AddCustomerAsync(newCustomer);
                }
                await LoadCustomersAsync();
                // Close dialog logic? 
                // The ContentDialog in view handles closing on Primary Button Click by default unless Cancel is set.
                // We might want to close it manually if validation fails, but here we assume success.
            }
            catch (Exception ex)
            {
                 System.Diagnostics.Debug.WriteLine($"Error saving customer: {ex.Message}");
            }
        }

        private bool CanPreviousPage() => PageIndex > 1;
        private bool CanNextPage() => PageIndex < TotalPages;

        private void PreviousPage()
        {
            if (CanPreviousPage())
            {
                PageIndex--;
                _ = LoadCustomersAsync();
            }
        }

        private void NextPage()
        {
            if (CanNextPage())
            {
                PageIndex++;
                _ = LoadCustomersAsync();
            }
        }

        private async Task LoadCustomersAsync()
        {
            if (IsLoading) return;

            IsLoading = true;
            try
            {
                var result = await _customerService.GetCustomersAsync(PageIndex, PageSize, SearchPhone, SearchName);
                
                Customers.Clear();
                foreach (var customer in result.Items)
                {
                    Customers.Add(customer);
                }

                TotalRecords = result.TotalRecords;
                TotalPages = result.TotalPages;

                OnPropertyChanged(nameof(TotalRecords));
                OnPropertyChanged(nameof(TotalPages));
                OnPropertyChanged(nameof(ShowingStatus));

                (PreviousPageCommand as RelayCommand)?.RaiseCanExecuteChanged();
                (NextPageCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading customers: {ex.Message}");
                Customers.Clear();
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
