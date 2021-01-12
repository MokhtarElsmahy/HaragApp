using HaragApp.Models;
using HaragApp.ViewModels;
using Microsoft.AspNetCore.Http;
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
        AdsImagesVM2 Details(int? id);
        List<AdsImagesVM2> GetUserAdvertisementsAsync(string id);

        List<PaidAddViewModel> GetAllPaidAdv();
        public void DeletePaidAdd(int id);

        public AdsImagesVm GetPaidAdv(int id);

        public void EditPaidAdv(AdsImagesVm advertisment);
        bool userDeleteADV(int id);
        AdsImagesVm userUpdateADV(int id , AdsImagesVm newADV);
        public ShopViewModel Shop(ShopViewModel model);

        public bool AddToFav(int AdID, String userID);

        List<favoriteViewModel> GetUserFavorites(string userID);
        public bool DeleteFav(string userID, int ADVid);

        public List<favoriteViewModel> GetAdvertismentsForIndex();
        public List<AdsImagesVm> GetAllAdvertisemtsData(string userID);
        public favoriteViewModel GetAdvertisementByID(int ADid);
        public List<favoriteViewModel> GetTopFiveFavs();

        public AdsImagesVm UpdateAsync(AdsImagesVm advertisment , IFormFileCollection files);
    }
}
