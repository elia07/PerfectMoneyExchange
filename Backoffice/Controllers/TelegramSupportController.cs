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
using Saraf365.Core.Utils;
using System.Threading.Tasks;

namespace Saraf365.Backoffice.Controllers
{

    [NoCache]
    [AdminAuthorize]
    public class TelegramSupportController : Controller
    {

        [BasicViewBagInitializer(CurrentController = "TelegramSupport")]
        [SectionPermission(CrudOperation = CrudOperationType.Select, SectionType = AdminRoleSectionType.TelegramSupportManagement)]
        public ActionResult Index(int currentPage = 1)
        {
            return View();
        }

       
        [HttpGet]
        [UniqueKeyDecryptor(UniqueKeyFieldName = "xID")]
        [BasicViewBagInitializer(CurrentController = "TelegramSupport")]
        [SectionPermission(CrudOperation = CrudOperationType.Update, SectionType = AdminRoleSectionType.TelegramSupportManagement)]
        public ActionResult UpdateRender(long xID = 0)
        {
            ViewBag.xID = xID;
            return View();
        }

     
        [HttpPost]
        [UniqueKeyDecryptor(UniqueKeyFieldName = "xID")]
        [SectionPermission(CrudOperation = CrudOperationType.Update, SectionType = AdminRoleSectionType.TelegramSupportManagement)]
        [AjaxAntiForgeryValidateAttribute]
        public async Task<ActionResult> Update( string response, long xID = 0)
        {
            JsonResponse jr = new JsonResponse(false, "خطا در انجام عملیات ، دوباره تلاش کنید و در صورت تکرار موضوع را گزارش کنید.");
            try
            {
                using (TelegramSupportRepository tsr = new TelegramSupportRepository(null, true))
                {
                    TelegramSupport instance = tsr.GetByID(xID);
                    instance.xResponse = response;
                    instance.xResponseDate = DateTime.Now;
                    instance.xResponseAdmin = ((Admin)(Session["Admin"])).xID;
                    var res = await new TelegramUtils().SendMessageResponse(SectionInfo.Setting.TelegramBotAccessToken, instance.xChatID, response, Convert.ToInt32(instance.xMessageID));
                    if (res)
                    {
                        jr.Message = "پاسخ فرستاده شد";
                        jr.Status = true;
                        tsr.Update(instance);
                    }
                }
                
                return Json(jr);
            }
            catch{}
            return Json(jr);
        }

       


    }
}
