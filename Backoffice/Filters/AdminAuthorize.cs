using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;

using Saraf365.Core.Enumerations;
using Saraf365.Core;
using Saraf365.Core.Repositories;
using RockCandy.Web.Framework.Core.Enumerations;
using RockCandy.Web.Framework.Core.Models;

namespace Saraf365.Backoffice.Filters
{
    public class AdminAuthorize : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session["admin"] == null)
            {
                filterContext.Result = new RedirectResult(ConfigurationManager.AppSettings.Get("DomainAddress") + "Home/AccessDenied");
                
            }
            try
            {
                filterContext.ActionParameters["admin"] = (Admin)filterContext.HttpContext.Session["admin"];
            }
            catch
            {
                filterContext.Result = new RedirectResult(ConfigurationManager.AppSettings.Get("DomainAddress") + "Home/AccessDenied");
            }
            base.OnActionExecuting(filterContext);
        }
    }
}