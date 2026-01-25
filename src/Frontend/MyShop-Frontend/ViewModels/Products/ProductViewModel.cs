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
                    FilterProducts();
                }
            }
        }

        private double _maxPriceFilter = 100;
        public double MaxPriceFilter
        {
            get => _maxPriceFilter;
            set
            {
                if (Math.Abs(_maxPriceFilter - value) > 0.01)
                {
                    _maxPriceFilter = value;
                    OnPropertyChanged(nameof(MaxPriceFilter));
                    FilterProducts();
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
                    FilterProducts();
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
                    FilterProducts();
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
                    FilterProducts();
                }
            }
        }

        public string SortOrderText => IsAscending ? "Ascending" : "Descending";

        public ICommand ToggleSortOrderCommand { get; }
        public ICommand LoadProductsCommand { get; }
    
        public ProductViewModel()
        {
            _productService = App.Services.GetRequiredService<IProductService>();

            ToggleSortOrderCommand = new RelayCommand(_ => IsAscending = !IsAscending);
            LoadProductsCommand = new RelayCommand(async _ => await LoadProductsAsync());

            // Load data on initialization
            _ = LoadProductsAsync();
        }

        private async Task LoadProductsAsync()
        {
            if (IsLoading) return;

            IsLoading = true;
            try
            {
                var products = await _productService.GetProductsAsync();
                _allProducts = products.ToList();

                // Update categories
                Categories.Clear();
                Categories.Add("All Categories");
                foreach (var cat in _allProducts.Select(p => p.CategoryName).Distinct().OrderBy(c => c))
                {
                    Categories.Add(cat ?? "Unknown");
                }

                // Set max price
                if (_allProducts.Any())
                {
                    MaxPriceFilter = (double)_allProducts.Max(p => p.Price);
                }

                FilterProducts();
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

        private void FilterProducts()
        {
            var filtered = _allProducts.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                filtered = filtered.Where(p =>
                    p.ProductName?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ?? false);
            }

            if (!string.IsNullOrEmpty(SelectedCategory) && SelectedCategory != "All Categories")
            {
                filtered = filtered.Where(p => p.CategoryName == SelectedCategory);
            }

            filtered = filtered.Where(p => (double)p.Price <= MaxPriceFilter);

            filtered = SelectedSortOption switch
            {
                "Price" => IsAscending ? filtered.OrderBy(p => p.Price) : filtered.OrderByDescending(p => p.Price),
                "Name" => IsAscending ? filtered.OrderBy(p => p.ProductName) : filtered.OrderByDescending(p => p.ProductName),
                _ => filtered
            };

            Products.Clear();
            foreach (var product in filtered)
            {
                Products.Add(product);
            }
        }
    }
}
