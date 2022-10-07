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
    public class FinancialLogController : Controller
    {

        [BasicViewBagInitializer(CurrentController = "FinancialLog")]
        [SectionPermission(CrudOperation = CrudOperationType.Select, SectionType = AdminRoleSectionType.FinancialLogManagement)]
        public ActionResult Index(int currentPage = 1)
        {
            return View();
        }

        [HttpGet]
        [UniqueKeyDecryptor(UniqueKeyFieldName = "xID")]
        [BasicViewBagInitializer(CurrentController = "FinancialLog")]
        [SectionPermission(CrudOperation = CrudOperationType.Update, SectionType = AdminRoleSectionType.FinancialLogManagement)]
        public ActionResult UpdateRender(long xID = 0)
        {
            ViewBag.xID = xID;
            return View();
        }

        [HttpGet]
        [BasicViewBagInitializer(CurrentController = "FinancialLog")]
        [SectionPermission(CrudOperation = CrudOperationType.Insert, SectionType = AdminRoleSectionType.FinancialLogManagement)]
        public ActionResult InsertRender()
        {
            return View();
        }

        [HttpPost]
        [SectionPermission(CrudOperation = CrudOperationType.Insert, SectionType = AdminRoleSectionType.FinancialLogManagement)]
        [AjaxAntiForgeryValidateAttribute]
        public ActionResult Insert(FinancialLog args, string xSectionPermissions)
        {

            JsonResponse jr = new JsonResponse(false, "خطا در انجام عملیات ، دوباره تلاش کنید و در صورت تکرار موضوع را گزارش کنید");
            try
            {
                if (!(args.xType == (byte)FinancialLogType.ShareHolderWithdrawal || args.xType == (byte)FinancialLogType.MiscCost))
                {
                    return Json(jr);
                }

                using (FinancialLogRepository flr = new FinancialLogRepository())
                {
                    var admin = (Admin)(Session["Admin"]);
                    args.xAddBy= admin.xEmail+" | "+admin.xID;
                    args.xDate = DateTime.Now;
                    args.xType = args.xType;

                    flr.Insert(args);
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

        [BasicViewBagInitializer(CurrentController = "FinancialLog")]
        [SectionPermission(CrudOperation = CrudOperationType.Report, SectionType = AdminRoleSectionType.FinancialLogManagement)]
        public string Report(int currentPage = 1)
        {
            return "این ماژول پیاده سازی نشده است";
        }


    }
}
