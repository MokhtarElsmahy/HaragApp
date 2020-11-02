using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HaragApp.Controllers.api
{

    public class ConfirmCodeViewModel
    {
        [Required(ErrorMessage = "من فضلك ادخل كود التحقق")]
        public int code { get; set; }
        [Required]
        public string user_id { get; set; }
        public string lang { get; set; } = "ar";


    }

}
