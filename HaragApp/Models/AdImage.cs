using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HaragApp.Models
{
    public class AdImage
    {
        [Key]
        public int ID { get; set; }
        public string img { get; set; }

        [ForeignKey("Advertisment")]
        public int AdID { get; set; }
        public virtual Advertisment Advertisment { get; set; }

    }
}
