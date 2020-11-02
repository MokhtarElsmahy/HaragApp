using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaragApp.Controllers.api
{
    public class UpdateDataUserViewModel
    {



        public string user_name { get; set; }
        public string email { get; set; }

        public string phone { get; set; }

        public string address { get; set; }
        public string region { get; set; }
        
        public string lat { get; set; }
        public string lng { get; set; }
        public int fk_city { get; set; }

        public IFormFile Img { get; set; }
        public string device_id { get; set; } = "";
        public string lang { get; set; } = "ar";
    }
}
