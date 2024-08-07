using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tourismApp.DTOs.LoginDto
{
    public class LoginDto
    {
         public string Username {get; set;} = String.Empty;

        public string Password {get; set;} = String.Empty;
    }
}