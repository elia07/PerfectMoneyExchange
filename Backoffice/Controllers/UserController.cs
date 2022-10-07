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
using System.Threading.Tasks;
using Saraf365.Core.Utils;

namespace Saraf365.Backoffice.Controllers
{

    [NoCache]
    [AdminAuthorize]
    public class UserController : Controller
    {

        [BasicViewBagInitializer(CurrentController = "User")]
        [SectionPermission(CrudOperation = CrudOperationType.Select, SectionType = AdminRoleSectionType.UserManagement)]
        public ActionResult Index(int currentPage = 1)
        {
            return View();
        }

      

        [HttpGet]
        [UniqueKeyDecryptor(UniqueKeyFieldName = "xID")]
        [BasicViewBagInitializer(CurrentController = "User")]
        [SectionPermission(CrudOperation = CrudOperationType.Update, SectionType = AdminRoleSectionType.UserManagement)]
        public ActionResult UpdateRender(long xID = 0)
        {
            ViewBag.xID = xID;
            return View();
        }


        [HttpPost]
        [UniqueKeyDecryptor(UniqueKeyFieldName = "xID")]
        [SectionPermission(CrudOperation = CrudOperationType.Update, SectionType = AdminRoleSectionType.UserManagement)]
        [AjaxAntiForgeryValidateAttribute]
        public ActionResult Update(User args, long xID = 0)
        {
            JsonResponse jr = new JsonResponse(false, "خطا در انجام عملیات ، دوباره تلاش کنید و در صورت تکرار موضوع را گزارش کنید.");
            try
            {
                using (UserRepository ar = new UserRepository(null, true))
                {
                    User instance = ar.GetByID(xID);
                    new SystemLogRepository().Log(SystemLogType.BO_User, "ویرایش اطلاعات کاربر-قبل از تغییر", instance.GetSerializedData(), ((Admin)(Session["Admin"])).xID);

                    instance.xIsActive = args.xIsActive;
                    instance.xDescription = args.xDescription;
                    instance.xIsVip = args.xIsVip;
                    instance.xIPGRestriction = args.xIPGRestriction;
                    if (Convert.ToDateTime(instance.xIPGRestriction) <= DateTime.Now)
                    {
                        instance.xIPGRestriction = null;
                    }
                    instance.xIsNationalIDValidated = args.xIsNationalIDValidated;
                    instance.xNationalIDNumber = args.xNationalIDNumber;
                    instance.xIsEmailValidated = args.xIsEmailValidated;
                    instance.xCellphoneActivated = args.xCellphoneActivated;
                    instance.xWallet = args.xWallet;
                    instance.xRefferalSharePercent = args.xRefferalSharePercent;


                    new SystemLogRepository().Log(SystemLogType.BO_User, "ویرایش اطلاعات کاربر-بعد از تغییر", instance.GetSerializedData(), ((Admin)(Session["Admin"])).xID);
                    ar.Update(instance);
                }
              
                jr.Message = "ذخیره با موفقیت انجام شد";
                jr.Status = true;
                return Json(jr);



            }
            catch{
                jr.Message = "شماره ملی تکراری است";
            }
            return Json(jr);
        }



        [HttpPost]
        [SectionPermission(CrudOperation = CrudOperationType.Update, SectionType = AdminRoleSectionType.UserManagement)]
        [AjaxAntiForgeryValidateAttribute]
        public ActionResult VerifyBankAccount(long bankAccountID)
        {
            JsonResponse jr = new JsonResponse(false, "خطا در انجام عملیات ، دوباره تلاش کنید و در صورت تکرار موضوع را گزارش کنید.");
            try
            {
                using (UserBankAccountRepository ar = new UserBankAccountRepository(null, true))
                {
                    var instance = ar.GetByID(bankAccountID);
                    new SystemLogRepository().Log(SystemLogType.BO_User, "تایید حساب کاربر", instance.xCartNumber, ((Admin)(Session["Admin"])).xID);

                    instance.xIsVerified = true;
                    ar.Update(instance);
                }

                jr.Message = "ذخیره با موفقیت انجام شد";
                jr.Status = true;
                return Json(jr);



            }
            catch
            {
                jr.Message = "شماره ملی تکراری است";
            }
            return Json(jr);
        }
        [HttpPost]
        [SectionPermission(CrudOperation = CrudOperationType.Update, SectionType = AdminRoleSectionType.UserManagement)]
        [AjaxAntiForgeryValidateAttribute]
        public ActionResult DeleteBankAccount(long bankAccountID)
        {
            JsonResponse jr = new JsonResponse(false, "خطا در انجام عملیات ، دوباره تلاش کنید و در صورت تکرار موضوع را گزارش کنید.");
            try
            {
                using (UserBankAccountRepository ar = new UserBankAccountRepository(null, true))
                {
                    var instance = ar.GetByID(bankAccountID);
                    new SystemLogRepository().Log(SystemLogType.BO_User, "حذف حساب کاربر", instance.xCartNumber, ((Admin)(Session["Admin"])).xID);
                    ar.Delete(bankAccountID);
                }

                jr.Message = "ذخیره با موفقیت انجام شد";
                jr.Status = true;
                return Json(jr);



            }
            catch
            {
                jr.Message = "شماره ملی تکراری است";
            }
            return Json(jr);
        }


        [HttpPost]
        [UniqueKeyDecryptor(UniqueKeyFieldName = "xID")]
        [SectionPermission(CrudOperation = CrudOperationType.Update, SectionType = AdminRoleSectionType.UserManagement)]
        [AjaxAntiForgeryValidateAttribute]
        public ActionResult DeleteNationalIdImage(long xID = 0)
        {
            JsonResponse jr = new JsonResponse(false, "خطا در انجام عملیات ، دوباره تلاش کنید و در صورت تکرار موضوع را گزارش کنید.");
            try
            {
                long nationalImageID = 0;
                using (UserRepository ar = new UserRepository(null, true))
                {
                    User instance = ar.GetByIDAndLoadNationaIDCard(xID);
                    new SystemLogRepository().Log(SystemLogType.BO_User, "ویرایش اطلاعات کاربر-قبل از تغییر", instance.GetSerializedData(), ((Admin)(Session["Admin"])).xID);
                    if(instance.xNationalIDImage!=null)
                    {
                        nationalImageID = Convert.ToInt64(instance.xNationalIDImage);
                        instance.xNationalIDImage = null;
                        instance.xIsNationalIDValidated = false;
                    }
                    new SystemLogRepository().Log(SystemLogType.BO_User, "ویرایش اطلاعات کاربر-بعد از تغییر", instance.GetSerializedData(), ((Admin)(Session["Admin"])).xID);
                    ar.Update(instance);
                }
                try
                {
                    using (SystemFileRepository sfr = new SystemFileRepository())
                    {
                        sfr.DeleteWithFileData(nationalImageID);
                    }
                }
                catch { }
                

                jr.Message = "ذخیره با موفقیت انجام شد";
                jr.Status = true;
                return Json(jr);



            }
            catch(Exception e)
            {
                jr.Message = "شماره ملی تکراری است";
            }
            return Json(jr);
        }







        [BasicViewBagInitializer(CurrentController = "User")]
        [SectionPermission(CrudOperation = CrudOperationType.Report, SectionType = AdminRoleSectionType.UserManagement)]
        public string Report(int currentPage = 1)
        {
            return "این ماژول پیاده سازی نشده است";
        }






    }
}
