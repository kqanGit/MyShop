using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyShop_Frontend.Contracts.Dtos;

namespace MyShop_Frontend.Contracts.Services
{
    public interface ITokenStore
    {
        string? GetAccessToken();
        void SetAccessToken(string token);
        
        void SetUserInfo(string username, string role);
        string? GetUsername();
        string? GetRole();
        
        // Remember Me
        void SetRememberMe(bool remember, string? username = null, string? password = null);
        bool GetRememberMe();
        string? GetSavedUsername();
        string? GetSavedPassword();
        
        void Clear();
    }
}
