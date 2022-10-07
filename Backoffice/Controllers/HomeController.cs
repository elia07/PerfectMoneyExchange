using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using RockCandy.Web.Framework.Core;
using RockCandy.Web.Framework.Core.Enumerations;
using RockCandy.Web.Framework.Core.ActionFilters;
using Saraf365.Backoffice.Filters;
using Saraf365.Core.Enumerations;
using Saraf365.Core;
using RockCandy.Web.Framework.Core.Models;
using Saraf365.Core.Repositories;
using RockCandy.Web.Framework.Utilities.Encryption;
using RockCandy.Web.Framework.Utilities;
using MvcReCaptcha;
using Saraf365.Backoffice;
using RockCandy.Web.Framework.Utilities.EmailUtils;
using System.Net.Mime;
using System.Text;
using Newtonsoft.Json;
using BotDetect.Web.Mvc;
using Saraf365.Backoffice.DomainUtils;
using Saraf365.Core.Utils;
using System.ComponentModel.DataAnnotations;

namespace Itsalat.Backoffice.Controllers
{
    [AllowCrossSiteJson]
    [NoCache]
    public class HomeController : Controller
    {
        
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [CaptchaValidation("CaptchaCode", "ExampleCaptcha", "Incorrect CAPTCHA code!")]
        public ActionResult Login(Admin instance, bool captchaValid)
        {
            try
            {
                if (captchaValid)
                {
                    MvcCaptcha.ResetCaptcha("ExampleCaptcha");
                    using (AdminRepository context = new AdminRepository())
                    {
                        Admin admin = context.GetByEmail(instance.xEmail);
                        if(admin!=null)
                        {
                            instance.xPassword = new CryptoUtils().ComputeSha256Hash(instance.xPassword + admin.xSalt);

                            if(context.Authentication(instance)!=null)
                            {
                                if (admin != null && admin.xIsActive)
                                {
                                   
                                    Session.Add("Admin", admin);
                                    if(admin.xTelegramID!=null)
                                    {
                                        new SystemLogRepository().Log(SystemLogType.BO_Login, "ورود ادمین", instance.GetSerializedData(), ((Admin)(Session["Admin"])).xID);
                                        try
                                        {
                                            new TelegramUtils().SendMessage(SectionInfo.Setting.TelegramBotAccessToken, (long)admin.xTelegramID, "ورود به ناحیه کاربری توسط حساب مدیریتی شما انجام شده است ، در صورتی که این ورود توسط شما انجام نشده است سریعا اقدام به تعویض رمز عبور خود کنید");
                                        }
                                        catch { }
                                    }
                                    return Json(new { Status = true, Message = "" });
                                }
                                else if (admin == null)
                                {
                                    return Json(new { Status = false, Message = "نام کاربری یا رمز عبور اشتباه است" });
                                }
                                else if (admin.xIsActive == false)
                                {
                                    return Json(new { Status = false, Message = "حساب کاربری شما غیر فعال است" });
                                }
                            }
                            else
                            {
                                return Json(new { Status = false, Message = "نام کاربری یا رمز عبور اشتباه است" });
                            }
                          
                        }
                        else
                        {
                            return Json(new { Status = false, Message = "نام کاربری یا رمز عبور اشتباه است" });
                        }
                        
                    }
                }
                return Json(new { Status = false ,Message="کد امنیتی صحیح نیست"});
            }
            catch
            {
                return Json(new { Status = false, Message="با پشتیبانی تماس بگیرید" });
            }

        }


        public ActionResult Page404()
        {
            return View();
        }
        public ActionResult AccessDenied()
        {
            return View();
        }

    }
}
