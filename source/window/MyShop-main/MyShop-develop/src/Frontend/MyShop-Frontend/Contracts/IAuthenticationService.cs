using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MyShop_Frontend.Contracts.Dtos.AuthDto;

namespace MyShop_Frontend.Contracts
{
    public interface IAuthenticationService
    {
        // Trả về Token nếu thành công, null nếu thất bại
        Task<string?> LoginAsync(string username, string password);
    }
}
