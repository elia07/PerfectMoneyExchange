using Saraf365.Core;
using Saraf365.WebClient.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Saraf365.WebClient.Filters
{
    public class ViewBagInitalizer : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {


            var cu = new CookieUtils();


            string LanguageCode = cu.GetCookieValue<string>("LanguageCode", filterContext.RequestContext.HttpContext.Request);
            if (LanguageCode == "" || LanguageCode ==null)
            {
                LanguageCode = SectionInfo.Setting.DefaultLanguage;
                filterContext.Controller.ViewBag.LanguageCode = SectionInfo.Setting.DefaultLanguage;
                cu.AddCookie("LanguageCode", SectionInfo.Setting.DefaultLanguage, Int32.MaxValue, filterContext.HttpContext);
            }
            else
            {
                filterContext.Controller.ViewBag.LanguageCode = LanguageCode;
            }

            if (!cu.GetCookieValue<bool>("cookieEnabled", filterContext.RequestContext.HttpContext.Request))
            {
                filterContext.Controller.ViewBag.Message = SectionInfo.Setting.Languages[LanguageCode]["Txt_PleaseEnableCookieInyourBrowsser"];
                filterContext.Controller.ViewBag.Title = SectionInfo.Setting.Languages[LanguageCode]["Txt_Error"];
                filterContext.Controller.ViewBag.ConfirmButtonText = SectionInfo.Setting.Languages[LanguageCode]["Txt_Ok"];
                filterContext.Controller.ViewBag.Type = "warning";
                if (SectionInfo.Setting.MaintenanceMode && !(filterContext.ActionDescriptor.ControllerDescriptor.ControllerName == "Home" && filterContext.ActionDescriptor.ActionName == "Notify"))
                {
                    filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary
                    {
                    { "controller", "Home" },
                    { "action", "Notify" }
                    });
                }
            }


            //filterContext.Controller.ViewBag.MaintenanceMessage = SectionInfo.Setting.MaintenanceMessage;
            if (SectionInfo.Setting.MaintenanceMode && !(filterContext.ActionDescriptor.ControllerDescriptor.ControllerName=="Home" && filterContext.ActionDescriptor.ActionName== "MaintenanceMode"))
            {
                filterContext.Result = new RedirectToRouteResult(
                new RouteValueDictionary
                {
                    { "controller", "Home" },
                    { "action", "MaintenanceMode" }
                });
            }

            base.OnActionExecuting(filterContext);
        }
    }
}