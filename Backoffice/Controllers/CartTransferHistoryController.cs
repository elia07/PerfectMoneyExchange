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
    public class CartTransferHistoryController : Controller
    {

        [BasicViewBagInitializer(CurrentController = "CartTransferHistory")]
        [SectionPermission(CrudOperation = CrudOperationType.Select, SectionType = AdminRoleSectionType.CartTransferHistoryManagement)]
        public ActionResult Index(int currentPage = 1)
        {
            return View();
        }
        [HttpPost]
        [SectionPermission(CrudOperation = CrudOperationType.Insert, SectionType = AdminRoleSectionType.CartTransferHistoryManagement)]
        [AjaxAntiForgeryValidateAttribute]
        public ActionResult Insert(CartTransferHistory args)
        {

            JsonResponse jr = new JsonResponse(false, "خطا در انجام عملیات ، مجددا تلاش کرده و در صورت تکرار موضوع را گزارش کنید");
            try
            {
                using (CartTransferHistoryRepository opr = new CartTransferHistoryRepository())
                {
                    args.xDate = DateTime.Now;
                    args.xDocumentDate = DateTime.Now;
                    args.xAmountOut = 0;
                    args.xCodePeigiri = "دستی";
                    args.xCodeErja = "";
                    args.xDescription = "";
                    args.xDocumentNumber = new Random().Next(1000000, 9999999).ToString();

                    opr.Insert(args);
                }
                new SystemLogRepository().Log(SystemLogType.CardTransferHistory, "ثبت دستی کارت به کارت",JsonConvert.SerializeObject(args), ((Admin)(Session["Admin"])).xID);
                jr.Message = "درج با موفقیت انجام شد";
                jr.Status = true;
                return Json(jr);

            }
            catch
            {
            }
            return Json(jr);

        }

        [HttpGet]
        [BasicViewBagInitializer(CurrentController = "CartTransferHistory")]
        [SectionPermission(CrudOperation = CrudOperationType.Insert, SectionType = AdminRoleSectionType.CartTransferHistoryManagement)]
        public ActionResult InsertRender()
        {
            return View();
        }

        [HttpPost]
        [UniqueKeyDecryptor(UniqueKeyFieldName = "xID")]
        [SectionPermission(CrudOperation = CrudOperationType.Delete, SectionType = AdminRoleSectionType.CartTransferHistoryManagement)]
        [AjaxAntiForgeryValidateAttribute]
        public ActionResult Delete(long xID = 0)
        {
            JsonResponse jr = new JsonResponse(false, "خطا در انجام عملیات ، دوباره تلاش کنید و در صورت تکرار موضوع را گزارش کنید.");
            try
            {
                string shomareSanad = "";
                using (CartTransferHistoryRepository opr = new CartTransferHistoryRepository())
                {
                    var instance = opr.GetByID(xID);
                    if(instance.Transaction==null && instance.xAmountOut==0)
                    {
                        shomareSanad = instance.xDocumentNumber;
                        opr.Delete(xID);
                        new SystemLogRepository().Log(SystemLogType.CardTransferHistory, "حذف کارت به کارت", shomareSanad, ((Admin)(Session["Admin"])).xID);
                    }
                    
                    
                }
                
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
        [BasicViewBagInitializer(CurrentController = "CartTransferHistory")]
        [SectionPermission(CrudOperation = CrudOperationType.Update, SectionType = AdminRoleSectionType.CartTransferHistoryManagement)]
        public ActionResult UpdateRender(long xID = 0)
        {
            ViewBag.xID = xID;
            return View();
        }


        [HttpPost]
        [UniqueKeyDecryptor(UniqueKeyFieldName = "xID")]
        [SectionPermission(CrudOperation = CrudOperationType.Update, SectionType = AdminRoleSectionType.CartTransferHistoryManagement)]
        [AjaxAntiForgeryValidateAttribute]
        public ActionResult Update(CartTransferHistory args, long xID = 0)
        {
            JsonResponse jr = new JsonResponse(false, "خطا در انجام عملیات ، دوباره تلاش کنید و در صورت تکرار موضوع را گزارش کنید.");
            try
            {
                using (CartTransferHistoryRepository cthr = new CartTransferHistoryRepository(null, true))
                {

                    CartTransferHistory instance = cthr.GetByID(xID);
                    if(string.IsNullOrEmpty(instance.xCardNumber) && instance.Transaction==null)
                    {
                        new SystemLogRepository().Log(SystemLogType.CardTransferHistory, "اصلاح شماره کارت قبل",JsonConvert.SerializeObject(instance), ((Admin)(Session["Admin"])).xID);
                        instance.xCardNumber = args.xCardNumber;

                        cthr.Update(instance);
                        new SystemLogRepository().Log(SystemLogType.CardTransferHistory, "اصلاح شماره کارت بعد", JsonConvert.SerializeObject(instance), ((Admin)(Session["Admin"])).xID);
                    }
                    
                    
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


        [BasicViewBagInitializer(CurrentController = "CartTransferHistory")]
        [SectionPermission(CrudOperation = CrudOperationType.Report, SectionType = AdminRoleSectionType.CartTransferHistoryManagement)]
        public string Report(int currentPage = 1)
        {
            return "این ماژول پیاده سازی نشده است";
        }


    }
}
