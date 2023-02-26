using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WyzeFtpLibrary.Models
{

    public class WyzeCredentials
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
    //Todo: use proper naming convention and confirm compatiblity with api call
    public class PushoverCredentials
    {
        public string token { get; set; }
        public string user { get; set; }
    }

}
