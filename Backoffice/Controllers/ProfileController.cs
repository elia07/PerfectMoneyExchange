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
using System.Threading.Tasks;
using Saraf365.Core.Utils;

namespace Saraf365.Backoffice.Controllers
{
    [NoCache]
    [AdminAuthorize]
    public class ProfileController : Controller
    {
       
        public ActionResult Index()
        {
           
            
            return View();
        }

        public ActionResult AdminProfile()
        {
            return View();
        }

        public ActionResult Charts()
        {
            return View();
        }
        public async Task<ActionResult> FinancialStatus()
        {
            ViewBag.PerfectMoneyAmount = new PerfectMoney(SectionInfo.Setting.PerfectMoneyID, SectionInfo.Setting.PerfectMoneyPassword, SectionInfo.Setting.PerfectMoneyAccount).GetBalance();
            if (ViewBag.PerfectMoneyAmount[0].Contains("disabled"))
            {
                ViewBag.PerfectMoneyAmount = new string[] { "0", "0" };
            }
            return View();
        }
        [HttpPost]
        [AjaxAntiForgeryValidateAttribute]
        public ActionResult Update(Admin args)
        {
            JsonResponse jr = new JsonResponse(false, "خطا در انجام عملیات ، دوباره تلاش کنید و در صورت تکرار موضوع را گزارش کنید.");
            try
            {

                using (AdminRepository ar = new AdminRepository(null, true))
                {
                    Admin instance = ar.GetByID(Convert.ToInt64(((Admin)Session["Admin"]).xID));

                    instance.xSalt = new CryptoUtils().GenerateSalt();
                    instance.xPassword = new CryptoUtils().ComputeSha256Hash(args.xPassword + instance.xSalt);

                    ar.Update(instance);

                }

                jr.Message = "ویرایش با موفقیت انجام شد";
                jr.Status = true;
                return Json(jr);

                //jr.Message = errors;

            }
            catch (Exception e)
            {
                jr.Message = "خطا در انجام عملیات ، درصورت مشاهده مجدد با موضوع را اطلاع رسانی کنید.";
            }
            return Json(jr);
        }

        public void Exit(Admin AdminUser)
        {
            Session.Remove("Admin");
            Response.Redirect(ConfigurationManager.AppSettings.Get("DomainAddress"));
        }

    }
}
