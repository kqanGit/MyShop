using Microsoft.Extensions.DependencyInjection;
using MyShop_Frontend.Contracts.Dtos;
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
        private readonly IMembershipService _membershipService;

        public ObservableCollection<Customer> Customers { get; } = new();
        public ObservableCollection<Membership> Tiers { get; } = new();

        private Membership _selectedTier;
        public Membership SelectedTier
        {
            get => _selectedTier;
            set
            {
                if (_selectedTier != value)
                {
                    _selectedTier = value;
                    OnPropertyChanged(nameof(SelectedTier));
                    PageIndex = 1;
                    _ = LoadCustomersAsync();
                }
            }
        }

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
        public ICommand DeleteCustomerCommand { get; }

        public event EventHandler? RequestOpenDialog;

        public CustomerViewModel(ICustomerService customerService, IMembershipService membershipService)
        {
            _customerService = customerService;
            _membershipService = membershipService;
            NextPageCommand = new RelayCommand(_ => NextPage(), _ => CanNextPage());
            PreviousPageCommand = new RelayCommand(_ => PreviousPage(), _ => CanPreviousPage());

            OpenAddDialogCommand = new RelayCommand(_ => OpenAddDialog());
            OpenEditDialogCommand = new RelayCommand<Customer>(OpenEditDialog);
            DeleteCustomerCommand = new RelayCommand<Customer>(async c => await DeleteCustomerAsync(c));
            SaveCustomerCommand = new RelayCommand(async _ => await SaveCustomerAsync());

            _ = LoadTiersAsync();
            _ = LoadCustomersAsync();
        }

        private async Task LoadTiersAsync()
        {
            try
            {
                var tiers = await _membershipService.GetMembershipsAsync();
                Tiers.Clear();
                Tiers.Add(new Membership { TierId = 0, TierName = "All" });
                foreach (var tier in tiers) Tiers.Add(tier);
                SelectedTier = Tiers.FirstOrDefault();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading tiers: {ex.Message}");
            }
        }

        private async Task DeleteCustomerAsync(Customer customer)
        {
            if (customer == null) return;
            var dialog = new Microsoft.UI.Xaml.Controls.ContentDialog
            {
                Title = "Delete Customer",
                Content = $"Are you sure you want to delete {customer.FullName}?",
                PrimaryButtonText = "Delete",
                CloseButtonText = "Cancel",
                DefaultButton = Microsoft.UI.Xaml.Controls.ContentDialogButton.Primary,
                XamlRoot = App.MainWindow.Content.XamlRoot
            };

            var result = await dialog.ShowAsync();
            if (result == Microsoft.UI.Xaml.Controls.ContentDialogResult.Primary)
            {
                try
                {
                    await _customerService.DeleteCustomerAsync(customer.CustomerId);
                    await LoadCustomersAsync();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error deleting customer: {ex.Message}");
                }
            }
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
                    var request = new UpdateCustomerDto
                    {
                        FullName = FormFullName,
                        Phone = FormPhone,
                        Address = FormAddress
                    };
                    await _customerService.UpdateCustomerAsync(_editingCustomerId, request);
                }
                else
                {
                    var request = new CreateCustomerDto
                    {
                        FullName = FormFullName,
                        Phone = FormPhone,
                        Address = FormAddress
                    };
                    await _customerService.CreateCustomerAsync(request);
                }
                await LoadCustomersAsync();
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
                var tierId = SelectedTier != null && SelectedTier.TierId > 0 ? (int?)SelectedTier.TierId : null;

                PagedResult<CustomerDto> result;
                if (string.IsNullOrWhiteSpace(SearchPhone) && string.IsNullOrWhiteSpace(SearchName) && !tierId.HasValue)
                {
                    result = await _customerService.GetCustomersAsync(PageIndex, PageSize);
                }
                else
                {
                    result = await _customerService.SearchCustomersAsync(PageIndex, PageSize, SearchPhone, SearchName, tierId);
                }

                Customers.Clear();
                foreach (var customer in result.Items)
                {
                    Customers.Add(new Customer
                    {
                        CustomerId = customer.CustomerId,
                        FullName = customer.FullName,
                        Phone = customer.Phone,
                        Address = customer.Address,
                        Point = customer.Point,
                        TierName = customer.TierName
                    });
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