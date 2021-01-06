using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HaragApp.Controllers.api
{
    public class ChangePasswordViewModel
    {

        public string user_id { get; set; }
        [Required(ErrorMessage = "من فضلك ادخل كلمة المرور القديمة")]
        public string old_password { get; set; }

        [Required(ErrorMessage = "من فضلك ادخل كلمة المرور الجديدة")]
        public string new_password { get; set; }
        public string lang { get; set; } = "ar";
    }
}
