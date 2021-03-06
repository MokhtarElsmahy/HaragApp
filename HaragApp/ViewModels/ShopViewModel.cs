﻿using HaragApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaragApp.ViewModels
{
    public class ShopViewModel
    {
        public string search { get; set; }
        public int CategoryId { get; set; }
        public int CityId { get; set; }
        public int? PageNo { get; set; }

        public string userID { get; set; }

        public List<AdsImagesVM2> Advertisments { get; set; }
        public List<AnimalCategory> Categories { get; set; }
        public List<City> Cities { get; set; }
        public int Km { get; set; }
        public double Lang { get; set; }
        public double Lat { get; set; }

        public int AllAdsCount { get; set; }
        public string shopSliderImage { get; set; }

        public int ActiveCity { get; set; }
        public int ActiveCategory { get; set; }
    }
}
