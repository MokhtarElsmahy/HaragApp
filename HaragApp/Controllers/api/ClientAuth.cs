﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using HaragApp.Data;
using HaragApp.Models;
using HaragApp.PathUrl;
using HaragApp.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;


namespace HaragApp.Controllers.api
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ClientAuthController : BaseController
    {

        private readonly UserManager<ApplicationDbUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<ApplicationDbUser> _signInManager;
        private readonly ApplicationDbContext db;
        private readonly IHostingEnvironment HostingEnvironment;

        public ClientAuthController(ApplicationDbContext _db, IHostingEnvironment HostingEnvironment, UserManager<ApplicationDbUser> userManager, SignInManager<ApplicationDbUser> signInManager, IConfiguration configuration, IHttpContextAccessor haccess)
            : base(_db, HostingEnvironment, userManager, signInManager, configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
            _signInManager = signInManager;
            db = _db;
            this.HostingEnvironment = HostingEnvironment;

        }


        #region MainInfo




        [AllowAnonymous]
        // /register
        [HttpPost(ApiRoutes.Identity.Register)]
        public async Task<ActionResult> InsertUser(RegisterViewModel userModel)
        {

            #region MyRegion
            // string PhoneNumber=   await LoadPhoneNumberAsync();

            //try
            //{
            //    //var verification = await VerificationResource.CreateAsync(
            //    //    to: PhoneNumber,
            //    //    channel: "sms",
            //    //    pathServiceSid: _settings.VerificationServiceSID
            //    //);

            //    //if (verification.Status == "pending")
            //    //{
            //    //    return RedirectToPage("ConfirmPhone");
            //    //}

            //   // ModelState.AddModelError("", $"There was an error sending the verification code: {verification.Status}");
            //}
            //catch (Exception)
            //{
            //    ModelState.AddModelError("",
            //        "There was an error sending the verification code, please check the phone number is correct and try again");
            //} 
            #endregion

            #region validation



            if (userModel.phone.Length > 12)
            {
                return Json(new
                {
                    key = 0,
                    msg = creatMessage("ar", "من فضلك ادخل رقم الهاتف دون كود الدوله", "Please enter your phone number without country code")
                });
            }
            //if (userModel.fk_city == 0)
            //{
            //    return Json(new
            //    {
            //        key = 0,
            //        msg = creatMessage(userModel.lang, "من فضلك ادخل المدينه", "Please enter your city")
            //    });
            //}
            if (userModel.phone == null)
            {
                return Json(new
                {
                    key = 0,
                    msg = creatMessage(userModel.lang, "من فضلك ادخل رقم الجوال", "Please enter your phone number")
                });
            }
            string Registerphone = $"966{userModel.phone}";
            var phone = (db.Users.Where(x => x.Phone == Registerphone).FirstOrDefault());
            if (phone != null)
            {
                return Json(new
                {
                    key = 0,
                    msg = creatMessage(userModel.lang, "عذرا هذا الجوال موجود بالفعل", "Sorry this mobile is already present")
                });
            }
            #region MyRegion
            // email
            //if (userModel.email != null)
            //{
            //    bool checkemail = IsValidEmail(userModel.email);
            //    if (!checkemail)
            //    {
            //        return Json(new
            //        {
            //            key = 0,
            //            msg = creatMessage(userModel.lang, "من فضلك ادخل البريد الالكترونى بشكل صحيح", "Make sure your e-mail is written correctly")
            //        });
            //    }
            //    var email = (db.Users.Where(x => x.Email == userModel.email).FirstOrDefault());
            //    if (email != null)
            //    {
            //        return Json(new
            //        {
            //            key = 0,
            //            msg = creatMessage(userModel.lang, "عذرا هذا البريد الالكترونى موجود بالفعل", "Sorry this email is already present")
            //        });
            //    }
            //} 
            #endregion

            //Password
            if (userModel.password == null)
            {
                return Json(new
                {
                    key = 0,
                    msg = creatMessage(userModel.lang, "من فضلك ادخل  كلمة المرور", "Please enter your password")
                });
            }


            #endregion

            int code = GetFormNumber();
            string smallCode = code.ToString().Substring(0,4);
            Random rnd = new Random();
            int num = rnd.Next(1000, 99999);
            var user = new ApplicationDbUser
            {

                UserName = $"{Registerphone}user.com",
                Email = $"{Registerphone}user.com",
                user_name = string.Empty,
                showpassword = userModel.password,
                img = BaisUrlHoste + "/images/User/generic-user.png",
                IsActive = true,

                lat = userModel.lat,
                lng = userModel.lng,

                publish_date = TimeNow(),
                code = int.Parse(smallCode),
                PhoneNumber = Registerphone,
                type_user = 1,
                CityID = 1,
                SecurityStamp = Guid.NewGuid().ToString(),
                Phone = Registerphone

            };

            var result = await _userManager.CreateAsync(user, userModel.password);

            #region MyRegion
            //if (result.Succeeded)
            //{
            //    await _userManager.AddToRoleAsync(user, "Mobile");

            //}
            //else
            //{
            //    return Json(new
            //    {
            //        key = 0,
            //        msg = creatMessage(userModel.lang, result.ToString(), result.ToString())
            //    });
            //}user.code.ToString() 
            #endregion

            db.SaveChanges();
            Task<string> s = SendMessage(user.code.ToString(), user.Phone.ToString());
            return Json(new
            {
                key = 1,
                data = GetUserInfo(user.Id, userModel.lang),
                msg = creatMessage(userModel.lang, "تم التسجيل بنجاح", "successfully registered"),
                status = false,
                code = int.Parse(smallCode)
            });
        }

        [AllowAnonymous]
        [HttpPost(ApiRoutes.Identity.ConfirmCodeRegister)]
        public ActionResult ConfirmCodeRegister(/*[FromForm]*/ ConfirmCodeViewModel userModel)
        {
            try
            {
                //string userdataid = User.Claims.FirstOrDefault(x => x.Type == "user_id")?.Value;
                if (userModel.code == 0)
                {
                    return Json(new
                    {
                        key = 0,
                        msg = creatMessage(userModel.lang, "من فضلك ادخل كود التحقق", "Please enter your verification code")
                    });
                }

                var codeuser = (db.Users.Where(x => x.Id == userModel.user_id).SingleOrDefault());
                if (codeuser != null)
                {
                    if (codeuser.code == userModel.code)
                    {
                        codeuser.active_code = true;
                        db.SaveChanges();

                        return Json(new
                        {
                            key = 1,
                            data = GetUserInfo(codeuser.Id, userModel.lang),
                            msg = creatMessage(userModel.lang, "تم تفعيل الدخول بنجاح", "Logged in successfully")
                        });
                    }
                    else
                    {
                        return Json(new { key = 0, msg = creatMessage(userModel.lang, "برجاء ادخال الكود بشكل صحيح", "Please enter the code correctly") });
                    }
                }
                else
                {
                    return Json(new { key = 0, msg = creatMessage(userModel.lang, "عذرا هذا المستخدم غير مسجل لدينا", "Sorry this phone is not registered") });

                }

            }
            catch (Exception ex)
            {
                return Json(new
                {
                    key = 0,
                    msg = ex.Message
                });
            }

        }


        [AllowAnonymous]
        [HttpPost(ApiRoutes.Identity.Login)]
        public async Task<ActionResult> Login(LoginViewModel userModel)
        {
            #region validation

            if (userModel.phone == "")
            {
                return Json(new
                {
                    key = 0,
                    msg = creatMessage("ar", "من فضلك ادخل من رقم الهاتف", "Please enter your phone number")
                });
            }

            if (userModel.phone.Length>12)
            {
                return Json(new
                {
                    key = 0,
                    msg = creatMessage("ar", "من فضلك ادخل رقم الهاتف دون كود الدوله", "Please enter your phone number without country code")
                });
            }

            if (userModel.password == "")
            {
                return Json(new
                {
                    key = 0,
                    msg = creatMessage("ar", "من فضلك ادخل  كلمة المرور ", "Please enter your  password")
                });
            }

            HttpContext.Session.SetString("ID", "The Doctor");
            string phone = $"966{userModel.phone}";
            var user = (db.Users.Where(x => x.Phone == phone).SingleOrDefault());

            if (user == null)
            {
                return Json(new
                {
                    key = 0,
                    msg = creatMessage("ar", "يرجى التاكد من البيانات", "Please check your data")
                });
            }



            if (userModel.password != user.showpassword)
            {
                return Json(new
                {
                    key = 0,
                    msg = creatMessage("ar", "من فضلك تاكد من كلمة المرور ", "Please sure your  password")
                });
            }

            //if (user.IsActive == false && user.active_code == true)
            //{
            //    return Json(new
            //    {
            //        key = 0,
            //        data = new { },
            //        status = "blocked",
            //        msg = creatMessage("ar", "هذا الحساب مغلق من قبل الادمن", "This account is closed by the addict")
            //    });
            //}
            if (user.active_code == false)
            {
                return Json(new
                {
                    key = 1,
                    data = new
                    {
                        id = user.Id,
                        user.code
                    },

                    status = false,
                    msg = creatMessage("ar", "هذا الحساب لم يفعل بعد", "This account is not active")
                });
            }
            #endregion
            if (user != null && await _userManager.CheckPasswordAsync(user, userModel.password))
            {
                var claim = new[] {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                    new Claim("user_id", user.Id),
                   // new Claim("lang", userModel.lang)
                };
                HttpContext.Session.SetString("ID", user.Id.ToString());



                var signinKey = new SymmetricSecurityKey(
                  Encoding.UTF8.GetBytes(_configuration["Jwt:SigningKey"]));

                int expiryInMinutes = Convert.ToInt32(_configuration["Jwt:ExpiryInMinutes"]);

               

                var token = new JwtSecurityToken(
                  issuer: _configuration["Jwt:Site"],
                  audience: _configuration["Jwt:Site"],
                  claims: claim,
                  expires: DateTime.UtcNow.AddMinutes(expiryInMinutes),
                  signingCredentials: new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256)
                );


                return Json(new
                {
                    key = 1,
                    data = GetUserInfo(user.Id, "ar"),
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo,
                    status = true,
                    msg = creatMessage("ar", "تم تسجيل الدخول بنجاح", "Logged in successfully")
                });




            }
            return Unauthorized();
        }



        [AllowAnonymous]
        [HttpPost(ApiRoutes.Identity.GetDataOfUser)]
        public async Task<IActionResult> GetDataOfUser(GetUserDataViewModel userModel)
        {
            try
            {
                //var id = HttpContext.Session.GetString("ID");
                //var claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
                //string id = User.FindFirstValue(ClaimTypes.PrimarySid);
                //Filter specific claim    
                //  string id = claims?.FirstOrDefault(x => x.Type.Equals("user_id", StringComparison.OrdinalIgnoreCase))?.Value;
                //string user_id = User.Claims.FirstOrDefault(x => x.Type == "user_id")?.Value;
                //var user = await _userManager.FindByIdAsync(id);


                // var user = await _userManager.FindByIdAsync(userModel.user_id);
                string ID ;
                if (!string.IsNullOrEmpty(userModel.user_id))
                {
                    var user = await _userManager.FindByIdAsync(userModel.user_id);
                    ID = userModel.user_id;
                }
                else
                {
                    ID = HttpContext.Session.GetString("ID");
                }
                return Json(new
                {
                    key = 1,
                    data = GetUserInfo(ID),
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    key = 0,
                    msg = ex.Message
                });
            }
        }


        #region MyRegion

        //[HttpPost(ApiRoutes.Identity.UpdateDataUser)]
        //public async Task<ActionResult> UpdateDataUser(UpdateDataUserViewModel userModel)
        //{
        //    string user_id = User.Claims.FirstOrDefault(x => x.Type == "user_id")?.Value;
        //    var user = await _userManager.FindByIdAsync(user_id);
        //    if (userModel.phone != null)
        //    {
        //        var phonechek = (db.Users.Where(x => x.PhoneNumber == userModel.phone && x.Id != user_id).SingleOrDefault());

        //        if (phonechek != null)
        //        {
        //            return Json(new
        //            {
        //                key = 0,
        //                msg = creatMessage(userModel.lang, "هذا الرقم موجود من قبل", "This number already exists")
        //            });
        //        }
        //    }
        //    if (userModel.email != null)
        //    {
        //        var emailchek = (db.Users.Where(x => x.Email == userModel.email && x.Id != user_id).SingleOrDefault());

        //        if (emailchek != null)
        //        {
        //            return Json(new
        //            {
        //                key = 0,
        //                msg = creatMessage(userModel.lang, "هذا البريد موجود من قبل", "This email already exists")
        //            });
        //        }
        //    }
        //    user.address = userModel.address ?? user.address;
        //    user.lng = userModel.lng ?? user.lng;
        //    user.lat = userModel.lat ?? user.lat;
        //    user.user_name = userModel.user_name ?? user.user_name;
        //    user.PhoneNumber = userModel.phone ?? user.PhoneNumber;

        //    user.Email = userModel.email;


        //    //if (userModel.Img != null)
        //    //{
        //    //    var uploads = Path.Combine(HostingEnvironment.WebRootPath, "images/User");

        //    //    var fileName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(userModel.Img.FileName);
        //    //    using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
        //    //    {
        //    //        await userModel.Img.CopyToAsync(fileStream);
        //    //        user.img = BaisUrlUser + fileName;
        //    //    }
        //    //}
        //    //else
        //    //{
        //    //    user.img = user.img;
        //    //}


        //    //}
        //    db.SaveChanges();


        //    return Json(new
        //    {
        //        key = 1,
        //        data = GetUserInfo(user.Id, userModel.lang),
        //        msg = creatMessage(userModel.lang, "تم التعديل بنجاح", "successfully modified"),
        //        status = true
        //    });

        //} 
        #endregion


        [AllowAnonymous]
        [HttpPost(ApiRoutes.Identity.ChangePassward)]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel userModel)
        {
            // string user_id = User.Claims.FirstOrDefault(x => x.Type == "user_id")?.Value;
            var user = await _userManager.FindByIdAsync(userModel.user_id);
            if (user == null)
            {
                return Json(new
                {
                    key = 0,
                    msg = creatMessage(userModel.lang, "عذرا لم يتم العثور على هذا المستخدم ", "Sorry this User was not found")
                });
            }
            if (userModel.old_password == null)
            {
                return Json(new
                {
                    key = 0,
                    msg = creatMessage(userModel.lang, "من فضلك ادخل كلمة المرور القديمة ", "Please enter your old password")
                });
            }
            if (userModel.new_password == null)
            {
                return Json(new
                {
                    key = 0,
                    msg = creatMessage(userModel.lang, "من فضلك ادخل كلمة المرور الجديدة  ", "Please enter your new password")
                });
            }
            if (user.showpassword != userModel.old_password)
            {
                return Json(new
                {
                    key = 0,
                    msg = creatMessage(user.lang, " كلمة المرور القديمة غير صحيحة", "Please enter the old password correctly")
                });
            }


            var changePasswordResult = await _userManager.ChangePasswordAsync(user, userModel.old_password, userModel.new_password);
            if (!changePasswordResult.Succeeded)
            {
                return Json(new { key = 0, msg = creatMessage(userModel.lang, changePasswordResult.ToString(), "Something went wrong") });
            }
            user.showpassword = userModel.new_password;
            db.SaveChanges();
            await _signInManager.RefreshSignInAsync(user);
            return Json(new { key = 1, msg = creatMessage(userModel.lang, "تم تغيير الباسورد بنجاح", "Password changed successfully") });
        }



        [AllowAnonymous]
        [HttpPost(ApiRoutes.Identity.Forget_password)]
        public ActionResult Forget_password(ForgetPasswordViewModel userModel)
        {
            try
            {
                if (userModel.phone == "")
                {
                    return Json(new
                    {
                        key = 0,
                        msg = creatMessage(userModel.lang, "من فضلك ادخل رقم الهاتف", "Plaese enter your phone number")
                    });
                }

                if (userModel.phone.Length > 12)
                {

                    return Json(new
                    {
                        key = 0,
                        msg = creatMessage("ar", "من فضلك ادخل رقم الهاتف دون كود الدوله", "Please enter your phone number without country code")
                    }) ; 

                }
                string phone = $"966{userModel.phone}";
                var codeuser = (db.Users.Where(x => x.Phone == phone).SingleOrDefault());
               // var codeuser = (db.Users.Where(x => x.PhoneNumber == userModel.phone).SingleOrDefault());

                if (codeuser != null)
                {
                    if (codeuser.IsActive == false)
                    {
                        return Json(new
                        {
                            key = 0,
                            data = new { },
                            status = "blocked",
                            msg = creatMessage(userModel.lang, "هذا الحساب مغلق من قبل الادمن", "This account is closed by the addict")
                        });
                    }

                    int code =int.Parse(codeuser.code.ToString());
                    Task<string> s = SendMessage(code.ToString(), userModel.phone);
                    codeuser.code = code;
                    db.SaveChanges();
                    return Json(new
                    {
                        key = 1,
                        code = new { code = code, user_id = codeuser.Id },
                        msg = creatMessage(userModel.lang, "تم ارسال الكود ", "Code sent"),
                        status = "active",
                    });
                }
                else
                {
                    return Json(new { key = 0, msg = creatMessage(userModel.lang, "عذرا رقم الهاتف غير مسجل لدينا", "Sorry phone number is not registered") });
                }
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    key = 0,
                    msg = ex.Message
                });
            }
        }


        [AllowAnonymous]
        [HttpPost(ApiRoutes.Identity.resend_code)]
        public ActionResult resend_code(ResendCodeViewModel userModel)
        {

            try
            {
                var codeuser = (db.Users.Where(x => x.Id == userModel.user_id).SingleOrDefault());

                if (codeuser != null)
                {
                    Random rnd = new Random();
                    int code = GetFormNumber();
                    Task<string> s = SendMessage(code.ToString(), codeuser.Phone);
                    codeuser.code = code;
                    db.SaveChanges();
                    return Json(new
                    {
                        key = 1,
                        code = new { code = code, user_id = codeuser.Id, phone = codeuser.PhoneNumber },
                        msg = creatMessage(userModel.lang, "تم ارسال الكود ", "Code sent"),

                    });
                }
                else
                {
                    return Json(new { key = 0, msg = creatMessage(userModel.lang, "عذرا رقم الهاتف غير مسجل لدينا", "Sorry phone number is not registered") });
                }

            }
            catch (Exception ex)
            {
                return Json(new
                {
                    key = 0,
                    msg = ex.Message
                });
            }
        }



        #region MyRegion

        [AllowAnonymous]
        [HttpPost(ApiRoutes.Identity.ChangePasswordByCode)]
        public async Task<IActionResult> ChangePasswordByCode(ChangePasswordByCodeViewModel userModel)
        {
            try
            {

                if (userModel.code == 0)
                {
                    return Json(new
                    {
                        key = 0,
                        msg = creatMessage(userModel.lang, "من فضلك ادخل كود التحقق", "Please enter your verification code")
                    });
                }
                if (userModel.new_password == "")
                {
                    return Json(new
                    {
                        key = 0,
                        msg = creatMessage(userModel.lang, "من فضلك ادخل كلمة المرور الجديدة ", "Please enter your new password")
                    });
                }
                try
                {

                    var codeuser = (db.Users.Where(x => x.Id == userModel.user_id).SingleOrDefault());
                    if (codeuser != null)
                    {
                        if (userModel.code != codeuser.code)
                        {
                            return Json(new
                            {
                                key = 0,
                                msg = creatMessage(userModel.lang, "يرجى التحقق من الكود  ", "Please check the code")
                            });
                        }
                        var changePasswordResult = await _userManager.ChangePasswordAsync(codeuser, codeuser.showpassword, userModel.new_password);
                        if (!changePasswordResult.Succeeded)
                        {
                            return Json(new { key = 0, msg = creatMessage(userModel.lang, changePasswordResult.ToString(), "Something went wrong") });
                        }
                        codeuser.showpassword = userModel.new_password;
                        db.SaveChanges();


                        return Json(new { key = 1, msg = creatMessage(userModel.lang, "تم تغيير الباسورد بنجاح", "Password changed successfully") });
                    }
                    else
                    {
                        return Json(new { key = 0, msg = creatMessage(userModel.lang, " كود التحقق غير صحيح", "  Invalid verification code") });
                    }
                }
                catch (Exception)
                {
                    return Json(new { key = 0, msg = creatMessage(userModel.lang, "حدث خطا ما", "Something went wrong") });
                }
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    key = 0,
                    msg = ex.Message
                });
            }
        }

        #endregion

        [AllowAnonymous]
        [HttpPost(ApiRoutes.Identity.GetCities)]
        public List<City> getAllCities()
        {

            var cities = db.Cities.ToList();
            return cities;
        }

        #region MyRegion
        //[AllowAnonymous]
        //[HttpPost(ApiRoutes.Identity.ChangePasswordByCode)]
        //public async Task<IActionResult> ChangePasswordByCode(ConfirmPhoneModel confirmPhoneModel)
        //{
        //    //string PhoneNumber=await LoadPhoneNumberAsync();

        //    //try
        //    //{
        //    //    var verification = await VerificationCheckResource.CreateAsync(
        //    //        to: PhoneNumber,
        //    //        code: confirmPhoneModel.VerificationCode,
        //    //        pathServiceSid: _settings.VerificationServiceSID
        //    //    );
        //    //    if (verification.Status == "approved")
        //    //    {
        //    //        var identityUser = await _userManager.GetUserAsync(User);
        //    //        identityUser.PhoneNumberConfirmed = true;
        //    //        identityUser.active_code = true;
        //    //        var updateResult = await _userManager.UpdateAsync(identityUser);

        //    //        if (updateResult.Succeeded)
        //    //        {
        //    //            return RedirectToAction("Index","Home");
        //    //        }
        //    //        else
        //    //        {
        //    //            ModelState.AddModelError("", "There was an error confirming the verification code, please try again");
        //    //        }
        //    //    }
        //    //    else
        //    //    {
        //    //        ModelState.AddModelError("", $"There was an error confirming the verification code: {verification.Status}");
        //    //    }
        //    //}
        //    //catch (Exception)
        //    //{
        //    //    ModelState.AddModelError("",
        //    //        "There was an error confirming the code, please check the verification code is correct and try again");
        //    //}
        //    return RedirectToAction("Create", "Advertisments");

        //} 
        #endregion




    }



    #endregion


}
