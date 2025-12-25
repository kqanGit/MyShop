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

        private List<Product> _allProducts;

        public ProductViewModel()
        {
            _allProducts = Product_MockData.GetProducts();
            Products = new ObservableCollection<Product>(_allProducts);
        }
    }
}
