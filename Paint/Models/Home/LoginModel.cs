using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Paint.Models.Home
{
    public class LoginModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string CaptchaCode { get; set; }
        public string LinkLogin { get; set; }
        public string LinkOrig { get; set; }
        public string Error { get; set; }
    }
}
