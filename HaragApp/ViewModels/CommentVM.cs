using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HaragApp.ViewModels
{
    public class CommentVM
    {
        public int CommentID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string CommentText { get; set; }

        public DateTime Date { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public int advID { get; set; }

    }
}
