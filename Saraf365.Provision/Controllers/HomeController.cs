using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Saraf365.Provision.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index(string password="")
        {
            if(password=="moolmaram!!koko15koko")
            {
                return View();
            }
            else
            {
                return new RedirectResult(SectionInfo.Setting.DomainAddress);
            }

        }
        public ActionResult FireJob(string jobName, string password = "")
        {
            if (password == "moolmaram!!koko15koko")
            {
                switch (jobName)
                {
                    case "ClearSystemLogs":
                        new ClearSystemLogs().Manage();
                        break;
                    case "ClearTelegramSupport":
                        new ClearTelegramSupport().Manage();
                        break;
                    case "ResetGatewayTodayTotalTransactions":
                        new ResetGatewayTodayTotalTransactions().Manage();
                        break;
                    case "PerfectMoney":
                        new PerfectMoney().Manage();
                        break;
                    case "SettingUpdater":
                        new SettingUpdater().Manage();
                        break;
                    case "InputCardInternetBank":
                        new InputCardInternetBank().Manage();
                        break;
                    case "CardToCardManager":
                        new CardToCardManager().Manage();
                        break;
                }
                return View("/Views/Home/Index.cshtml");
            }
            else
            {
                return new RedirectResult(SectionInfo.Setting.DomainAddress);
            }
            
        }
    }
}