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
using System.Threading.Tasks;

namespace Saraf365.Backoffice.Controllers
{

    [NoCache]
    [AdminAuthorize]
    public class SettingController : Controller
    {

        [BasicViewBagInitializer(CurrentController = "Setting")]
        [SectionPermission(CrudOperation = CrudOperationType.Select, SectionType = AdminRoleSectionType.SettingManagement)]
        public async Task<ActionResult> Index(int currentPage = 1)
        {
            return View();
        }

       
        [HttpGet]
        [UniqueKeyDecryptor(UniqueKeyFieldName = "xID")]
        [BasicViewBagInitializer(CurrentController = "Setting")]
        [SectionPermission(CrudOperation = CrudOperationType.Update, SectionType = AdminRoleSectionType.SettingManagement)]
        public ActionResult UpdateRender(long xID = 0)
        {
            ViewBag.xID = xID;
            return View();
        }

     
        [HttpPost]
        [UniqueKeyDecryptor(UniqueKeyFieldName = "xID")]
        [SectionPermission(CrudOperation = CrudOperationType.Update, SectionType = AdminRoleSectionType.SettingManagement)]
        [AjaxAntiForgeryValidateAttribute]
        public ActionResult Update(Setting args, long xID = 0)
        {
            JsonResponse jr = new JsonResponse(false, "خطا در انجام عملیات ، دوباره تلاش کنید و در صورت تکرار موضوع را گزارش کنید.");
            try
            {
                using (SettingRepository sr = new SettingRepository())
                {
                    Setting instance = sr.GetByID(xID);

                    new SystemLogRepository().Log(SystemLogType.BO_Setting, "ویرایش اطلاعات تنظیمات-قبل از تغییر", JsonConvert.SerializeObject(instance, SectionInfo.JsonSerializerSettings), ((Admin)(Session["Admin"])).xID);

                    instance.xValue = args.xValue==null?"false": args.xValue;

                    new SystemLogRepository().Log(SystemLogType.BO_Setting, "ویرایش اطلاعات تنظیمات-بعد از تغییر", JsonConvert.SerializeObject(instance, SectionInfo.JsonSerializerSettings), ((Admin)(Session["Admin"])).xID);
                    sr.Update(instance);
                }
                

                jr.Message = "ذخیره با موفقیت انجام شد";
                jr.Status = true;
                return Json(jr);



            }
            catch{}
            return Json(jr);
        }





    }
}
