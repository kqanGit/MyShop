namespace MyShop_Frontend.Contracts.Services;

public interface IUserSettingsStore
{
    int GetProductsPageSize(int defaultValue = 10);
    void SetProductsPageSize(int value);

    bool GetRememberLastModule(bool defaultValue = true);
    void SetRememberLastModule(bool value);

    string? GetLastModule();
    void SetLastModule(string moduleTag);
}
