using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaragApp.PathUrl
{
    public class ApiRoutes
    {
        public const string Root = "api";

        public const string Version = "v1";

        public const string Base = Root + "/" + Version;

     
        public static class setting
        {
            
 public const string PostAdvertisment = Base + "/PostAdvertisment";
            public const string GetOffersDashbord = Base + "/GetOffersDashbord";
            public const string GetQAndAnswer = Base + "/GetQAndAnswer";
            public const string CondtionsForDelegt = Base + "/CondtionsForDelegt";
            public const string AboutUsForDelegt = Base + "/AboutUsForDelegt";
            public const string Condtions = Base + "/Condtions";
            public const string ContactUs = Base + "/ContactUs";
            public const string AboutUs = Base + "/AboutUs";
            public const string GetAdvertsment = Base + "/GetAdvertsment";
            public const string GetSetting = Base + "/GetSetting";
            public const string GetAllPaidAdv = Base + "/GetAllPaidAdv";
            public const string Shop = Base + "/Shop";

            public const string addComment = Base + "/addComment";


            public const string Addcomplaints = Base + "/Addcomplaints";

            public const string GetQAndAnswerforDeleget = Base + "/GetQAndAnswerforDeleget";
            public const string CondtionsforDeleget = Base + "/CondtionsforDeleget";
            public const string ContactUsforDeleget = Base + "/ContactUsforDeleget";
            public const string AboutUsforDeleget = Base + "/AboutUsforDeleget";
            public const string GetSettingforDeleget = Base + "/GetSettingforDeleget";
            public const string AddcomplaintsforDeleget = Base + "/AddcomplaintsforDeleget";


        }




        public static class Identity
        {
            // delegt

            public const string DeleteNotificationDeleget = Base + "/DeleteNotificationDeleget";

            public const string UpdateDataForDelegt = Base + "/UpdateDataForDelegt";
            public const string GetListOfNotifyForDelegt = Base + "/GetListOfNotifyForDelegt";
            public const string GetDataOfUserForDelegt = Base + "/GetDataOfUserForDelegt";
            public const string ChangePasswardForDelegt = Base + "/ChangePasswardForDelegt";
            public const string resend_codeForDelegt = Base + "/resend_codeForDelegt";
            public const string ChangePasswordByCodeForDelegt = Base + "/ChangePasswordByCodeForDelegt";
            public const string LoginForDelegt = Base + "/loginForDelegt";
            public const string Forget_passwordForDelegt = Base + "/Forget_passwordForDelegt";

            public const string RegisterforDeleget = Base + "/registerforDeleget";
            public const string ConfirmCodeRegisterforDeleget = Base + "/ConfirmCodeRegisterforDeleget";
            // client
            public const string DeleteNotificationClient = Base + "/DeleteNotificationClient";
            public const string GetFinancial_accounts = Base + "/GetFinancial_accounts";
            public const string GetListOfNotifyForClient = Base + "/GetListOfNotifyForClient";
            public const string ChangeReciveOrder = Base + "/ChangeReciveOrder";
            public const string UpdateDataUser = Base + "/UpdateDataUser";
            public const string ChangePasswordByCode = Base + "/ChangePasswordByCode";
            public const string Login = Base + "/login";
            public const string Forget_password = Base + "/Forget_password";
            public const string Register = Base + "/register";
            public const string resend_code = Base + "/resend_code";
            public const string ConfirmCodeRegister = Base + "/ConfirmCodeRegister";
            public const string reset_password = Base + "/reset_password";
            public const string ChangePassward = Base + "/ChangePassward";
            public const string GetDataOfUser = Base + "/GetDataOfUser";

            public const string GetCities = Base + "/GetCities";



            // addtional services from user 



        }
    }
}
