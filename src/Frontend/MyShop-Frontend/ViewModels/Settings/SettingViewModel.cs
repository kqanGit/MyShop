using Microsoft.Extensions.DependencyInjection;
using MyShop_Frontend.Contracts.Services;
using MyShop_Frontend.ViewModels.Base;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using MyShop_Frontend.Contracts.Dtos;

namespace MyShop_Frontend.ViewModels.Settings
{
    public class SettingViewModel : ViewModelBase
    {
        private readonly IBackendConfig _backendConfig;
        private readonly IUserSettingsStore _userSettings;
        private readonly IUserConfigService _userConfigService;

        private int _itemsPerPage;
        private bool _rememberLastModule;
        private bool _autoRefreshDashboard;
        private string _selectedLanguage;
        private string _apiServerUrl;
        private int _connectionTimeout;
        private string _selectedTheme;
        private string _selectedFontSize;
        private bool _enableAnimations;
        private bool _lowStockAlerts;
        private bool _orderNotifications;
        private bool _notificationSound;
        private bool _autoSaveData;
        private bool _isLoading;
        private string _error;

        public int[] ItemsPerPageOptions { get; } = { 5, 10, 20, 50 };

        public SettingViewModel(IBackendConfig backendConfig)
        {
            _backendConfig = backendConfig;
            _userSettings = App.Services.GetRequiredService<IUserSettingsStore>();
            _userConfigService = App.Services.GetRequiredService<IUserConfigService>();

            // Initialize with default values
            ItemsPerPage = 20;
            RememberLastModule = true;
            AutoRefreshDashboard = false;
            SelectedLanguage = "English (US)";
            ApiServerUrl = _backendConfig.GetBaseUri()?.ToString() ?? "http://localhost:5000";
            ConnectionTimeout = 30;
            SelectedTheme = "Light";
            SelectedFontSize = "Medium";
            EnableAnimations = true;
            LowStockAlerts = true;
            OrderNotifications = true;
            NotificationSound = false;
            AutoSaveData = false;

            // Initialize commands
            SaveChangesCommand = new Helpers.Command.RelayCommand(async (param) => await SaveChangesAsync());
            ResetToDefaultCommand = new Helpers.Command.RelayCommand((param) => ResetToDefault());

            // Load saved settings
            _ = LoadSettingsAsync();
        }

        #region Properties

        public int ItemsPerPage
        {
            get => _itemsPerPage;
            set
            {
                _itemsPerPage = value;
                OnPropertyChanged(nameof(ItemsPerPage));
            }
        }

        public bool RememberLastModule
        {
            get => _rememberLastModule;
            set
            {
                _rememberLastModule = value;
                OnPropertyChanged(nameof(RememberLastModule));
            }
        }

        public bool AutoRefreshDashboard
        {
            get => _autoRefreshDashboard;
            set
            {
                _autoRefreshDashboard = value;
                OnPropertyChanged(nameof(AutoRefreshDashboard));
            }
        }

        public string SelectedLanguage
        {
            get => _selectedLanguage;
            set
            {
                _selectedLanguage = value;
                OnPropertyChanged(nameof(SelectedLanguage));
            }
        }

        public string ApiServerUrl
        {
            get => _apiServerUrl;
            set
            {
                _apiServerUrl = value;
                OnPropertyChanged(nameof(ApiServerUrl));
            }
        }

        public int ConnectionTimeout
        {
            get => _connectionTimeout;
            set
            {
                _connectionTimeout = value;
                OnPropertyChanged(nameof(ConnectionTimeout));
            }
        }

        public string SelectedTheme
        {
            get => _selectedTheme;
            set
            {
                _selectedTheme = value;
                OnPropertyChanged(nameof(SelectedTheme));
            }
        }

        public string SelectedFontSize
        {
            get => _selectedFontSize;
            set
            {
                _selectedFontSize = value;
                OnPropertyChanged(nameof(SelectedFontSize));
            }
        }

        public bool EnableAnimations
        {
            get => _enableAnimations;
            set
            {
                _enableAnimations = value;
                OnPropertyChanged(nameof(EnableAnimations));
            }
        }

        public bool LowStockAlerts
        {
            get => _lowStockAlerts;
            set
            {
                _lowStockAlerts = value;
                OnPropertyChanged(nameof(LowStockAlerts));
            }
        }

        public bool OrderNotifications
        {
            get => _orderNotifications;
            set
            {
                _orderNotifications = value;
                OnPropertyChanged(nameof(OrderNotifications));
            }
        }

        public bool NotificationSound
        {
            get => _notificationSound;
            set
            {
                _notificationSound = value;
                OnPropertyChanged(nameof(NotificationSound));
            }
        }

        public bool AutoSaveData
        {
            get => _autoSaveData;
            set
            {
                _autoSaveData = value;
                OnPropertyChanged(nameof(AutoSaveData));
            }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }

        public string Error
        {
            get => _error;
            set
            {
                _error = value;
                OnPropertyChanged(nameof(Error));
            }
        }

        #endregion

        #region Commands

        public ICommand SaveChangesCommand { get; }
        public ICommand ResetToDefaultCommand { get; }

        #endregion

        #region Methods

        private async Task LoadSettingsAsync()
        {
            try
            {
                ItemsPerPage = _userSettings.GetProductsPageSize(defaultValue: 10);
                RememberLastModule = _userSettings.GetRememberLastModule(defaultValue: true);

                ApiServerUrl = _backendConfig.GetBaseUri()?.ToString() ?? "http://localhost:5126/";

                var remote = await _userConfigService.GetConfigAsync();
                if (remote != null && ItemsPerPageOptions.Contains(remote.PerPage))
                {
                    ItemsPerPage = remote.PerPage;
                }
            }
            catch (Exception ex)
            {
                Error = $"Failed to load settings: {ex.Message}";
            }
        }

        private async Task SaveChangesAsync()
        {
            try
            {
                IsLoading = true;
                Error = null;

                if (!ItemsPerPageOptions.Contains(ItemsPerPage))
                {
                    ItemsPerPage = 10;
                }

                _userSettings.SetProductsPageSize(ItemsPerPage);
                _userSettings.SetRememberLastModule(RememberLastModule);

                var lastModule = _userSettings.GetLastModule() ?? "Dashboard";
                await _userConfigService.SaveConfigAsync(new UserConfigClientDto
                {
                    PerPage = ItemsPerPage,
                    LastModule = lastModule
                });

                if (_backendConfig.GetBaseUri()?.ToString() != ApiServerUrl)
                {
                    _backendConfig.SetBaseUrl(ApiServerUrl);
                }

                await Task.Delay(50);
            }
            catch (Exception ex)
            {
                Error = $"Failed to save settings: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void ResetToDefault()
        {
            ItemsPerPage = 10;
            RememberLastModule = true;
            AutoRefreshDashboard = false;

            _userSettings.SetProductsPageSize(ItemsPerPage);
            _userSettings.SetRememberLastModule(RememberLastModule);

            _ = _userConfigService.SaveConfigAsync(new UserConfigClientDto
            {
                PerPage = ItemsPerPage,
                LastModule = _userSettings.GetLastModule() ?? "Dashboard"
            });
        }

        #endregion
    }
}
