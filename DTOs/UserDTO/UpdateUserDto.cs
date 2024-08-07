using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tourismApp.DTOs.UserDTO
{
    public class UpdateUserDto
    {
        
        public string Username {get; set;} = String.Empty;

        public string Email {get; set;} = String.Empty;

        public string Password {get; set;} = String.Empty;
    }
}