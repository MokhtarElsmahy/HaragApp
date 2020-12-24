using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HaragApp.ViewModels
{
    public class ConfirmPhoneModel
    {
        public string PhoneNumber { get; set; }

        [Required, Display(Name = "Code")]
        public string VerificationCode { get; set; }
    }
}
