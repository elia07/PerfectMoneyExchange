using Newtonsoft.Json;
using Saraf365.Core;
using Saraf365.Core.Repositories;
using Saraf365.WebClient.Utils;
using RockCandy.Web.Framework.Core.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Saraf365.WebClient.Filters
{
    public class UserAuthorize : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {


            long userID = new SessionUtils().GetSessionValue<long>(filterContext.HttpContext.Session, SectionInfo.UserSessionName);
            if( userID == 0)
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    JsonResponse jr = new JsonResponse(false, "");
                    jr.Message = "There is something wrong , please try again or Contact administrator";
                    jr.CustomFields.Add("title", "Something went wrong");
                    jr.CustomFields.Add("confirmButtonText", "ok");
                    jr.CustomFields.Add("Redirect", SectionInfo.Setting.DomainAddress);
                    //filterContext.HttpContext.Response.Write(JsonConvert.SerializeObject(jr));
                    filterContext.Result = new JsonResult {Data= jr ,JsonRequestBehavior=JsonRequestBehavior.AllowGet};
                }
                else
                {
                    filterContext.Result = new RedirectResult(SectionInfo.Setting.DomainAddress + "Home/Index");
                }
                

            }
            /*try
            {
                filterContext.ActionParameters["User"] = new UserRepository().GetByID(userID);
            }
            catch
            {
                filterContext.Result = new RedirectResult(ConfigurationManager.AppSettings.Get("DomainAddress") + "Home/Index");
            }*/
            filterContext.Controller.ViewBag.UserID = userID;
            base.OnActionExecuting(filterContext);
        }
    }
}