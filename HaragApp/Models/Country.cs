using HaragApp.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HaragApp.Models
{
    public class Country
    {
        [Key]
        public int CountryID { get; set; }
        public string CountryName { get; set; }


        public virtual ICollection<ApplicationDbUser> Users { get; set; }
        public virtual ICollection<City> Cities { get; set; }
    }
}
