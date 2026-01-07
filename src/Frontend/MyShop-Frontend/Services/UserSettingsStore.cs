using MyShop_Frontend.Contracts.Services;
using MyShop_Frontend.Helpers;
using Windows.Storage;

namespace MyShop_Frontend.Services;

public sealed class UserSettingsStore : IUserSettingsStore
{
    public int GetProductsPageSize(int defaultValue = 10)
    {
        var v = LocalSettingsHelper.GetValue<int?>(AppKeys.ProductsPageSize);
        if (v.HasValue && v.Value > 0) return v.Value;
        return defaultValue;
    }

    public void SetProductsPageSize(int value)
    {
        if (value <= 0) return;
        LocalSettingsHelper.SetValue(AppKeys.ProductsPageSize, value);
    }

    public bool GetRememberLastModule(bool defaultValue = true)
    {
        var v = LocalSettingsHelper.GetValue<bool?>(AppKeys.RememberLastModule);
        if (v.HasValue) return v.Value;
        return defaultValue;
    }

    public void SetRememberLastModule(bool value)
    {
        LocalSettingsHelper.SetValue(AppKeys.RememberLastModule, value);
    }

    public string? GetLastModule() => LocalSettingsHelper.GetValue<string>(AppKeys.LastModule);

    public void SetLastModule(string moduleTag)
    {
        if (string.IsNullOrWhiteSpace(moduleTag)) return;
        LocalSettingsHelper.SetValue(AppKeys.LastModule, moduleTag);
    }
}
