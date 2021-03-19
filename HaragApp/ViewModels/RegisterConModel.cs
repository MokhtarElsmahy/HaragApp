using HaragApp.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaragApp.ViewModels
{
    public class RegisterConModel
    {
        public string UserID { get; set; }
        //public string code { get; set; }
        public string Phone { get; set; }
        public bool IsCreated { get; set; }
    }
}
