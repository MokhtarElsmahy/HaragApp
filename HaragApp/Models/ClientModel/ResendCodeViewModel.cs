﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaragApp.Controllers.api
{

    public class ResendCodeViewModel
    {

        public string user_id { get; set; }
        public string lang { get; set; } = "ar";
    }
}
