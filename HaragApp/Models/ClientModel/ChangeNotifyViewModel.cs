﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaragApp.Controllers.api
{

    public class ChangeNotifyViewModel
    {

        public bool notify { get; set; }
        public string lang { get; set; } = "ar";
        public string device_id { get; set; } = "";
    }
}
