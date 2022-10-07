using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Saraf365.CDN.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return new RedirectResult(SectionInfo.Setting.DomainAddress);
        }

        public ActionResult Page404()
        {
            return new RedirectResult(SectionInfo.Setting.CdnAddress+"404.jpg");
        }

        public string RevokeImages()
        {
            new ImageUpdater().Manage();
            return "ok";
        }
    }
}