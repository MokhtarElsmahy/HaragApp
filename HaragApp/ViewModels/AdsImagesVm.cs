using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HaragApp.ViewModels
{
    public class AdsImagesVm
    {
        public int AdID { get; set; }
        public string Title { get; set; }

        [Required]
        public bool IsPact { get; set; }
        public string UserId { get; set; }
        public int CityID { get; set; }
        public int CategoryID { get; set; }
        public string ImageUrl1 { get; set; }
        public string ImageUrl2 { get; set; }
        public string ImageUrl3 { get; set; }
        public string ImageUrl4 { get; set; }
        public string ImageUrl5 { get; set; }
    }
}
