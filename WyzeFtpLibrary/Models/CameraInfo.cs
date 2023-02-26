using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Todo: use proper naming convention and confirm compatiblity with api call

namespace WyzeFtpLibrary.Models
{
    public class CameraInfo
    {
        public Data data { get; set; }
    }

    public class Data
    {
        public string mac { get; set; }
        public string nickname { get; set; }
        public string ip { get; set; }

    }

}
