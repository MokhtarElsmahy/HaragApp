using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HaragApp.Controllers.api
{
    public class ForgetPasswordViewModel
    {
        [Required(ErrorMessage = "من فضلك ادخل رقم الجوال")]
        public string phone { get; set; }

        public string lang { get; set; } = "ar";
    }
}
