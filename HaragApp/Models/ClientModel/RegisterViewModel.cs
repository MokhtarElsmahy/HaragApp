using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HaragApp.Controllers.api
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "من فضلك ادخل اسم المستخدم")]
        public string user_name { get; set; }
        //[Required(ErrorMessage = "من فضلك ادخل المدينه")]
        ////public int fk_city { get; set; }

        [Required(ErrorMessage = "من فضلك ادخل رقم الجوال")]
        public string phone { get; set; }

        /// <summary>
        ///Not Required
        /// </summary>

        [EmailAddress(ErrorMessage = " من فضلك ادخل البريد الالكترونى بشكل صحيح")]
        public string email { get; set; }
      

        

            [Required(ErrorMessage = "من فضلك ادخل كلمة المرور")]
        public string password { get; set; }

        [Required]
        public int CityID { get; set; }
        [Required]
        public string lat { get; set; }
        [Required]
        public string lng { get; set; }
        //[Required]
        //public string device_id { get; set; } = "";
        /// <summary>
        ///ar or en
        /// </summary>

        public string lang { get; set; } = "ar";


    }
}
