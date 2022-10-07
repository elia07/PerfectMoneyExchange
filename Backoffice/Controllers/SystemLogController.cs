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
    public class SystemLogController : Controller
    {

        [BasicViewBagInitializer(CurrentController = "SystemLog")]
        [SectionPermission(CrudOperation = CrudOperationType.Select, SectionType = AdminRoleSectionType.SystemLogManagement)]
        public ActionResult Index(int currentPage = 1)
        {
            return View();
        }


        [HttpGet]
        [UniqueKeyDecryptor(UniqueKeyFieldName = "xID")]
        [BasicViewBagInitializer(CurrentController = "SystemLog")]
        [SectionPermission(CrudOperation = CrudOperationType.Update, SectionType = AdminRoleSectionType.SystemLogManagement)]
        public ActionResult UpdateRender(long xID = 0)
        {
            ViewBag.xID = xID;
            return View();
        }

        [BasicViewBagInitializer(CurrentController = "SystemLog")]
        [SectionPermission(CrudOperation = CrudOperationType.Report, SectionType = AdminRoleSectionType.SystemLogManagement)]
        public string Report(int currentPage = 1)
        {
            return "این ماژول پیاده سازی نشده است";
        }


    }
}
