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
using Saraf365.Backoffice;
using Newtonsoft.Json;
using Saraf365.Backoffice.DomainUtils;

namespace Saraf365.Backoffice.Controllers
{

    [NoCache]
    [AdminAuthorize]
    public class GatewayController : Controller
    {

        [BasicViewBagInitializer(CurrentController = "Gateway")]
        [SectionPermission(CrudOperation = CrudOperationType.Select, SectionType = AdminRoleSectionType.GatewayManagement)]
        public ActionResult Index(int currentPage = 1)
        {
            return View();
        }

        [HttpPost]
        [SectionPermission(CrudOperation = CrudOperationType.Insert, SectionType = AdminRoleSectionType.GatewayManagement)]
        [AjaxAntiForgeryValidateAttribute]
        public ActionResult Insert(Gateway args, string xSectionPermissions)
        {

            JsonResponse jr = new JsonResponse(false, "خطا در انجام عملیات ، مجددا تلاش کرده و در صورت تکرار موضوع را گزارش کنید");
            try
            {


                using (GatewayRepository opr = new GatewayRepository())
                {
                    args.xLastSuccessTransactionDate = DateTime.Now;
                    opr.Insert(args);
                }
                new SystemLogRepository().Log(SystemLogType.BO_Gateway, "ایجاد درگاه", args.GetSerializedData(), ((Admin)(Session["Admin"])).xID);
                jr.Message = "درج با موفقیت انجام شد";
                jr.Status = true;
                return Json(jr);

            }
            catch
            {
            }
            return Json(jr);

        }

        [HttpPost]
        [UniqueKeyDecryptor(UniqueKeyFieldName = "xID")]
        [SectionPermission(CrudOperation = CrudOperationType.Delete, SectionType = AdminRoleSectionType.GatewayManagement)]
        [AjaxAntiForgeryValidateAttribute]
        public ActionResult Delete(long xID = 0)
        {
            JsonResponse jr = new JsonResponse(false, "خطا در انجام عملیات ، دوباره تلاش کنید و در صورت تکرار موضوع را گزارش کنید.");
            try
            {
                string ipgName = "";
                using (GatewayRepository opr = new GatewayRepository())
                {
                    var instance = opr.GetByID(xID);
                    ipgName = instance.xName ;
                    opr.Delete(xID);
                }
                new SystemLogRepository().Log(SystemLogType.BO_Gateway, "حذف درگاه", ipgName, ((Admin)(Session["Admin"])).xID);
                jr.Message = "حذف با موفقیت انجام شد";
                jr.Status = true;
            }
            catch (Exception e)
            {
                jr.Message = "حذف این درگاه امکام پذیر نیست ، در صورت تمایل آنرا غیر فعال کنید";
            }

            return Json(jr);
        }

        [HttpGet]
        [UniqueKeyDecryptor(UniqueKeyFieldName = "xID")]
        [BasicViewBagInitializer(CurrentController = "Gateway")]
        [SectionPermission(CrudOperation = CrudOperationType.Update, SectionType = AdminRoleSectionType.GatewayManagement)]
        public ActionResult UpdateRender(long xID = 0)
        {
            ViewBag.xID = xID;
            return View();
        }

        [HttpGet]
        [BasicViewBagInitializer(CurrentController = "Gateway")]
        [SectionPermission(CrudOperation = CrudOperationType.Insert, SectionType = AdminRoleSectionType.GatewayManagement)]
        public ActionResult InsertRender()
        {
            return View();
        }

        [HttpPost]
        [UniqueKeyDecryptor(UniqueKeyFieldName = "xID")]
        [SectionPermission(CrudOperation = CrudOperationType.Update, SectionType = AdminRoleSectionType.GatewayManagement)]
        [AjaxAntiForgeryValidateAttribute]
        public ActionResult Update(Gateway args, long xID = 0)
        {
            JsonResponse jr = new JsonResponse(false, "خطا در انجام عملیات ، دوباره تلاش کنید و در صورت تکرار موضوع را گزارش کنید.");
            try
            {
                using (GatewayRepository ar = new GatewayRepository(null, true))
                {
                    
                    Gateway instance = ar.GetByID(xID);
                    new SystemLogRepository().Log(SystemLogType.BO_Gateway, "ویرایش اطلاعات درگاه-قبل از تغییر", instance.GetSerializedData(), ((Admin)(Session["Admin"])).xID);

                    instance.xActiveforLevelsAbove = args.xActiveforLevelsAbove;
                    instance.xAllowCustomAmountForBuyIn = args.xAllowCustomAmountForBuyIn;
                    instance.xCommisionPercent = args.xCommisionPercent;
                    instance.xConfig = args.xConfig;
                    instance.xIsActive = args.xIsActive;
                    instance.xIsPrimary = args.xIsPrimary;
                    instance.xIsVip = args.xIsVip;
                    instance.xMaxDailyAmount = args.xMaxDailyAmount;
                    instance.xMinBuyIn = args.xMinBuyIn;
                    instance.xName = args.xName;
                    instance.xTodayTotalTransactionAmounts = args.xTodayTotalTransactionAmounts;
                    instance.xType = args.xType;

                    new SystemLogRepository().Log(SystemLogType.BO_Gateway, "ویرایش اطلاعات درگاه-بعد از تغییر", instance.GetSerializedData(), ((Admin)(Session["Admin"])).xID);
                    ar.Update(instance);
                }
              
                jr.Message = "ذخیره با موفقیت انجام شد";
                jr.Status = true;
                return Json(jr);



            }
            catch (Exception e)
            {
                jr.Message = "خطا در انجام عملیات ، درصورت مشاهده مجدد با موضوع را اطلاع رسانی کنید.";
            }
            return Json(jr);
        }


        [HttpPost]
        [UniqueKeyDecryptor(UniqueKeyFieldName = "xID")]
        [SectionPermission(CrudOperation = CrudOperationType.Update, SectionType = AdminRoleSectionType.GatewayManagement)]
        [AjaxAntiForgeryValidateAttribute]
        public ActionResult UpdatePerfectMoneyExchangeRate(long amount)
        {
            JsonResponse jr = new JsonResponse(false, "خطا در انجام عملیات ، دوباره تلاش کنید و در صورت تکرار موضوع را گزارش کنید.");
            try
            {
                using (SettingRepository sr = new SettingRepository())
                {

                    var instance = sr.GetByID("PerfectMoneyExchangeRate");
                    new SystemLogRepository().Log(SystemLogType.BO_Setting, "ویرایش اطلاعات تنظیمات-قبل از تغییر", JsonConvert.SerializeObject(instance, SectionInfo.JsonSerializerSettings), ((Admin)(Session["Admin"])).xID);

                    instance.xValue = amount.ToString();

                    new SystemLogRepository().Log(SystemLogType.BO_Setting, "ویرایش اطلاعات تنظیمات-بعد از تغییر", JsonConvert.SerializeObject(instance, SectionInfo.JsonSerializerSettings), ((Admin)(Session["Admin"])).xID);
                    sr.Update(instance);
                }

                jr.Message = "ذخیره با موفقیت انجام شد";
                jr.Status = true;
                return Json(jr);



            }
            catch (Exception e)
            {
                jr.Message = "خطا در انجام عملیات ، درصورت مشاهده مجدد با موضوع را اطلاع رسانی کنید.";
            }
            return Json(jr);
        }

        [BasicViewBagInitializer(CurrentController = "Gateway")]
        [SectionPermission(CrudOperation = CrudOperationType.Report, SectionType = AdminRoleSectionType.GatewayManagement)]
        public string Report(int currentPage = 1)
        {
            return "این ماژول پیاده سازی نشده است";
        }


    }
}
