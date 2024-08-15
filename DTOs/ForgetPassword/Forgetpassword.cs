using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tourismApp.DTOs.ForgetPassword
{
    public class ResetPasswordRequest
    {
    public string Token { get; set; }
    public string Email { get; set; }
    public string NewPassword { get; set; }
    }
}