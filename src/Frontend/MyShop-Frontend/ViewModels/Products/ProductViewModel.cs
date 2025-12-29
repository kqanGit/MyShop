using Microsoft.Extensions.DependencyInjection;
using MyShop_Frontend.Contracts.Services;
using MyShop_Frontend.Helpers.Command;
using MyShop_Frontend.Models;
using MyShop_Frontend.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyShop_Frontend.ViewModels.Products
{
    public class ProductViewModel : ViewModelBase
    {
        private readonly IProductService _productService;
        private List<Product> _allProducts = new();

        public ObservableCollection<Product> Products { get; } = new();

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set { _isLoading = value; OnPropertyChanged(nameof(IsLoading)); }
        }

        private string _searchText = string.Empty;
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged(nameof(SearchText));
                    // Reset to first page and reload
                    PageIndex = 1; 
                    _ = LoadProductsAsync();
                }
            }
        }

        private double _maxPriceFilter = 1000000000;
        public double MaxPriceFilter
        {
            get => _maxPriceFilter;
            set
            {
                // Debounce or only load on drag end usually better, but for now simple check
                if (Math.Abs(_maxPriceFilter - value) > 1000) 
                {
                    _maxPriceFilter = value;
                    OnPropertyChanged(nameof(MaxPriceFilter));
                    PageIndex = 1;
                    _ = LoadProductsAsync();
                }
            }
        }

        public ObservableCollection<string> Categories { get; } = new() { "All Categories" };

        private string _selectedCategory = "All Categories";
        public string SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                if (_selectedCategory != value)
                {
                    _selectedCategory = value;
                    OnPropertyChanged(nameof(SelectedCategory));
                    PageIndex = 1;
                    _ = LoadProductsAsync();
                }
            }
        }

        public ObservableCollection<string> SortOptions { get; } = new() { "None", "Price", "Name" };

        private string _selectedSortOption = "None";
        public string SelectedSortOption
        {
            get => _selectedSortOption;
            set
            {
                if (_selectedSortOption != value)
                {
                    _selectedSortOption = value;
                    OnPropertyChanged(nameof(SelectedSortOption));
                    PageIndex = 1;
                    _ = LoadProductsAsync();
                }
            }
        }

        private bool _isAscending = true;
        public bool IsAscending
        {
            get => _isAscending;
            set
            {
                if (_isAscending != value)
                {
                    _isAscending = value;
                    OnPropertyChanged(nameof(IsAscending));
                    OnPropertyChanged(nameof(SortOrderText));
                    PageIndex = 1;
                    _ = LoadProductsAsync();
                }
            }
        }

        public string SortOrderText => IsAscending ? "Ascending" : "Descending";

        public ICommand ToggleSortOrderCommand { get; }
        public ICommand LoadProductsCommand { get; }
        public ICommand AddProductCommand { get; }
        public ICommand UpdateProductCommand { get; }
        public ICommand DeleteProductCommand { get; }
        public ICommand NextPageCommand { get; }
        public ICommand PreviousPageCommand { get; }

        public int TotalRecords { get; set; }
        public string ShowingStatus => $"Showing {Products.Count} of {TotalRecords} products (Page {PageIndex} of {TotalPages})";

        public int TotalPages => TotalRecords == 0 ? 0 : (int)Math.Ceiling((double)TotalRecords / PageSize);

        private int _pageIndex = 1;
        public int PageIndex
        {
            get => _pageIndex;
            set { if (_pageIndex != value) { _pageIndex = value; OnPropertyChanged(nameof(PageIndex)); OnPropertyChanged(nameof(ShowingStatus)); } }
        }

        private int _pageSize = 5;
        public int PageSize
        {
            get => _pageSize;
            set { if (_pageSize != value) { _pageSize = value; OnPropertyChanged(nameof(PageSize)); OnPropertyChanged(nameof(ShowingStatus)); } }
        }

        public ProductViewModel()
        {
            _productService = App.Services.GetRequiredService<IProductService>();

            ToggleSortOrderCommand = new RelayCommand(_ => IsAscending = !IsAscending);
            LoadProductsCommand = new RelayCommand(async _ => await LoadProductsAsync());
            AddProductCommand = new RelayCommand(async _ => await AddProductAsync());
            UpdateProductCommand = new RelayCommand<Product>(async p => await UpdateProductAsync(p));
            DeleteProductCommand = new RelayCommand<Product>(async p => await DeleteProductAsync(p));
            NextPageCommand = new RelayCommand(async _ => { PageIndex++; await LoadProductsAsync(); }, _ => PageIndex < TotalPages);
            PreviousPageCommand = new RelayCommand(async _ => { PageIndex--; await LoadProductsAsync(); }, _ => PageIndex > 1);

            // Load data on initialization
            _ = LoadProductsAsync();
        }

        private async Task LoadProductsAsync()
        {
            if (IsLoading) return;

            IsLoading = true;
            try
            {
                var sortOrder = IsAscending ? "asc" : "desc";
                var category = SelectedCategory == "All Categories" ? null : SelectedCategory;
                var sort = SelectedSortOption == "None" ? null : SelectedSortOption;

                // Pass filters to Service
                var result = await _productService.GetProductsAsync(
                    PageIndex, PageSize, 
                    SearchText, 
                    category, 
                    MaxPriceFilter, 
                    sort, 
                    sortOrder
                );

                _allProducts = result.Items.ToList();
                
                System.Diagnostics.Debug.WriteLine($"Loaded {_allProducts.Count} products. Total: {result.TotalRecords}");
                TotalRecords = result.TotalRecords;
                OnPropertyChanged(nameof(TotalRecords));
                OnPropertyChanged(nameof(TotalPages));
                OnPropertyChanged(nameof(ShowingStatus));
                
                (PreviousPageCommand as RelayCommand)?.RaiseCanExecuteChanged();
                (NextPageCommand as RelayCommand)?.RaiseCanExecuteChanged();

                // Note: Categories should ideally come from a separate API. 
                // Dynamically updating them from current page might lose "other" categories not on this page.
                // For now, we accumulate or just keep "All Categories" + current page ones.
                // Better approach: Don't clear if we already have them, OR use a separate LoadCategories().
                // To be safe and show at least what we have:
                foreach (var cat in _allProducts.Select(p => p.CategoryName).Distinct().OrderBy(c => c))
                {
                     if (!Categories.Contains(cat)) {
                         Categories.Add(cat ?? "Unknown");
                     }
                }
                
                // Do NOT re-calculate MaxPriceFilter here as it overrides user input.
                
                // Populate ObservableCollection directly (No more client-side FilterProducts)
                Products.Clear();
                foreach (var product in _allProducts)
                {
                    Products.Add(product);
                }
                OnPropertyChanged(nameof(ShowingStatus));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading products: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task AddProductAsync()
        {
             // TODO: Implement Add Logic (Dialog)
             await LoadProductsAsync();
        }

        private async Task UpdateProductAsync(Product? product)
        {
             if (product == null) return;
             // TODO: Implement Edit Logic
             await LoadProductsAsync();
        }

        private async Task DeleteProductAsync(Product? product)
        {
             if (product == null) return;
             
             try {
                await _productService.DeleteProductAsync(product.ProductId);
                await LoadProductsAsync();
             } catch (Exception ex) {
                 System.Diagnostics.Debug.WriteLine($"Delete failed: {ex}");
             }
        }


    }
}
