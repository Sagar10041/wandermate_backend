using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tourismApp.Emailsender{

 public class SmtpSettings
{
    public string Host { get; set; }
    public int Port { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public bool EnableSsl { get; set; }
}
}