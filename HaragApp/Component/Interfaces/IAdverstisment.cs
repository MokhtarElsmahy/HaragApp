using HaragApp.Models;
using HaragApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HaragApp.Component.Interfaces
{
    interface IAdverstisment
    {
        AdsImagesVm Create(AdsImagesVm advertisment);
        AdsImagesVm Details(int? id);
    }
}
