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
    public class AdminNotificationController : Controller
    {

        [BasicViewBagInitializer(CurrentController = "AdminNotification")]
        public ActionResult Index(int currentPage = 1)
        {
            return View();
        }

        [HttpPost]
        [AjaxAntiForgeryValidateAttribute]
        public ActionResult Insert(AdminNotification args, string xSectionPermissions)
        {

            JsonResponse jr = new JsonResponse(false, "خطا در انجام عملیات ، دوباره تلاش کنید و در صورت تکرار موضوع را گزارش کنید");
            try
            {
                using (AdminNotificationRepository opr = new AdminNotificationRepository())
                {
                    args.xDate = DateTime.Now;
                    args.xCreator = ((Admin)(Session["Admin"])).xID;
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
        [AjaxAntiForgeryValidateAttribute]
        public ActionResult Delete(long xID = 0)
        {
            JsonResponse jr = new JsonResponse(false, "خطا در انجام عملیات ، دوباره تلاش کنید و در صورت تکرار موضوع را گزارش کنید.");
            try
            {
              
                using (AdminNotificationRepository opr = new AdminNotificationRepository())
                {
                    var instance = opr.GetByID(xID);
                    if(instance.xResponsibleAdmin==((Admin)(Session["Admin"])).xID || instance.xResponsibleAdmin==null || instance.xCreator== ((Admin)(Session["Admin"])).xID)
                    {
                        opr.Delete(xID);
                        jr.Message = "حذف با موفقیت انجام شد";
                        jr.Status = true;
                    }

                    jr.Message = "امکان حذف این مورد توسط شما وجود ندارد";

                }

                
            }
            catch (Exception e)
            {
            }

            return Json(jr);
        }

        
        [HttpGet]
        [BasicViewBagInitializer(CurrentController = "AdminNotification")]        
        public ActionResult InsertRender()
        {
            return View();
        }


        [HttpGet]
        [UniqueKeyDecryptor(UniqueKeyFieldName = "xID")]
        [BasicViewBagInitializer(CurrentController = "AdminNotification")]
        public ActionResult UpdateRender(long xID = 0)
        {
            using (AdminNotificationRepository anr = new AdminNotificationRepository())
            {
                var instance = anr.GetByID(xID);
                if (instance.xResponsibleAdmin == ((Admin)(Session["Admin"])).xID || instance.xResponsibleAdmin == null || instance.xCreator == ((Admin)(Session["Admin"])).xID)
                {
                    ViewBag.xID = xID;
                    return View();
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
               
        }


        [HttpPost]
        [UniqueKeyDecryptor(UniqueKeyFieldName = "xID")]
        [AjaxAntiForgeryValidateAttribute]
        public ActionResult Update(AdminNotification args, long xID = 0)
        {
            JsonResponse jr = new JsonResponse(false, "خطا در انجام عملیات ، دوباره تلاش کنید و در صورت تکرار موضوع را گزارش کنید.");
            try
            {
                using (AdminNotificationRepository anr = new AdminNotificationRepository(null, true))
                {
                    var instance = anr.GetByID(xID);
                    if (instance.xResponsibleAdmin == ((Admin)(Session["Admin"])).xID || instance.xResponsibleAdmin == null || instance.xCreator == ((Admin)(Session["Admin"])).xID)
                    {
                        if(args.xResponsibleAdmin!=instance.xResponsibleAdmin)
                        {
                            instance.xCreator = instance.xResponsibleAdmin;
                            instance.xResponsibleAdmin = args.xResponsibleAdmin;
                        }
                        instance.xDescription = args.xDescription;
                        instance.xType = args.xType;
                        anr.Update(instance);

                        jr.Message = "ذخیره با موفقیت انجام شد";
                        jr.Status = true;
                    }
                    

                    
                }

                
                return Json(jr);



            }
            catch (Exception e)
            {
                jr.Message = "خطا در انجام عملیات ، درصورت مشاهده مجدد با موضوع را اطلاع رسانی کنید.";
            }
            return Json(jr);
        }



    }
}
