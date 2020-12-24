using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HaragApp.Controllers.api
{
    public class LoginViewModel
    {

        [Required(ErrorMessage = "من فضلك ادخل رقم الجوال")]
        public string phone { get; set; }
        [Required(ErrorMessage = "من فضلك ادخل كلمة المرور")]
        public string password { get; set; }
        //public string lang { get; set; }

        //public string device_id { get; set; }
    }
}
