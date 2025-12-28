using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop_Frontend.Contracts.Services
{
    public interface IBackendConfig
    {
        Uri GetBaseUri();
        void SetBaseUrl(string baseUrl);
    }
}
