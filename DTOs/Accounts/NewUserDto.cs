using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tourismApp.DTOs.Accounts
{
    public class NewUserDto
    {
         public string UserName {get; set;}
        public string Email{get; set;}

        public string Token{get; set;}

        public string UserId {get; set;}
    }
}