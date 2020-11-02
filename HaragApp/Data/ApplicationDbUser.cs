using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaragApp.Data
{
    public class ApplicationDbUser : IdentityUser
    {


        public bool active_code { get; set; } = false;
        public bool IsActive { get; set; } = false;
        public int code { get; set; } = 1234;
        public int fk_city { get; set; } = 0;
        public int fk_cat { get; set; } = 0;
        public string img { get; set; } //mult part
        public string showpassword { get; set; } //mult part
        public string device_type { get; set; } = "android"; // android or ios
        public string lang { get; set; } = "ar";  //اللغه هتكون عند اليوزر وتكون عربى فى الاول - وتتغير بسيرفس
        public DateTime publish_date { get; set; } = DateTime.Now;

        public string? first_name { get; set; } = "";
        public string? last_name { get; set; } = "";
        public string? region { get; set; }

   

        //تم اضافته لتعامل مع السيرفس اما 
        //UuserName  ده هنساويه بال email
        public string user_name { get; set; } //= first_name + " " + last_name;
        public int type_user { get; set; } //Client = 1  //deleget = 2 //org_delget = 3//Admin = 4

        public bool close_notify { get; set; } = false; //تفعيل الاشعار

        public string lat { get; set; }
        public string lng { get; set; }
        public string address { get; set; }
     
    }
}
