using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
//using System.Web.Http.Filters;
using System.Web.Mvc;

using Saraf365.Core.Enumerations;
using Saraf365.Core;
using Saraf365.Core.Repositories;
using RockCandy.Web.Framework.Core.Enumerations;
using RockCandy.Web.Framework.Core.Models;

namespace Saraf365.Backoffice.Filters
{
    public class SectionPermission : ActionFilterAttribute
    {
        public AdminRoleSectionType SectionType { get; set; }
        public CrudOperationType CrudOperation { set; get; }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            try
            {
                Admin admin;
                using (AdminRepository ur = new AdminRepository())
                {
                    admin = ur.GetByID(((Admin)filterContext.HttpContext.Session["Admin"]).xID);

                    AdminRole adminRole = admin.AdminRole.Where(r => r.xSectionType == (byte)SectionType).SingleOrDefault();
                    char[] AdminRoleCrudPermission = new char[5];
                    if (adminRole != null)
                    {
                        AdminRoleCrudPermission = adminRole.xCrudPermission.ToCharArray();
                    }
                    bool hasPermission = false;
                    if (admin.xType == (byte)AdminType.Supervisor || (adminRole != null && AdminRoleCrudPermission[(byte)CrudOperation] == '1'))
                    {
                        hasPermission = true;
                    }
                    if (!hasPermission)
                    {
                        filterContext.Result = new JsonResult
                        {
                            Data = new JsonResponse(false, string.Format("اجازه دسترسی به این قسمت را ندارید<script type='text/javascript'>window.location ='/Home/AccessDenied'</script>")),
                            ContentEncoding = System.Text.Encoding.UTF8,
                            ContentType = "application/json",
                            JsonRequestBehavior = JsonRequestBehavior.AllowGet
                        };
                    }
                }


            }
            catch
            {
                filterContext.Result = new JsonResult
                {
                    Data = new JsonResponse(false, string.Format("اجازه دسترسی به این قسمت را ندارید<script type='text/javascript'>window.location ='/Home/AccessDenied'</script>")),
                    ContentEncoding = System.Text.Encoding.UTF8,
                    ContentType = "application/json",
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            base.OnActionExecuting(filterContext);
        }
    }
}