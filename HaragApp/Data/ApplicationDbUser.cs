using HaragApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HaragApp.Data
{
    public class ApplicationDbUser : IdentityUser
    {


        public bool active_code { get; set; } = false;
        public bool IsActive { get; set; } = false;

        public int code { get; set; } = 1234;
       
        public string lang { get; set; } = "ar";  //اللغه هتكون عند اليوزر وتكون عربى فى الاول - وتتغير بسيرفس
        public DateTime publish_date { get; set; } = DateTime.Now;

        public string fullName { get; set; } = "";
        

        public string Phone { get; set; }

        [ForeignKey("City")]
        public int CityID { get; set; } 
        public virtual City City { get; set; }

        public string showpassword { get; set; } //mult part

        public string img { get; set; } //mult part


        [ForeignKey("Country")]
        public int CountryID { get; set; }

        public virtual Country Country { get; set; }


        //تم اضافته لتعامل مع السيرفس اما 
        //UuserName  ده هنساويه بال email
        public string user_name { get; set; } //= first_name + " " + last_name;
        public int type_user { get; set; } //Client = 1  //deleget = 2 //org_delget = 3//Admin = 4

      

        public string lat { get; set; }
        public string lng { get; set; }
        public string address { get; set; }


        public virtual ICollection<Advertisment> Advertisments { get; set; }
        public virtual ICollection<Favorite> Favorites { get; set; }
    }
}
