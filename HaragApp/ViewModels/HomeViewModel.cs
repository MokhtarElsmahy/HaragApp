using HaragApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaragApp.ViewModels
{
    public class HomeViewModel
    {
        public ICollection<PaidAddViewModel> PaidAdvs { get; set; }
        public List<AnimalCategory> animalCategories { get; set; }
        public List<favoriteViewModel> advertisments { get; set; }
        public List<favoriteViewModel> TOPadvertisments { get; set; }
        public Configs config { get; set; }
    }
}
