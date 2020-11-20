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

        ICollection<PaidAddViewModel> GetAllPaidAdv();
        public void DeletePaidAdd(int id);

        public AdsImagesVm GetPaidAdv(int id);

        public void EditPaidAdv(AdsImagesVm advertisment);
        bool userDeleteADV(int id);
        AdsImagesVm userUpdateADV(int id , AdsImagesVm newADV);
    }
}
