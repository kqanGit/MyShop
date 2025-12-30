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

        private double _maxPriceFilter = 1000000000;
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
                var result = await _productService.GetProductsAsync(PageIndex, PageSize);
                _allProducts = result.Items.ToList();
                System.Diagnostics.Debug.WriteLine($"Loaded {_allProducts.Count} products.");
                TotalRecords = result.TotalRecords;
                OnPropertyChanged(nameof(TotalRecords));
                OnPropertyChanged(nameof(TotalPages));
                OnPropertyChanged(nameof(ShowingStatus));
                
                (PreviousPageCommand as RelayCommand)?.RaiseCanExecuteChanged();
                (NextPageCommand as RelayCommand)?.RaiseCanExecuteChanged();



                // Set max price
                // Set max price only once or if current max is less (to avoid resetting user filter on paging)
                // Actually, if we paginate, we might not know global max price. 
                // For now, let's just keep it simple but handle empty list.
                if (_allProducts.Any())
                {
                    // Only update max price if it wasn't set or new products have higher price?
                    // Better to just leave it or set a fixed high value if backend doesn't provide it.
                    // Or set it based on current page max (which is temporary).
                    // Example: MaxPriceFilter = (double)_allProducts.Max(p => p.Price);
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

        private void FilterProducts()
        {
            var filtered = _allProducts.AsEnumerable();



            filtered = filtered.Where(p => (double)p.Price <= MaxPriceFilter);

            // Client-side sorting removed in favor of Server-side

            Products.Clear();
            foreach (var product in filtered)
            {
                Products.Add(product);
            }
            OnPropertyChanged(nameof(ShowingStatus));
        }
    }
}
