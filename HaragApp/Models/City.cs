using HaragApp.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HaragApp.Models
{
    public class City
    {
        [Key]
        public int CityID { get; set; }
        public string CityName { get; set; }

        public string Langtude { get; set; }
        public string Lantitude { get; set; }

        //[ForeignKey("Country")]
        //public int CountryID { get; set; }

        //public virtual Country Country { get; set; }


        public virtual ICollection<ApplicationDbUser> Users { get; set; }
        public virtual ICollection<Advertisment> Advertisments { get; set; }
    }
}
