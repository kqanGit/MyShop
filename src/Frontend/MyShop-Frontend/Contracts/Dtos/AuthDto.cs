using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop_Frontend.Contracts.Dtos
{
    public class AuthDto
    {
        public class LoginRequest
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

       
    }
}
