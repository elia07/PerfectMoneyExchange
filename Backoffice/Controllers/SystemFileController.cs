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
using MvcReCaptcha;
using Saraf365.Backoffice;
using RockCandy.Web.Framework.Utilities.EmailUtils;
using System.Net.Mime;
using System.Text;
using Newtonsoft.Json;
using BotDetect.Web.Mvc;
using Saraf365.Backoffice.DomainUtils;
using Saraf365.Core.Utils;
using System.Threading.Tasks;

namespace Salmeg.Backoffice.Controllers
{

    [NoCache]
    [AdminAuthorize]
    public class SystemFileController : Controller
    {


        [BasicViewBagInitializer(CurrentController = "SystemFile")]
        [SectionPermission(CrudOperation = CrudOperationType.Select, SectionType = AdminRoleSectionType.SystemFileManagement)]
        public ActionResult Index(int currentPage = 1)
        {
            return View();
        }

      

        [HttpPost]
        [UniqueKeyDecryptor(UniqueKeyFieldName = "xID")]
        [SectionPermission(CrudOperation = CrudOperationType.Delete, SectionType = AdminRoleSectionType.SystemFileManagement)]
        [AjaxAntiForgeryValidateAttribute]
        public ActionResult Delete(long xID = 0)
        {
            JsonResponse jr = new JsonResponse(false, "خطا در انجام عملیات ، دوباره تلاش کنید و در صورت تکرار موضوع را گزارش کنید.");
            try
            {

                using (SystemFileRepository opr = new SystemFileRepository())
                {
                    opr.DeleteWithFileData(xID);
                }

                jr.Message = "حذف با موفقیت انجام شد";
                jr.Status = true;
            }
            catch (Exception e)
            {
                jr.Message = "امکان حذف این فایل در حال حاضر وجود ندارد";
            }

            return Json(jr);
        }

        [HttpPost]
        [SectionPermission(CrudOperation = CrudOperationType.Update, SectionType = AdminRoleSectionType.SystemFileManagement)]
        [AjaxAntiForgeryValidateAttribute]
        public async Task<ActionResult> SendMessageToSocial(string xMessage)
        {
            JsonResponse jr = new JsonResponse(false, "خطا در انجام عملیات ، دوباره تلاش کنید و در صورت تکرار موضوع را گزارش کنید.");
            try
            {

                var res = await new TelegramUtils().SendMessage(SectionInfo.Setting.TelegramBotAccessToken,SectionInfo.Setting.TelegramChannel,xMessage,SectionInfo.Setting.TelegramMessageAppendText);

                jr.Message = "حذف با موفقیت انجام شد";
                jr.Status = true;
            }
            catch (Exception e)
            {
            }

            return Json(jr);
        }

        

         [HttpPost]
        [SectionPermission(CrudOperation = CrudOperationType.Insert, SectionType = AdminRoleSectionType.SystemFileManagement)]
        [AjaxAntiForgeryValidateAttribute]
        public ActionResult UploadImage()
        {

            JsonResponse jr = new JsonResponse(false, "خطا در انجام عملیات ، دوباره تلاش کنید و در صورت تکرار موضوع را گزارش کنید.");
            try
            {
                using (SystemFileRepository sfr = new SystemFileRepository())
                {
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        HttpPostedFileBase hpf = Request.Files[i] as HttpPostedFileBase;
                        sfr.InsertImage(hpf, SectionInfo.Setting.ValidImageFormatsForBackoffice, SectionInfo.Setting.MaxFileSizeForIDCard);

                    }
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

       





    }
}
