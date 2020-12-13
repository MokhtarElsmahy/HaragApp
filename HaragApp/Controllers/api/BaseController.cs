using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HaragApp.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RestSharp;

namespace HaragApp.Controllers.api
{
    public class BaseController : Controller
    {
        private readonly UserManager<ApplicationDbUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<ApplicationDbUser> _signInManager;
        private readonly ApplicationDbContext db;
        private readonly IHostingEnvironment HostingEnvironment;


        public BaseController(ApplicationDbContext _db, IHostingEnvironment HostingEnvironment, UserManager<ApplicationDbUser> userManager, SignInManager<ApplicationDbUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
            _signInManager = signInManager;
            db = _db;
            this.HostingEnvironment = HostingEnvironment;
        }


       
        public UserInfoViewModel GetUserInfo(string userId, string lang = "ar")
        {
            var UserDB = db.Users.Where(x => x.Id == userId).Select(x => new UserInfoViewModel
            {
                id=x.Id,
                user_name = x.user_name,
                CityID = x.CityID,
                email = x.Email,
                phone = x.Phone,
                lang=x.lang,
                lat=x.lat,
               
               
            }).FirstOrDefault();

            return UserDB;
        }
        public class UserInfoViewModel
        {
          
            public string id { get; set; }
            public string user_name { get; set; } = "";
            public string email { get; set; } = "";
            public int CityID { get; set; } = 1;
           
            public string phone { get; set; } = "";
         
         
            public string lat { get; set; }
            public string lng { get; set; }
            public string address { get; set; }
            public string lang { get; set; }
        }
        public class DelegtInfoViewModel
        {


            public int fk_package { get; set; }
            public DateTime end_subscrip_date { get; set; }
            public DateTime start_subscrip_date { get; set; }
            public string lang { get; set; }
            public string first_name { get; set; }
            public string last_name { get; set; }

            public int cat_id { get; set; }
            public string cat_name { get; set; }
            public string org_name { get; set; }
            public int number_deleget { get; set; }
            public string rate { get; set; }
            public int type_user { get; set; }
            public string lat { get; set; }
            public string lng { get; set; }
            public string address { get; set; }
            public string id { get; set; }
            public string user_name { get; set; } = "";
            public int fk_city { get; set; } = 0;
            public string city_name { get; set; } = "";
            public string region { get; set; } = "";
            public string email { get; set; } = "";
            public string img { get; set; } = "";
            public string phone { get; set; } = "";
            public bool notify { get; set; }
            public bool recive_order { get; set; }
        }
        public DateTime TimeNow()
        {
            TimeZone localZone = TimeZone.CurrentTimeZone;
            DateTime currentDate = DateTime.Now;
            DateTime currentUTC =
           localZone.ToUniversalTime(currentDate);
            return currentUTC.AddHours(3);
        }




        public enum Order_Reservation_out
        {
            AcceptFromAdmin = 1, // 1- وتوجييه الطلب لمقدم الخدمه -- تم الموافقه من لوجه التحكم
        }
        #region المعادلات
        static double Avarage(double x1, double x2, double x3)
        {
            double result = (x1 + x2 + x3) / 3;
            return result;

        }
        static double effective(double avarage, double workload, double output, double backgraond)
        {
            double result = (((avarage * workload) / output) * 60) - backgraond;
            return result;

        }
        static double output(double mass, double secound)
        {
            double result = mass / secound;
            return result;

        }
        static string remark(double effective)
        {
            if (effective > 100)
            {
                return "fail";
            }
            else
            {
                return "pass";
            }


        }
        #endregion

        // GET: Base
        // GET: Base
        public readonly static string BaisUrlHoste = "https://rodina.ip4s.com/";
        public readonly static string BaisUrlAdvert = BaisUrlHoste + "images/Advert/";
        public readonly static string BaisUrlProduct = BaisUrlHoste + "images/Product/";
        public readonly static string BaisUrlCategory = BaisUrlHoste + "images/Category/";
        public readonly static string BaisUrlUser = BaisUrlHoste + "images/User/";
        public readonly static string BaisUrlCar_form = BaisUrlHoste + "images/Car_form/";
        public readonly static string BaisUrlId_photo = BaisUrlHoste + "images/Id_photo/";
        public readonly static string BaisUrOrder = BaisUrlHoste + "images/Order/";
        public readonly static string BaisUrlSubscribe = BaisUrlHoste + "images/Subscribe/";
        #region Validtion
        public static int GetFormNumber()
        {
            Random rnd = new Random();
            return rnd.Next();
        }
        //valid for email
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    var domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                return false;
            }
            catch (ArgumentException e)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
        // convert arabic number to english 
        public static string toEnglishNumber(string input)
        {
            string EnglishNumbers = "";

            for (int i = 0; i < input.Length; i++)
            {
                if (Char.IsDigit(input[i]))
                {
                    EnglishNumbers += char.GetNumericValue(input, i);
                }
                else
                {
                    EnglishNumbers += input[i].ToString();
                }
            }
            return EnglishNumbers;
        }
        // check if format is valid
        public static bool IsDateValid(string Date, string format)
        {
            DateTime dt;
            if (DateTime.TryParseExact(Date, format, null, DateTimeStyles.None, out dt))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
        #region date
        static List<DateTime> GetDatesBetween(DateTime startDate, DateTime endDate)
        {

            List<DateTime> allDates = new List<DateTime>();

            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
                allDates.Add(date);
            return allDates;

        }

        #endregion

        #region othor
        public static string creatMessage(string lang, string textAr, string textEn)
        {
            if (lang == "ar")
            {
                return textAr;
            }
            else
            {
                return textEn;
            }

        }

        //public void SendPushNotificationForDeleget(string user_id, string msg, int? type = 0, int? order_id = 0)
        //{
        //    try
        //    {

        //        var devide_ids = (from st in db.Device_Id where st.fk_userID == user_id select st).ToList();


        //        foreach (var item in devide_ids)
        //        {
        //            string applicationID = "AAAAgys_Amw:APA91bGe24U3_fchgBw-1w-GjdOdNHNEiMzC7O6M-NWlXrjJSA9kIk5Ie6CH4CDT_HDAfZyvraHyylmzkVoWcU63oeXS1s_86QFeDl2Y4ZcJuF8oZAtaMTZ9HyYfzmqw30I11U9caE_w";
        //            string senderId = "563366265452";
        //            string deviceId = item.device_id;
        //            WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
        //            tRequest.Method = "post";
        //            tRequest.ContentType = "application/json";
        //            if (item.device_id != null)
        //            {
        //                var data = new
        //                {
        //                    to = deviceId,

        //                    notification = new
        //                    {
        //                        body = msg,
        //                        title = "رودينا",
        //                        sound = "Enabled",
        //                        priority = "high",
        //                        type = type,
        //                        order_id = order_id
        //                        ,
        //                        click_action = "FLUTTER_NOTIFICATION_CLICK"
        //                    },
        //                    data = new
        //                    {
        //                        body = msg,
        //                        title = "رودينا",
        //                        sound = "Enabled",
        //                        priority = "high",
        //                        type = type,
        //                        order_id = order_id
        //                        ,
        //                        click_action = "FLUTTER_NOTIFICATION_CLICK"
        //                    }
        //                };
        //                var serializer = new JavaScriptSerializer();
        //                var json = serializer.Serialize(data);
        //                Byte[] byteArray = Encoding.UTF8.GetBytes(json);
        //                tRequest.Headers.Add(string.Format("Authorization: key={0}", applicationID));
        //                tRequest.Headers.Add(string.Format("Sender: id={0}", senderId));
        //                tRequest.ContentLength = byteArray.Length;
        //                using (Stream dataStream = tRequest.GetRequestStream())
        //                {
        //                    dataStream.Write(byteArray, 0, byteArray.Length);
        //                    using (WebResponse tResponse = tRequest.GetResponse())
        //                    {
        //                        using (Stream dataStreamResponse = tResponse.GetResponseStream())
        //                        {
        //                            using (StreamReader tReader = new StreamReader(dataStreamResponse))
        //                            {
        //                                String sResponseFromServer = tReader.ReadToEnd();
        //                                string str = sResponseFromServer;
        //                            }
        //                        }
        //                    }
        //                }
        //            }


        //        }





        //    }
        //    catch (Exception ex)
        //    {
        //        string str = ex.Message;

        //    }
        //}

        #endregion

        #region sms

        //public static async System.Threading.Tasks.Task<string> SendMessage(string msg, string numbers)
        //{

        //    using (var client = new HttpClient())
        //    {
        //        client.BaseAddress = new Uri("http://www.4jawaly.net/");
        //        //HTTP GET
        //        var response = await client.GetAsync($"api/sendsms.php?username=othaim&password=565656&numbers={numbers}&sender=ALOTHAIM&message={msg}&&return=string");
        //        var responseString = await response.Content.ReadAsStringAsync();
        //        return responseString;

        //    }

        //}

        //public static async Task<string> SendMessage(string msg, string numbers)
        //{


        //    string url = "http://api.yamamah.com/SendSMSV2?Username=966532866666&Password=Ht5pTY26&Message=987654321&RecepientNumber=966532866666&Tagname=Haraajm&SendDateTime=0";

        //    var client = new RestClient(url);
        //     var data = await client.Get();
        //    return "";
        //}

        public static async Task<string> SendMessage(string msg, string numbers)
        {
            var client = new RestClient($"http://api.yamamah.com/SendSMS");
            var request = new RestRequest(Method.POST);
            request.AddJsonBody(new {
                Username= "966532866666",
                Password= "Ht5pTY26",
                Tagname ="Haraajm",
                RecepientNumber= numbers,
                Message= msg

            });
            IRestResponse response = await client.ExecuteAsync(request);

            return "";
        }

        //mobily
        static public string SendMessagemobily(string msg, string numbers)
        {
            //int temp = '0';

            HttpWebRequest req = (HttpWebRequest)
            WebRequest.Create("http://www.mobily.ws/api/msgSend.php");
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            string postData = "mobile=966503444442" + "&password=" + "444442" + "&numbers=" + numbers + "&sender=" + "MTA.SA" + "&msg=" + msg + "&applicationType=68&lang=3";

            // string postData = "mobile=966569051984" + "&password=" + "ASD569051984" + "&numbers=" + numbers + "&sender=" + "Qatif Cars" + "&msg=" + msg + "&applicationType=68&lang=3";
            req.ContentLength = postData.Length;

            StreamWriter stOut = new
            StreamWriter(req.GetRequestStream(),
            System.Text.Encoding.ASCII);
            stOut.Write(postData);
            stOut.Close();
            // Do the request to get the response
            string strResponse;
            StreamReader stIn = new StreamReader(req.GetResponse().GetResponseStream());
            strResponse = stIn.ReadToEnd();
            stIn.Close();
            return strResponse;
        }
        public string SendMessageText(string msg, string numbers, string code)
        {
            //int temp = '0';

            HttpWebRequest req = (HttpWebRequest)
            WebRequest.Create("http://www.mobily.ws/api/msgSend.php");
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            string postData = "mobile=966503444442" + "&password=" + "444442" + "&numbers=" + numbers + "&sender=" + "MTA.SA" + "&msg=" + ConvertToUnicode(msg + " " + code) + "&applicationType=59";
            req.ContentLength = postData.Length;

            StreamWriter stOut = new
            StreamWriter(req.GetRequestStream(),
            System.Text.Encoding.ASCII);
            stOut.Write(postData);
            stOut.Close();
            // Do the request to get the response
            string strResponse;
            StreamReader stIn = new StreamReader(req.GetResponse().GetResponseStream());
            strResponse = stIn.ReadToEnd();
            stIn.Close();
            return strResponse;
        }

        private string ConvertToUnicode(string val)
        {
            string msg2 = string.Empty;

            for (int i = 0; i < val.Length; i++)
            {
                msg2 += convertToUnicode(System.Convert.ToChar(val.Substring(i, 1)));
            }

            return msg2;
        }

        private string convertToUnicode(char ch)
        {
            System.Text.UnicodeEncoding class1 = new System.Text.UnicodeEncoding();
            byte[] msg = class1.GetBytes(System.Convert.ToString(ch));

            return fourDigits(msg[1] + msg[0].ToString("X"));
        }

        private string fourDigits(string val)
        {
            string result = string.Empty;

            switch (val.Length)
            {
                case 1: result = "000" + val; break;
                case 2: result = "00" + val; break;
                case 3: result = "0" + val; break;
                case 4: result = val; break;
            }

            return result;
        }
        #endregion


        //public Tuple<List<dataorder>, List<dataorder>> GetAllNearestFamousPlaces(double currentLatitude, double currentLongitude,
        //  int km, List<dataorder> data)
        //{

        //    List<dataorder> advertsment = new List<dataorder>();
        //    var query = (from c in data

        //                 select c).ToList();
        //    foreach (var place in query)
        //    {
        //        double distance = Distance(currentLatitude, currentLongitude, Convert.ToDouble(place.lat), Convert.ToDouble(place.lng));
        //        if (distance <= km)         //nearbyplaces which are within 25 kms  50 w 70
        //        {
        //            dataorder dist = new dataorder();
        //            dist.img = place.img;
        //            dist.km = distance / 1000;
        //            dist.user_name = place.user_name;
        //            dist.rate = place.rate ?? "0";
        //            dist.deleget_id = place.deleget_id;
        //            dist.type_deleget = place.type_deleget;
        //            dist.lat = place.lat;
        //            dist.lng = place.lng;
        //            dist.address = place.address;
        //            advertsment.Add(dist);

        //        }

        //    }


        //    return Tuple.Create(normal_provider.ToList(), org_provider.ToList());
        //}
        public class DistanceModel
        {
            public int ID { get; set; }
            public string lat_from { get; set; }
            public string lng_from { get; set; }
            public int type { get; set; }

        }
        double Deg2Rad(double deg)
        {
            return deg * (Math.PI / 180d);
        }
        private double deg2rad(double deg)
        {
            return (deg * Math.PI / 180.0);
        }

        private double rad2deg(double rad)
        {
            return (rad * 180.0 / Math.PI);
        }
        private double Distance(double lat1, double lon1, double lat2, double lon2)
        {
            double theta = lon1 - lon2;
            double dist = Math.Sin(deg2rad(lat1)) * Math.Sin(deg2rad(lat2)) + Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) * Math.Cos(deg2rad(theta));
            dist = Math.Acos(dist);
            dist = rad2deg(dist);
            dist = (dist * 60 * 1.1515) / 0.6213711922;
            // dist = (dist  * 1.609344);   //miles to kms
            return (dist);
        }



        public class dataorder
        {

            public string id { get; set; }
            public string titke { get; set; }
            public string img { get; set; }
            public string rate { get; set; }
            public double km { get; set; }
            public int type_deleget { get; set; }
            public string lat { get; set; }
            public string lng { get; set; }
            public string address { get; set; }
        }


        //public static async Task<string> SendMessage(string msg, string numbers)
        //{
        //    string url = "http://api.yamamah.com/SendSMSV2?Username=966500469446&Password=123456789&Message=" + msg + "&RecepientNumber=" + numbers + "&Tagname=moodak&SendDateTime=0&EnableDR=false&SentMessageID=false";
        //    dynamic client = new RestClient(url);
        //    var data = await client.Get();
        //    return "";
        //}
    }
}
