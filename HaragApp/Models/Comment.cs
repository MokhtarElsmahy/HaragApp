using HaragApp.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HaragApp.Models
{
    public class Comment
    {
        [Key]
        public int CommentID { get; set; }
        public string Name { get; set; }
        public string CommentText { get; set; }
        public DateTime Date { get; set; }


        [ForeignKey("ApplicationDbUser")]
        public string UserId { get; set; }
        public virtual ApplicationDbUser User { get; set; }

        [ForeignKey("Advertisment")]
        public int advID { get; set; }
        public virtual Advertisment Advertisment { get; set; }

    }
}
