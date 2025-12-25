using MyShop_Frontend.Models;
using MyShop_Frontend.Services;
using MyShop_Frontend.ViewModels.Base;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace MyShop_Frontend.ViewModels.Orders
{
    public class OrderViewModel : ViewModelBase
    {
        private readonly HttpClient _httpClient;
        private bool _isBusy;
        public ObservableCollection<Order> Orders { get; set; } = new ObservableCollection<Order>();

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged(nameof(IsBusy));
            }
        }

        public OrderViewModel()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5126/")
            };
            LoadOrdersAsync();
        }

        public async Task LoadOrdersAsync()
        {
            if (IsBusy) return;
            IsBusy = true;

            try
            {
                // Ensure token is available
                var token = AuthenticationService.CurrentToken;
                if (string.IsNullOrEmpty(token))
                {
                    Debug.WriteLine("No access token available.");
                    return;
                }

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetFromJsonAsync<OrderResponse>("api/Orders?pageIndex=1&pageSize=10");

                if (response != null && response.Items != null)
                {
                    Orders.Clear();
                    foreach (var order in response.Items)
                    {
                        Orders.Add(order);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading orders: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
