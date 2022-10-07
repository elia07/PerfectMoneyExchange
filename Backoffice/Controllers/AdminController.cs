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
    public class AdminController : Controller
    {

        [BasicViewBagInitializer(CurrentController = "Admin")]
        [SectionPermission(CrudOperation = CrudOperationType.Select, SectionType = AdminRoleSectionType.AdminManagement)]
        public ActionResult Index(int currentPage = 1)
        {
            return View();
        }

        [HttpPost]
        [SectionPermission(CrudOperation = CrudOperationType.Insert, SectionType = AdminRoleSectionType.AdminManagement)]
        [AjaxAntiForgeryValidateAttribute]
        public ActionResult Insert(Admin args, string xSectionPermissions)
        {

            JsonResponse jr = new JsonResponse(false, "ایمیل تکراری است");
            try
            {
                List<byte> xSectionPermissionsL = new List<byte>();
                if (xSectionPermissions.Length != 0)
                {
                    foreach (var item in xSectionPermissions.Split(new string[] { "," }, StringSplitOptions.None))
                    {
                        xSectionPermissionsL.Add(Convert.ToByte(item));
                    }
                }

                using (AdminRepository opr = new AdminRepository())
                {
                    args.xName = (args.xName);
                    args.xSalt = new CryptoUtils().GenerateSalt();
                    args.xPassword = new CryptoUtils().ComputeSha256Hash(args.xPassword + args.xSalt);
                    args.xIsActive = true;
                    if (args.xType != (byte)AdminType.Supervisor)
                    {
                        if (xSectionPermissionsL.Count != 0)
                        {
                            foreach (var item in xSectionPermissionsL)
                            {
                                AdminRole adminRole = new AdminRole();
                                adminRole.xCrudPermission = "11111";
                                adminRole.xSectionType = item;
                                args.AdminRole.Add(adminRole);
                            }
                        }


                    }
                    opr.Insert(args);
                }

                new SystemLogRepository().Log(SystemLogType.BO_Administration, "ایجاد مدیر",args.GetSerializedData(), ((Admin)(Session["Admin"])).xID);
                jr.Message = "درج با موفقیت انجام شد";
                jr.Status = true;
                return Json(jr);

            }
            catch
            {
                jr.Message = "ایمیل تکراری است";
            }
            return Json(jr);

        }

        [HttpPost]
        [UniqueKeyDecryptor(UniqueKeyFieldName = "xID")]
        [SectionPermission(CrudOperation = CrudOperationType.Delete, SectionType = AdminRoleSectionType.AdminManagement)]
        [AjaxAntiForgeryValidateAttribute]
        public ActionResult Delete(long xID = 0)
        {
            JsonResponse jr = new JsonResponse(false, "خطا در انجام عملیات ، دوباره تلاش کنید و در صورت تکرار موضوع را گزارش کنید.");
            try
            {


                string adminInfo = "";
                using (AdminRepository opr = new AdminRepository())
                {

                    opr.DeleteAdminAgregates(xID);
                    Admin instance = opr.GetByID(xID);
                    adminInfo = instance.xEmail + " | " + instance.xName;
                    opr.Delete(instance);
                }

                jr.Message = "حذف با موفقیت انجام شد";
                jr.Status = true;
                new SystemLogRepository().Log(SystemLogType.BO_Administration, "حذف مدیر",adminInfo, ((Admin)(Session["Admin"])).xID);
            }
            catch (Exception e)
            {
                try
                {
                    LogUtils.log(SectionInfo.LogAddress, "Backoffice Delete exception : " + e.Message + "\r\n" + e.InnerException.InnerException.Message);
                }
                catch { }
            }

            return Json(jr);
        }

        [HttpGet]
        [UniqueKeyDecryptor(UniqueKeyFieldName = "xID")]
        [BasicViewBagInitializer(CurrentController = "Admin")]
        [SectionPermission(CrudOperation = CrudOperationType.Update, SectionType = AdminRoleSectionType.AdminManagement)]
        public ActionResult UpdateRender(long xID = 0)
        {
            ViewBag.xID = xID;
            return View();
        }

        [HttpGet]
        [BasicViewBagInitializer(CurrentController = "Admin")]
        [SectionPermission(CrudOperation = CrudOperationType.Insert, SectionType = AdminRoleSectionType.AdminManagement)]
        public ActionResult InsertRender()
        {
            return View();
        }

        [HttpPost]
        [UniqueKeyDecryptor(UniqueKeyFieldName = "xID")]
        [SectionPermission(CrudOperation = CrudOperationType.Update, SectionType = AdminRoleSectionType.AdminManagement)]
        [AjaxAntiForgeryValidateAttribute]
        public ActionResult Update(Admin args, string xSectionPermissions, long xID = 0)
        {
            JsonResponse jr = new JsonResponse(false, "خطا در انجام عملیات ، دوباره تلاش کنید و در صورت تکرار موضوع را گزارش کنید.");
            string errors = "";
            try
            {



                List<byte> xSectionPermissionsL = new List<byte>();

                if (xSectionPermissions.Length != 0)
                {
                    foreach (var item in xSectionPermissions.Split(new string[] { "," }, StringSplitOptions.None))
                    {
                        xSectionPermissionsL.Add(Convert.ToByte(item));
                    }
                }





                using (AdminRepository ar = new AdminRepository(null, true))
                {
                    
                    ar.DeleteAdminAgregates(xID);
                    Admin instance = ar.GetByID(xID);

                    new SystemLogRepository().Log(SystemLogType.BO_Administration, "ویرایش اطلاعات مدیر-قبل از تغییر", instance.GetSerializedData(), ((Admin)(Session["Admin"])).xID);

                    instance.xName = (args.xName);
                    if(args.xPassword!="" && args.xPassword!=null)
                    {
                        instance.xSalt = new CryptoUtils().GenerateSalt();
                        instance.xPassword = new CryptoUtils().ComputeSha256Hash(args.xPassword + instance.xSalt);
                    }
                    
                    instance.xEmail = args.xEmail;
                    instance.xIsActive = args.xIsActive;
                    instance.xType = args.xType;
                    instance.xTelegramID = args.xTelegramID;
                    instance.xTelegramUsername = args.xTelegramUsername;

                    if (args.xType != (byte)AdminType.Supervisor)
                    {

                        if (xSectionPermissionsL.Count != 0)
                        {
                            foreach (var item in xSectionPermissionsL)
                            {
                                AdminRole adminRole = new AdminRole();
                                adminRole.xCrudPermission = "11111";
                                adminRole.xSectionType = item;
                                instance.AdminRole.Add(adminRole);
                            }
                        }

                       
                    }
                    ar.Update(instance);
                    new SystemLogRepository().Log(SystemLogType.BO_Administration, "ویرایش اطلاعات مدیر-بعد از تغییر",instance.GetSerializedData(), ((Admin)(Session["Admin"])).xID);
                }
              
                jr.Message = "ذخیره با موفقیت انجام شد";
                jr.Status = true;
                return Json(jr);



            }
            catch (Exception e)
            {
                jr.Message = "خطا در انجام عملیات ، درصورت مشاهده مجدد با موضوع را اطلاع رسانی کنید.";
            }
            return Json(jr);
        }

        [BasicViewBagInitializer(CurrentController = "Admin")]
        [SectionPermission(CrudOperation = CrudOperationType.Report, SectionType = AdminRoleSectionType.AdminManagement)]
        public string Report(int currentPage = 1)
        {
            return "این ماژول پیاده سازی نشده است";
        }





        [HttpGet]
        [UniqueKeyDecryptor(UniqueKeyFieldName = "xID")]
        [BasicViewBagInitializer(CurrentController = "Admin")]
        [SectionPermission(CrudOperation = CrudOperationType.Update, SectionType = AdminRoleSectionType.AdminManagement)]
        public ActionResult CrudPermissionUpdateRender(long xID = 0)
        {
            ViewBag.xID = xID;
            return View();
        }

        [HttpPost]
        [UniqueKeyDecryptor(UniqueKeyFieldName = "xID")]
        [SectionPermission(CrudOperation = CrudOperationType.Update, SectionType = AdminRoleSectionType.AdminManagement)]
        [AjaxAntiForgeryValidateAttribute]
        public ActionResult CrudPermissionInsert(byte xSectionType, long xAdminID)
        {

            JsonResponse jr = new JsonResponse(false, "خطا در انجام عملیات ، دوباره تلاش کنید و در صورت تکرار موضوع را گزارش کنید.");
            try
            {

              
                using (AdminRepository ar = new AdminRepository(null, true))
                {
                    ar.db.Configuration.LazyLoadingEnabled = false;
                    Admin instance = ar.GetByID(xAdminID);
                    new SystemLogRepository().Log(SystemLogType.BO_Administration, "ویرایش اطلاعات مدیر-قبل از تغییر", instance.GetSerializedData(), ((Admin)(Session["Admin"])).xID);
                    AdminRole arInstance = new AdminRole();
                    arInstance.xCrudPermission = "11111";
                    arInstance.xSectionType = xSectionType;
                    instance.AdminRole.Add(arInstance);


                    new SystemLogRepository().Log(SystemLogType.BO_Administration, "ویرایش اطلاعات مدیر-بعد از تغییر", instance.GetSerializedData(), ((Admin)(Session["Admin"])).xID);
                    ar.Update(instance);

                }


                jr.Message = "درج با موفقیت انجام شد";
                jr.Status = true;
                return Json(jr);

            }
            catch
            {
                jr.Message = "خطا در انجام عملیات ، درصورت مشاهده مجدد با موضوع را اطلاع رسانی کنید.";
            }
            return Json(jr);

        }

        [HttpPost]
        [UniqueKeyDecryptor(UniqueKeyFieldName = "xID")]
        [SectionPermission(CrudOperation = CrudOperationType.Update, SectionType = AdminRoleSectionType.AdminManagement)]
        [AjaxAntiForgeryValidateAttribute]
        public ActionResult CrudPermissionUpdate(string xCrudPermissions, long xID = 0)
        {

            JsonResponse jr = new JsonResponse(false, "خطا در انجام عملیات ، دوباره تلاش کنید و در صورت تکرار موضوع را گزارش کنید.");
            try
            {

               
                using (AdminRoleRepository arr = new AdminRoleRepository(null, true))
                {
                    AdminRole instance = arr.GetByID(xID);
                    new SystemLogRepository().Log(SystemLogType.BO_Administration, "ویرایش اطلاعات مدیر-قبل از تغییر", JsonConvert.SerializeObject(instance), ((Admin)(Session["Admin"])).xID);

                    instance.xCrudPermission = xCrudPermissions;

                    new SystemLogRepository().Log(SystemLogType.BO_Administration, "ویرایش اطلاعات مدیر-بعد از تغییر", JsonConvert.SerializeObject(instance), ((Admin)(Session["Admin"])).xID);
                    arr.Update(instance);
                }



                jr.Message = "ذخیره با موفقیت انجام شد";
                jr.Status = true;
                return Json(jr);

            }
            catch(Exception e)
            {
                jr.Message = "خطا در انجام عملیات ، درصورت مشاهده مجدد با موضوع را اطلاع رسانی کنید.";
            }
            return Json(jr);

        }

        [HttpPost]
        [UniqueKeyDecryptor(UniqueKeyFieldName = "xID")]
        [SectionPermission(CrudOperation = CrudOperationType.Update, SectionType = AdminRoleSectionType.AdminManagement)]
        [AjaxAntiForgeryValidateAttribute]
        public ActionResult CrudPermissionDelete(long xID = 0)
        {

            JsonResponse jr = new JsonResponse(false, "خطا در انجام عملیات ، دوباره تلاش کنید و در صورت تکرار موضوع را گزارش کنید.");
            try
            {
              
                using (AdminRoleRepository arr = new AdminRoleRepository(null,true))
                {


                    AdminRole instance = arr.GetByID(xID);
                  

                    arr.Delete(instance);
                    arr.Save();

                    new SystemLogRepository().Log(SystemLogType.BO_Administration, "ویرایش اطلاعات مدیر- حذف دسترسی", JsonConvert.SerializeObject(instance), ((Admin)(Session["Admin"])).xID);

                }

                jr.Message = "حذف با موفقیت انجام شد";
                jr.Status = true;
                return Json(jr);

            }
            catch
            {
                jr.Message = "خطا در انجام عملیات ، درصورت مشاهده مجدد با موضوع را اطلاع رسانی کنید.";
            }
            return Json(jr);

        }


    }
}
