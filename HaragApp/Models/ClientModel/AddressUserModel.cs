using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaragApp.Controllers.api
{
    public class AddressUserModel
    {

        public string title { get; set; }
        public string address { get; set; }
        public string lat { get; set; }
        public string lng { get; set; }
        public string lang { get; set; } = "ar";

        public string info { get; set; }
        public bool is_used { get; set; }
    }
}
