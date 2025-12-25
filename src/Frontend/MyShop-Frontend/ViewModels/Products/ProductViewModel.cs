using MyShop_Frontend.Models;
using MyShop_Frontend.ViewModels.Base;
using MyShop_Frontend.Helpers.MockData;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System;

namespace MyShop_Frontend.ViewModels.Products
{
    public class ProductViewModel : ViewModelBase
    {
        private ObservableCollection<Product> _products;
        public ObservableCollection<Product> Products
        {
            get => _products;
            set { _products = value; OnPropertyChanged(nameof(Products)); }
        }

        private string _searchText;
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

        private ObservableCollection<string> _categories;
        public ObservableCollection<string> Categories
        {
            get => _categories;
            set { _categories = value; OnPropertyChanged(nameof(Categories)); }
        }

        private string _selectedCategory;
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

        private List<Product> _allProducts;

        public ProductViewModel()
        {
            _allProducts = Product_MockData.GetProducts();
            Products = new ObservableCollection<Product>(_allProducts);
            
            // Populate Categories
            var categories = _allProducts.Select(p => p.CategoryName).Distinct().OrderBy(c => c).ToList();
            categories.Insert(0, "All Categories");
            Categories = new ObservableCollection<string>(categories);
            SelectedCategory = "All Categories";

            // Optional: Set initial MaxPriceFilter based on data
            if (_allProducts.Any())
            {
                _maxPriceFilter = (double)_allProducts.Max(p => p.Price);
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

            Products.Clear();
            foreach (var product in filtered)
            {
                Products.Add(product);
            }
        }
    }
}
