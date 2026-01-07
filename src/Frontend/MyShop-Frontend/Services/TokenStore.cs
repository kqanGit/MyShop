using MyShop_Frontend.Contracts.Services;
using MyShop_Frontend.Helpers;
using Windows.Storage;

namespace MyShop_Frontend.Services
{
    public sealed class TokenStore : ITokenStore
    {
        // === Token & User Info ===
        public string? GetAccessToken() => LocalSettingsHelper.GetValue<string>(AppKeys.AccessToken);
        public void SetAccessToken(string token) => LocalSettingsHelper.SetValue(AppKeys.AccessToken, token);

        public void SetUserInfo(string username, string role)
        {
            LocalSettingsHelper.SetValue(AppKeys.Username, username);
            LocalSettingsHelper.SetValue(AppKeys.UserRole, role);
        }

        public string? GetUsername() => LocalSettingsHelper.GetValue<string>(AppKeys.Username);
        public string? GetRole() => LocalSettingsHelper.GetValue<string>(AppKeys.UserRole);

        // === Remember Me ===
        public void SetRememberMe(bool remember, string? username = null, string? password = null)
        {
            LocalSettingsHelper.SetValue(AppKeys.RememberMe, remember);
            
            if (remember && !string.IsNullOrEmpty(username))
            {
                LocalSettingsHelper.SetValue(AppKeys.SavedUsername, username);
                // Lưu ý: Trong production nên mã hóa password
                LocalSettingsHelper.SetValue(AppKeys.SavedPassword, password);
            }
            else if (!remember)
            {
                LocalSettingsHelper.Remove(AppKeys.SavedUsername);
                LocalSettingsHelper.Remove(AppKeys.SavedPassword);
            }
        }

        public bool GetRememberMe()
        {
            var value = LocalSettingsHelper.GetValue<bool>(AppKeys.RememberMe);
            return value;
        }

        public string? GetSavedUsername() => LocalSettingsHelper.GetValue<string>(AppKeys.SavedUsername);
        public string? GetSavedPassword() => LocalSettingsHelper.GetValue<string>(AppKeys.SavedPassword);

        // === Clear ===
        public void Clear()
        {
            LocalSettingsHelper.Remove(AppKeys.AccessToken);
            LocalSettingsHelper.Remove(AppKeys.Username);
            LocalSettingsHelper.Remove(AppKeys.UserRole);
            // Không xóa Remember Me khi logout
        }
    }
}