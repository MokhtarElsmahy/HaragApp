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
        public string Description { get; set; }

        [Required]
        public bool IsPact { get; set; }
        public bool IsPaid { get; set; }
        public string UserId { get; set; }
        public int CityID { get; set; }
        public string CityName { get; set; }
        public double Lang { get; set; }
        public double Lat { get; set; }
        public string userPhone { get; set; }
        public string CategoryName { get; set; }
        public int CategoryID { get; set; }
        public int Km { get; set; }
        public int PageNo { get; set; }

        public bool IsFav { get; set; }

        public string search { get; set; }
        public string ImageUrl1 { get; set; }
        public string ImageUrl2 { get; set; }
        public string ImageUrl3 { get; set; }
        public string ImageUrl4 { get; set; }
        public string ImageUrl5 { get; set; }
    }
}
