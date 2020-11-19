using HaragApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaragApp.Commonet
{
   public interface IAnimalCategory
    {
        AnimalCategory Create(AnimalCategory animalCategory);
        AnimalCategory Get(int id);
    }
}
