using MyShop_Frontend.Contracts.Services;
using MyShop_Frontend.Helpers;
using Windows.Storage;

namespace MyShop_Frontend.Services;

public sealed class UserSettingsStore : IUserSettingsStore
{
    private readonly ApplicationDataContainer _settings = ApplicationData.Current.LocalSettings;

    public int GetProductsPageSize(int defaultValue = 10)
    {
        var v = _settings.Values[AppKeys.ProductsPageSize];
        if (v is int i && i > 0) return i;
        return defaultValue;
    }

    public void SetProductsPageSize(int value)
    {
        if (value <= 0) return;
        _settings.Values[AppKeys.ProductsPageSize] = value;
    }

    public bool GetRememberLastModule(bool defaultValue = true)
    {
        var v = _settings.Values[AppKeys.RememberLastModule];
        if (v is bool b) return b;
        return defaultValue;
    }

    public void SetRememberLastModule(bool value)
    {
        _settings.Values[AppKeys.RememberLastModule] = value;
    }

    public string? GetLastModule() => _settings.Values[AppKeys.LastModule] as string;

    public void SetLastModule(string moduleTag)
    {
        if (string.IsNullOrWhiteSpace(moduleTag)) return;
        _settings.Values[AppKeys.LastModule] = moduleTag;
    }
}
