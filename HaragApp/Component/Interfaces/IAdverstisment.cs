using HaragApp.Models;
using HaragApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaragApp.Component.Interfaces
{
    interface IAdverstisment
    {
        AdsImagesVm CreateAsync(AdsImagesVm advertisment);
        AdsImagesVm Details(int? id);
        List<AdsImagesVm> GetUserAdvertisementsAsync(string id);
    }
}
