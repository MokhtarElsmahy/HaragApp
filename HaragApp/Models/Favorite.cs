using HaragApp.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HaragApp.Models
{
    public class Favorite
    {
      

        [ForeignKey("Advertisment")]
        public int AdID { get; set; }
        public virtual Advertisment Advertisment { get; set; }


        [ForeignKey("ApplicationDbUser")]
        public string UserId { get; set; }
        public virtual ApplicationDbUser User { get; set; }
    }
}
