using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tourismApp.Models;

namespace tourismApp.Interface
{
    public interface ITokenService
    {
         string CreateToken (AppUser user);
    }
}