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

namespace Saraf365.Backoffice.Controllers
{

    [NoCache]
    [AdminAuthorize]
    public class TicketController : Controller
    {

        [BasicViewBagInitializer(CurrentController = "Ticket")]
        [SectionPermission(CrudOperation = CrudOperationType.Select, SectionType = AdminRoleSectionType.TicketManagement)]
        public ActionResult Index(int currentPage = 1)
        {
            return View();
        }

       
        [HttpGet]
        [UniqueKeyDecryptor(UniqueKeyFieldName = "xID")]
        [BasicViewBagInitializer(CurrentController = "Ticket")]
        [SectionPermission(CrudOperation = CrudOperationType.Update, SectionType = AdminRoleSectionType.TicketManagement)]
        public ActionResult UpdateRender(long xID = 0)
        {
            ViewBag.xID = xID;
            return View();
        }

     
        [HttpPost]
        [UniqueKeyDecryptor(UniqueKeyFieldName = "xID")]
        [SectionPermission(CrudOperation = CrudOperationType.Update, SectionType = AdminRoleSectionType.TicketManagement)]
        [AjaxAntiForgeryValidateAttribute]
        public ActionResult Update( string response, long xID = 0)
        {
            JsonResponse jr = new JsonResponse(false, "خطا در انجام عملیات ، دوباره تلاش کنید و در صورت تکرار موضوع را گزارش کنید.");
            try
            {
                using (ConversationRepository cr = new ConversationRepository(null, true))
                {
                    Conversation instance = cr.GetByIdAndLoadTicket(xID);
                    instance.xResponse = response;
                    instance.xResponseDate = DateTime.Now;
                    instance.Ticket.xStatus = (byte)TicketStatusType.Responded;
                    cr.Update(instance);
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
