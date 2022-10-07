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
    public class ScheduleMessageController : Controller
    {

        [BasicViewBagInitializer(CurrentController = "ScheduleMessage")]
        [SectionPermission(CrudOperation = CrudOperationType.Select, SectionType = AdminRoleSectionType.ScheduleMessageManagement)]
        public ActionResult Index(int currentPage = 1)
        {
            return View();
        }

        [HttpPost]
        [SectionPermission(CrudOperation = CrudOperationType.Insert, SectionType = AdminRoleSectionType.ScheduleMessageManagement)]
        [AjaxAntiForgeryValidateAttribute]
        public ActionResult Insert(ScheduleMessage args, string xSectionPermissions)
        {

            JsonResponse jr = new JsonResponse(false, "خطا در انجام عملیات ، مجددا تلاش کرده و در صورت تکرار موضوع را گزارش کنید");
            try
            {


                using (ScheduleMessageRepository opr = new ScheduleMessageRepository())
                {
                    args.xIsActive = true;
                    opr.Insert(args);
                }
              
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
        [SectionPermission(CrudOperation = CrudOperationType.Delete, SectionType = AdminRoleSectionType.ScheduleMessageManagement)]
        [AjaxAntiForgeryValidateAttribute]
        public ActionResult Delete(long xID = 0)
        {
            JsonResponse jr = new JsonResponse(false, "خطا در انجام عملیات ، دوباره تلاش کنید و در صورت تکرار موضوع را گزارش کنید.");
            try
            {
              
                using (ScheduleMessageRepository opr = new ScheduleMessageRepository())
                {
                    opr.Delete(xID);
                }

                jr.Message = "حذف با موفقیت انجام شد";
                jr.Status = true;
            }
            catch (Exception e)
            {
            }

            return Json(jr);
        }

        [HttpGet]
        [UniqueKeyDecryptor(UniqueKeyFieldName = "xID")]
        [BasicViewBagInitializer(CurrentController = "ScheduleMessage")]
        [SectionPermission(CrudOperation = CrudOperationType.Update, SectionType = AdminRoleSectionType.ScheduleMessageManagement)]
        public ActionResult UpdateRender(long xID = 0)
        {
            ViewBag.xID = xID;
            return View();
        }

        [HttpGet]
        [BasicViewBagInitializer(CurrentController = "ScheduleMessage")]
        [SectionPermission(CrudOperation = CrudOperationType.Insert, SectionType = AdminRoleSectionType.ScheduleMessageManagement)]
        public ActionResult InsertRender()
        {
            return View();
        }

        [HttpPost]
        [UniqueKeyDecryptor(UniqueKeyFieldName = "xID")]
        [SectionPermission(CrudOperation = CrudOperationType.Update, SectionType = AdminRoleSectionType.ScheduleMessageManagement)]
        [AjaxAntiForgeryValidateAttribute]
        public ActionResult Update(ScheduleMessage args, long xID = 0)
        {
            JsonResponse jr = new JsonResponse(false, "خطا در انجام عملیات ، دوباره تلاش کنید و در صورت تکرار موضوع را گزارش کنید.");
            try
            {
                using (ScheduleMessageRepository ar = new ScheduleMessageRepository())
                {
                    
                    ScheduleMessage instance = ar.GetByID(xID);

                    
                    instance.xSystemFileID = args.xSystemFileID;
                    instance.xTitle = args.xTitle;
                    instance.xBody = args.xBody;
                    instance.xExpression = args.xExpression;
                    instance.xIsActive = args.xIsActive;
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

        [BasicViewBagInitializer(CurrentController = "ScheduleMessage")]
        [SectionPermission(CrudOperation = CrudOperationType.Report, SectionType = AdminRoleSectionType.ScheduleMessageManagement)]
        public string Report(int currentPage = 1)
        {
            return "این ماژول پیاده سازی نشده است";
        }


    }
}
