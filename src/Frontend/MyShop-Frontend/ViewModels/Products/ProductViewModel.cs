using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using MyShop_Frontend.Contracts.Services;
using MyShop_Frontend.Helpers.Command;
using MyShop_Frontend.Models;
using MyShop_Frontend.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage.Pickers;
using Windows.Storage;
using WinRT.Interop;

namespace MyShop_Frontend.ViewModels.Products
{
    public class ProductViewModel : ViewModelBase
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly MyShop_Frontend.Contracts.Services.IUserSettingsStore _userSettings;
        private readonly ITokenStore _tokenStore;
        private readonly bool _canManageProducts;
        private List<Product> _allProducts = new();
        private Dictionary<int, string> _categoryIdToName = new();

        public ObservableCollection<Product> Products { get; } = new();
        public ObservableCollection<Category> Categories { get; } = new();

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
    
        private double _minPriceFilter = 0;
        public double MinPriceFilter
        {
            get => _minPriceFilter;
            set
            {
                if (Math.Abs(_minPriceFilter - value) < 0.01) return;
                _minPriceFilter = value;
                OnPropertyChanged(nameof(MinPriceFilter));
                FilterProducts();
            }
        }

        private string _keyword = string.Empty;
        public string Keyword
        {
            get => _keyword;
            set
            {
                if (_keyword == value) return;
                _keyword = value;
                OnPropertyChanged(nameof(Keyword));
                PageIndex = 1;
                _ = LoadProductsAsync();
            }
        }

        private string _sort = "name_asc";
        public string Sort
        {
            get => _sort;
            set
            {
                if (_sort == value) return;
                _sort = value;
                OnPropertyChanged(nameof(Sort));
                PageIndex = 1;
                _ = LoadProductsAsync();
            }
        }

        private Category? _selectedCategory;
        public Category? SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                if (_selectedCategory == value) return;
                _selectedCategory = value;
                OnPropertyChanged(nameof(SelectedCategory));
                PageIndex = 1;
                _ = LoadProductsAsync();
            }
        }

        public ICommand ViewProductCommand { get; }
        public ICommand LoadProductsCommand { get; }
        public ICommand AddProductCommand { get; }
        public ICommand UpdateProductCommand { get; }
        public ICommand DeleteProductCommand { get; }
        public ICommand NextPageCommand { get; }
        public ICommand PreviousPageCommand { get; }
        public ICommand AddCategoryCommand { get; }
        public ICommand ImportCommand { get; }
        public ICommand ExportCommand { get; }

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
            private set
            {
                if (_pageSize == value) return;
                _pageSize = value;
                _userSettings.SetProductsPageSize(value);
                PageIndex = 1;
                OnPropertyChanged(nameof(PageSize));
                OnPropertyChanged(nameof(TotalPages));
                OnPropertyChanged(nameof(ShowingStatus));
                _ = LoadProductsAsync();
            }
        }

        public ProductViewModel()
        {
            _productService = App.Services.GetRequiredService<IProductService>();
            _categoryService = App.Services.GetRequiredService<ICategoryService>();
            _userSettings = App.Services.GetRequiredService<MyShop_Frontend.Contracts.Services.IUserSettingsStore>();
            _tokenStore = App.Services.GetRequiredService<ITokenStore>();

            var role = _tokenStore.GetRole()?.ToLowerInvariant();
            _canManageProducts = role == "1" || role == "admin" || role == "2" || role == "manager";

            PageSize = _userSettings.GetProductsPageSize(defaultValue: 10);

            ViewProductCommand = new RelayCommand<Product>(async p => await ViewProductAsync(p));
            LoadProductsCommand = new RelayCommand(async _ => await LoadProductsAsync());
            AddProductCommand = new RelayCommand(async _ => await AddProductAsync(), _ => _canManageProducts);
            UpdateProductCommand = new RelayCommand<Product>(async p => await UpdateProductAsync(p), _ => _canManageProducts);
            DeleteProductCommand = new RelayCommand<Product>(async p => await DeleteProductAsync(p), _ => _canManageProducts);
            NextPageCommand = new RelayCommand(async _ => { PageIndex++; await LoadProductsAsync(); }, _ => PageIndex < TotalPages);
            PreviousPageCommand = new RelayCommand(async _ => { PageIndex--; await LoadProductsAsync(); }, _ => PageIndex > 1);

            AddCategoryCommand = new RelayCommand(async _ => await AddCategoryAsync(), _ => _canManageProducts);
            ImportCommand = new RelayCommand(async _ => await ImportAsync(), _ => _canManageProducts);
            ExportCommand = new RelayCommand(async _ => await ExportAsync(), _ => _canManageProducts);

            _ = LoadCategoriesAsync();
            _ = LoadProductsAsync();
        }

        private async Task LoadCategoriesAsync()
        {
            try
            {
                var cats = await _categoryService.GetCategoriesAsync();
                Categories.Clear();
                Categories.Add(new Category { CategoryId = 0, CategoryName = "All" });
                
                _categoryIdToName.Clear();
                foreach (var c in cats.OrderBy(c => c.CategoryName))
                {
                    Categories.Add(c);
                    _categoryIdToName[c.CategoryId] = c.CategoryName ?? $"Category {c.CategoryId}";
                }

                SelectedCategory ??= Categories.FirstOrDefault();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"LoadCategoriesAsync failed: {ex}");
            }
        }

        private async Task LoadProductsAsync()
        {
            if (IsLoading) return;

            IsLoading = true;
            try
            {
                int? categoryId = SelectedCategory is null || SelectedCategory.CategoryId == 0
                    ? null
                    : SelectedCategory.CategoryId;

                var result = await _productService.GetProductsAsync(
                    keyword: string.IsNullOrWhiteSpace(Keyword) ? null : Keyword,
                    categoryId: categoryId,
                    minPrice: (decimal?)MinPriceFilter,
                    maxPrice: (decimal?)MaxPriceFilter,
                    sort: Sort,
                    pageIndex: PageIndex,
                    pageSize: PageSize);

                _allProducts = result.Items.Select(p =>
                {
                    if (string.IsNullOrWhiteSpace(p.CategoryName) && _categoryIdToName.TryGetValue(p.CategoryId, out var name))
                    {
                        p.CategoryName = name;
                    }
                    return p;
                }).ToList();

                TotalRecords = result.TotalRecords;
                OnPropertyChanged(nameof(TotalRecords));
                OnPropertyChanged(nameof(TotalPages));
                OnPropertyChanged(nameof(ShowingStatus));

                (PreviousPageCommand as RelayCommand)?.RaiseCanExecuteChanged();
                (NextPageCommand as RelayCommand)?.RaiseCanExecuteChanged();

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
            var title = new TextBlock { Text = "Product Information", FontWeight = Microsoft.UI.Text.FontWeights.SemiBold, Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Black) };

            var nameBox = new TextBox { PlaceholderText = "Enter product name..." };
            
            var categoryBox = new ComboBox 
            { 
                HorizontalAlignment = HorizontalAlignment.Stretch,
                DisplayMemberPath = "CategoryName",
                SelectedValuePath = "CategoryId"
            };
            foreach (var cat in Categories.Where(c => c.CategoryId > 0))
            {
                categoryBox.Items.Add(cat);
            }
            if (categoryBox.Items.Count > 0)
                categoryBox.SelectedIndex = 0;

            var unitBox = new TextBox { PlaceholderText = "Piece / Box / Bottle..." };
            var costBox = new NumberBox { Minimum = 0, Value = 0, SpinButtonPlacementMode = NumberBoxSpinButtonPlacementMode.Inline };
            var priceBox = new NumberBox { Minimum = 0, Value = 0, SpinButtonPlacementMode = NumberBoxSpinButtonPlacementMode.Inline };
            var stockBox = new NumberBox { Minimum = 0, Value = 0, SpinButtonPlacementMode = NumberBoxSpinButtonPlacementMode.Inline };
            var imageBox = new TextBox { PlaceholderText = "https://... or file path" };

            var panel = new StackPanel { Spacing = 12, Padding = new Thickness(2) };
            panel.Children.Add(title);

            panel.Children.Add(new TextBlock { Text = "Product name", FontSize = 12, Opacity = 0.6 });
            panel.Children.Add(nameBox);

            var row1 = new Grid { ColumnSpacing = 12 };
            row1.ColumnDefinitions.Add(new ColumnDefinition());
            row1.ColumnDefinitions.Add(new ColumnDefinition());
            var s1 = new StackPanel { Spacing = 6 };
            s1.Children.Add(new TextBlock { Text = "Category", FontSize = 12, Opacity = 0.6 });
            s1.Children.Add(categoryBox);
            var s2 = new StackPanel { Spacing = 6 };
            s2.Children.Add(new TextBlock { Text = "Unit", FontSize = 12, Opacity = 0.6 });
            s2.Children.Add(unitBox);
            Grid.SetColumn(s2, 1);
            row1.Children.Add(s1);
            row1.Children.Add(s2);
            panel.Children.Add(row1);

            var row2 = new Grid { ColumnSpacing = 12 };
            row2.ColumnDefinitions.Add(new ColumnDefinition());
            row2.ColumnDefinitions.Add(new ColumnDefinition());
            var s3 = new StackPanel { Spacing = 6 };
            s3.Children.Add(new TextBlock { Text = "Cost", FontSize = 12, Opacity = 0.6 });
            s3.Children.Add(costBox);
            var s4 = new StackPanel { Spacing = 6 };
            s4.Children.Add(new TextBlock { Text = "Price", FontSize = 12, Opacity = 0.6 });
            s4.Children.Add(priceBox);
            Grid.SetColumn(s4, 1);
            row2.Children.Add(s3);
            row2.Children.Add(s4);
            panel.Children.Add(row2);

            panel.Children.Add(new TextBlock { Text = "Stock", FontSize = 12, Opacity = 0.6 });
            panel.Children.Add(stockBox);

            panel.Children.Add(new TextBlock { Text = "Image (url/path)", FontSize = 12, Opacity = 0.6 });
            panel.Children.Add(imageBox);

            var content = new ScrollViewer { MaxHeight = 560, Content = panel };

            var dlg = new ContentDialog
            {
                XamlRoot = App.MainWindow?.Content is FrameworkElement fe ? fe.XamlRoot : null,
                Title = "New Product",
                Content = content,
                PrimaryButtonText = "Create",
                SecondaryButtonText = "Cancel",
                DefaultButton = ContentDialogButton.Primary
            };

            dlg.PrimaryButtonClick += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(nameBox.Text))
                {
                    e.Cancel = true;
                    dlg.Title = "New Product - Product name is required";
                }
                else if (categoryBox.SelectedItem == null)
                {
                    e.Cancel = true;
                    dlg.Title = "New Product - Please select a category";
                }
            };

            var res = await dlg.ShowAsync();
            if (res != ContentDialogResult.Primary)
                return;

            try
            {
                var selectedCat = categoryBox.SelectedItem as Category;
                var product = new Product
                {
                    ProductName = (nameBox.Text ?? string.Empty).Trim(),
                    CategoryId = selectedCat?.CategoryId ?? 1,
                    Unit = (unitBox.Text ?? string.Empty).Trim(),
                    Cost = (decimal)(costBox.Value < 0 ? 0 : costBox.Value),
                    Price = (decimal)(priceBox.Value < 0 ? 0 : priceBox.Value),
                    Stock = (int)(stockBox.Value < 0 ? 0 : stockBox.Value),
                    Image = (imageBox.Text ?? string.Empty).Trim(),
                    IsRemoved = false
                };

                await _productService.AddProductAsync(product);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Add product failed: {ex}");
            }

            await LoadProductsAsync();
        }

        private async Task UpdateProductAsync(Product? product)
        {
            if (product == null) return;

            var nameBox = new TextBox { Text = product.ProductName };
            
            var categoryBox = new ComboBox 
            { 
                HorizontalAlignment = HorizontalAlignment.Stretch,
                DisplayMemberPath = "CategoryName",
                SelectedValuePath = "CategoryId"
            };
            
            Category? selectedCategory = null;
            foreach (var cat in Categories.Where(c => c.CategoryId > 0))
            {
                categoryBox.Items.Add(cat);
                if (cat.CategoryId == product.CategoryId)
                    selectedCategory = cat;
            }
            if (selectedCategory != null)
                categoryBox.SelectedItem = selectedCategory;
            else if (categoryBox.Items.Count > 0)
                categoryBox.SelectedIndex = 0;

            var unitBox = new TextBox { Text = product.Unit };
            var costBox = new NumberBox { Minimum = 0, Value = (double)product.Cost, SpinButtonPlacementMode = NumberBoxSpinButtonPlacementMode.Inline };
            var priceBox = new NumberBox { Minimum = 0, Value = (double)product.Price, SpinButtonPlacementMode = NumberBoxSpinButtonPlacementMode.Inline };
            var stockBox = new NumberBox { Minimum = 0, Value = product.Stock, SpinButtonPlacementMode = NumberBoxSpinButtonPlacementMode.Inline };
            var imageBox = new TextBox { Text = product.Image };

            var panel = new StackPanel { Spacing = 12, Padding = new Thickness(2) };
            panel.Children.Add(new TextBlock { Text = "Product Information", FontWeight = Microsoft.UI.Text.FontWeights.SemiBold });

            panel.Children.Add(new TextBlock { Text = "Product name", FontSize = 12, Opacity = 0.6 });
            panel.Children.Add(nameBox);

            var row1 = new Grid { ColumnSpacing = 12 };
            row1.ColumnDefinitions.Add(new ColumnDefinition());
            row1.ColumnDefinitions.Add(new ColumnDefinition());
            var s1 = new StackPanel { Spacing = 6 };
            s1.Children.Add(new TextBlock { Text = "Category", FontSize = 12, Opacity = 0.6 });
            s1.Children.Add(categoryBox);
            var s2 = new StackPanel { Spacing = 6 };
            s2.Children.Add(new TextBlock { Text = "Unit", FontSize = 12, Opacity = 0.6 });
            s2.Children.Add(unitBox);
            Grid.SetColumn(s2, 1);
            row1.Children.Add(s1);
            row1.Children.Add(s2);
            panel.Children.Add(row1);

            var row2 = new Grid { ColumnSpacing = 12 };
            row2.ColumnDefinitions.Add(new ColumnDefinition());
            row2.ColumnDefinitions.Add(new ColumnDefinition());
            var s3 = new StackPanel { Spacing = 6 };
            s3.Children.Add(new TextBlock { Text = "Cost", FontSize = 12, Opacity = 0.6 });
            s3.Children.Add(costBox);
            var s4 = new StackPanel { Spacing = 6 };
            s4.Children.Add(new TextBlock { Text = "Price", FontSize = 12, Opacity = 0.6 });
            s4.Children.Add(priceBox);
            Grid.SetColumn(s4, 1);
            row2.Children.Add(s3);
            row2.Children.Add(s4);
            panel.Children.Add(row2);

            panel.Children.Add(new TextBlock { Text = "Stock", FontSize = 12, Opacity = 0.6 });
            panel.Children.Add(stockBox);

            panel.Children.Add(new TextBlock { Text = "Image (url/path)", FontSize = 12, Opacity = 0.6 });
            panel.Children.Add(imageBox);

            var content = new ScrollViewer { MaxHeight = 560, Content = panel };

            var dlg = new ContentDialog
            {
                XamlRoot = App.MainWindow?.Content is FrameworkElement fe ? fe.XamlRoot : null,
                Title = "Update Product",
                Content = content,
                PrimaryButtonText = "Save",
                SecondaryButtonText = "Cancel",
                DefaultButton = ContentDialogButton.Primary
            };

            dlg.PrimaryButtonClick += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(nameBox.Text))
                {
                    e.Cancel = true;
                    dlg.Title = "Update Product - Product name is required";
                }
                else if (categoryBox.SelectedItem == null)
                {
                    e.Cancel = true;
                    dlg.Title = "Update Product - Please select a category";
                }
            };

            var res = await dlg.ShowAsync();
            if (res != ContentDialogResult.Primary)
                return;

            try
            {
                var selectedCat = categoryBox.SelectedItem as Category;
                var updated = new Product
                {
                    ProductId = product.ProductId,
                    ProductName = (nameBox.Text ?? string.Empty).Trim(),
                    CategoryId = selectedCat?.CategoryId ?? 1,
                    Unit = (unitBox.Text ?? string.Empty).Trim(),
                    Cost = (decimal)(costBox.Value < 0 ? 0 : costBox.Value),
                    Price = (decimal)(priceBox.Value < 0 ? 0 : priceBox.Value),
                    Stock = (int)(stockBox.Value < 0 ? 0 : stockBox.Value),
                    Image = (imageBox.Text ?? string.Empty).Trim(),
                    IsRemoved = product.IsRemoved
                };

                await _productService.UpdateProductAsync(updated);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Update product failed: {ex}");
            }

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

        private async Task AddCategoryAsync()
        {
            var nameBox = new TextBox { PlaceholderText = "Category name..." };

            var dlg = new ContentDialog
            {
                XamlRoot = App.MainWindow?.Content is FrameworkElement fe ? fe.XamlRoot : null,
                Title = "New Category",
                Content = nameBox,
                PrimaryButtonText = "Create",
                SecondaryButtonText = "Cancel",
                DefaultButton = ContentDialogButton.Primary
            };

            dlg.PrimaryButtonClick += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(nameBox.Text))
                {
                    e.Cancel = true;
                    dlg.Title = "New Category - Name is required";
                }
            };

            var res = await dlg.ShowAsync();
            if (res != ContentDialogResult.Primary) return;

            var info = new ContentDialog
            {
                XamlRoot = dlg.XamlRoot,
                Title = "Not Implemented",
                Content = "POST /api/Categories is not available in the provided API list.",
                CloseButtonText = "OK"
            };
            await info.ShowAsync();
        }

        private async Task ImportAsync()
        {
            var picker = new FileOpenPicker();
            picker.ViewMode = PickerViewMode.List;
            picker.SuggestedStartLocation = PickerLocationId.ComputerFolder;
            picker.FileTypeFilter.Add(".csv");

            // Helper to initialize picker with window handle
            var windowHandle = WindowNative.GetWindowHandle(App.MainWindow);
            InitializeWithWindow.Initialize(picker, windowHandle);

            var file = await picker.PickSingleFileAsync();
            if (file == null) return;

            IsLoading = true;
            try
            {
                var lines = await FileIO.ReadLinesAsync(file);
                // Assume Header: ProductName,CategoryId,Unit,Cost,Price,Stock,Image
                // Skip header
                var dataLines = lines.Skip(1);
                
                int count = 0;
                foreach (var line in dataLines)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    var parts = line.Split(',');
                    if (parts.Length < 6) continue; // Basic validation

                    // Simple CSV parsing (assuming no commas in values for now)
                    /* 
                       0: Name
                       1: CategoryId
                       2: Unit
                       3: Cost
                       4: Price
                       5: Stock
                       6: Image (Optional)
                    */

                    try
                    {
                        var product = new Product
                        {
                            ProductName = parts[0].Trim(),
                            CategoryId = int.TryParse(parts[1], out var cid) ? cid : 1, // Default to 1 if invalid
                            Unit = parts[2].Trim(),
                            Cost = decimal.TryParse(parts[3], out var c) ? c : 0,
                            Price = decimal.TryParse(parts[4], out var p) ? p : 0,
                            Stock = int.TryParse(parts[5], out var s) ? s : 0,
                            Image = parts.Length > 6 ? parts[6].Trim() : string.Empty,
                            IsRemoved = false
                        };

                        await _productService.AddProductAsync(product);
                        count++;
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Failed to import line: {line}. Error: {ex.Message}");
                    }
                }

                await LoadProductsAsync();

                var dlg = new ContentDialog
                {
                    XamlRoot = App.MainWindow?.Content is FrameworkElement fe ? fe.XamlRoot : null,
                    Title = "Import Successful",
                    Content = $"Imported {count} products successfully.",
                    CloseButtonText = "OK",
                    DefaultButton = ContentDialogButton.Close
                };
                await dlg.ShowAsync();
            }
            catch (Exception ex)
            {
                var errorDlg = new ContentDialog
                {
                    XamlRoot = App.MainWindow?.Content is FrameworkElement fe ? fe.XamlRoot : null,
                    Title = "Import Failed",
                    Content = $"An error occurred: {ex.Message}",
                    CloseButtonText = "Close",
                    DefaultButton = ContentDialogButton.Close
                };
                await errorDlg.ShowAsync();
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task ExportAsync()
        {
            var picker = new FileSavePicker();
            picker.SuggestedStartLocation = PickerLocationId.ComputerFolder;
            picker.FileTypeChoices.Add("CSV File", new List<string>() { ".csv" });
            picker.SuggestedFileName = "Products_Export";

            var windowHandle = WindowNative.GetWindowHandle(App.MainWindow);
            InitializeWithWindow.Initialize(picker, windowHandle);

            var file = await picker.PickSaveFileAsync();
            if (file == null) return;

            IsLoading = true;
            try
            {
                // Fetch all products (using a large page size to get all)
                var result = await _productService.GetProductsAsync(pageSize: 100000);
                var products = result.Items;

                var sb = new System.Text.StringBuilder();
                // Header (matching Import expectation mostly: ProductName,CategoryId,Unit,Cost,Price,Stock,Image)
                sb.AppendLine("ProductName,CategoryId,Unit,Cost,Price,Stock,Image");

                foreach (var p in products)
                {
                    // Basic escaping for CSV: if contains comma, wrap in quotes. 
                    // For now, simpler approach or just quote only name if needed.
                    // Doing robust CSV writing manually:
                    
                    string Escape(string s)
                    {
                        if (string.IsNullOrEmpty(s)) return "";
                        if (s.Contains(",") || s.Contains("\"") || s.Contains("\n"))
                        {
                            return $"\"{s.Replace("\"", "\"\"")}\"";
                        }
                        return s;
                    }

                    var line = string.Join(",",
                        Escape(p.ProductName),
                        p.CategoryId,
                        Escape(p.Unit),
                        p.Cost,
                        p.Price,
                        p.Stock,
                        Escape(p.Image)
                    );
                    sb.AppendLine(line);
                }

                await FileIO.WriteTextAsync(file, sb.ToString());

                var dlg = new ContentDialog
                {
                    XamlRoot = App.MainWindow?.Content is FrameworkElement fe ? fe.XamlRoot : null,
                    Title = "Export Successful",
                    Content = $"Exported {products.Count} products successfully to {file.Name}.",
                    CloseButtonText = "OK",
                    DefaultButton = ContentDialogButton.Close
                };
                await dlg.ShowAsync();
            }
            catch (Exception ex)
            {
                var errorDlg = new ContentDialog
                {
                    XamlRoot = App.MainWindow?.Content is FrameworkElement fe ? fe.XamlRoot : null,
                    Title = "Export Failed",
                    Content = $"An error occurred: {ex.Message}",
                    CloseButtonText = "Close",
                    DefaultButton = ContentDialogButton.Close
                };
                await errorDlg.ShowAsync();
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task ViewProductAsync(Product? product)
        {
            if (product == null) return;

            // Prepare images
            var images = new List<ProductImage>();
            if (product.Images != null && product.Images.Any())
            {
                images.AddRange(product.Images);
            }
            else if (!string.IsNullOrEmpty(product.Image))
            {
                images.Add(new ProductImage { ImageUrl = product.Image, IsPrimary = true });
            }

            // Create UI
            var flipView = new FlipView { Height = 400, Width = 600 };
            flipView.ItemsSource = images;
            flipView.ItemTemplate = (DataTemplate)Microsoft.UI.Xaml.Markup.XamlReader.Load(
                @"<DataTemplate xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'>
                    <Image Source='{Binding ImageUrl}' Stretch='Uniform' HorizontalAlignment='Center' VerticalAlignment='Center'/>
                  </DataTemplate>");

            var dlg = new ContentDialog
            {
                XamlRoot = App.MainWindow?.Content is FrameworkElement fe ? fe.XamlRoot : null,
                Title = product.ProductName,
                Content = flipView,
                CloseButtonText = "Close",
                DefaultButton = ContentDialogButton.Close
            };

            await dlg.ShowAsync();
        }

        private void FilterProducts()
        {
            // Normalize price range
            if (MinPriceFilter > MaxPriceFilter)
            {
                MaxPriceFilter = MinPriceFilter;
            }

            var filtered = _allProducts.AsEnumerable();

            // Apply local keyword filter (defensive in case server-side filter not applied)
            if (!string.IsNullOrWhiteSpace(Keyword))
            {
                filtered = filtered.Where(p => (p.ProductName ?? string.Empty)
                    .Contains(Keyword, StringComparison.OrdinalIgnoreCase));
            }

            // Apply local category filter (defensive)
            if (SelectedCategory is { CategoryId: > 0 })
            {
                filtered = filtered.Where(p => p.CategoryId == SelectedCategory.CategoryId);
            }

            // Price range filter
            filtered = filtered.Where(p => (double)p.Price <= MaxPriceFilter && (double)p.Price >= MinPriceFilter);

            // Apply sort locally to reflect UI sorting
            filtered = Sort switch
            {
                "name_desc" => filtered.OrderByDescending(p => p.ProductName),
                "price_asc" => filtered.OrderBy(p => p.Price),
                "price_desc" => filtered.OrderByDescending(p => p.Price),
                "stock_asc" => filtered.OrderBy(p => p.Stock),
                "stock_desc" => filtered.OrderByDescending(p => p.Stock),
                _ => filtered.OrderBy(p => p.ProductName)
            };

            Products.Clear();
            foreach (var product in filtered)
            {
                Products.Add(product);
            }
            OnPropertyChanged(nameof(ShowingStatus));
        }
    }
}
