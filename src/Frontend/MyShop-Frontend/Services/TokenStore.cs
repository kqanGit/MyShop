using MyShop_Frontend.Contracts.Services;
using MyShop_Frontend.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace MyShop_Frontend.Services
{
    public sealed class TokenStore : ITokenStore
    {
        private readonly ApplicationDataContainer _settings = ApplicationData.Current.LocalSettings;

        public string? GetAccessToken() => _settings.Values[AppKeys.AccessToken] as string;

        public void SetAccessToken(string token) => _settings.Values[AppKeys.AccessToken] = token;

        public void Clear() => _settings.Values.Remove(AppKeys.AccessToken);
    }
}