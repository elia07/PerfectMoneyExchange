using Saraf365.Backoffice.Filters;
using Saraf365.Core.Enumerations;
using Saraf365.Core.Repositories;
using RockCandy.Web.Framework.Core.ActionFilters;
using RockCandy.Web.Framework.Core.Enumerations;
using RockCandy.Web.Framework.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telegram.Bot;
using Telegram.Bot.Types.InputFiles;
using RockCandy.Web.Framework.Utilities;
using RockCandy.Web.Framework.Utilities.Encryption;
using System.IO;
using System.Threading.Tasks;
using Saraf365.Core.Utils;

namespace Saraf365.Backoffice.Controllers
{
    public class SendToSocialController : Controller
    {
        [HttpPost]
        [SectionPermission(CrudOperation = CrudOperationType.Insert, SectionType = AdminRoleSectionType.SendToSocial)]
        [AjaxAntiForgeryValidateAttribute]
        public async Task<ActionResult> SendPhoto(string description,long systemFileID)
        {
            JsonResponse jr = new JsonResponse(false, "خطا در انجام عملیات ، دوباره تلاش کنید و در صورت تکرار موضوع را گزارش کنید.");
            try
            {

                

                using (SystemFileRepository sfr = new SystemFileRepository())
                {
                    var sfInsatnce = sfr.GetByID(systemFileID);
                    var res = await new TelegramUtils().SendPhoto(sfInsatnce.FileData.Where(x => x.xIsThumbnail == false).Single().xData, sfInsatnce.xFileName, description, SectionInfo.Setting.TelegramBotAccessToken, SectionInfo.Setting.TelegramChannel);
                    if (res)
                    {
                        jr.Message = "ارسال با موفقیت انجام شد";
                        jr.Status = true;
                    }

                }
            }
            catch (Exception e)
            {
               
            }

            
            return Json(jr);
        }
    }
}