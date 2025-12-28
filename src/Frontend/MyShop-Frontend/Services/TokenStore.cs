using MyShop_Frontend.Contracts.Services;
using MyShop_Frontend.Helpers;
using Windows.Storage;

namespace MyShop_Frontend.Services
{
    public sealed class TokenStore : ITokenStore
    {
        private readonly ApplicationDataContainer _settings = ApplicationData.Current.LocalSettings;

        // === Token & User Info ===
        public string? GetAccessToken() => _settings.Values[AppKeys.AccessToken] as string;
        public void SetAccessToken(string token) => _settings.Values[AppKeys.AccessToken] = token;

        public void SetUserInfo(string username, string role)
        {
            _settings.Values[AppKeys.Username] = username;
            _settings.Values[AppKeys.UserRole] = role;
        }

        public string? GetUsername() => _settings.Values[AppKeys.Username] as string;
        public string? GetRole() => _settings.Values[AppKeys.UserRole] as string;

        // === Remember Me ===
        public void SetRememberMe(bool remember, string? username = null, string? password = null)
        {
            _settings.Values[AppKeys.RememberMe] = remember;
            
            if (remember && !string.IsNullOrEmpty(username))
            {
                _settings.Values[AppKeys.SavedUsername] = username;
                // Lưu ý: Trong production nên mã hóa password
                _settings.Values[AppKeys.SavedPassword] = password;
            }
            else if (!remember)
            {
                _settings.Values.Remove(AppKeys.SavedUsername);
                _settings.Values.Remove(AppKeys.SavedPassword);
            }
        }

        public bool GetRememberMe()
        {
            var value = _settings.Values[AppKeys.RememberMe];
            return value is bool b && b;
        }

        public string? GetSavedUsername() => _settings.Values[AppKeys.SavedUsername] as string;
        public string? GetSavedPassword() => _settings.Values[AppKeys.SavedPassword] as string;

        // === Clear ===
        public void Clear()
        {
            _settings.Values.Remove(AppKeys.AccessToken);
            _settings.Values.Remove(AppKeys.Username);
            _settings.Values.Remove(AppKeys.UserRole);
            // Không xóa Remember Me khi logout
        }
    }
}