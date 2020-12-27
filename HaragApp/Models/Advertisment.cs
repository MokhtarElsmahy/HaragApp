using HaragApp.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace HaragApp.Models
{
    public class Advertisment
    {
        [Key]
        public int AdID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public bool IsPact { get; set; } = true;

        [ForeignKey("ApplicationDbUser")]
        public string UserId { get; set; }
        public virtual ApplicationDbUser User { get; set; }

        [ForeignKey("City")]
        public int CityID { get; set; }
        public virtual City City { get; set; }

        [ForeignKey("AnimalCategory")]
        public int CategoryID { get; set; }


        public bool IsPaid { get; set; } = false;
        //public string IsPaidDescription { get; set; }


        public virtual AnimalCategory AnimalCategory { get; set; }

        public virtual ICollection<AdImage> AdImages { get; set; }

        public virtual ICollection<Favorite> Favorites { get; set; }


    }
}
