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
using Saraf365.Core.InternetBankModules;

namespace Saraf365.Backoffice.Controllers
{

    [NoCache]
    [AdminAuthorize]
    public class BankCardController : Controller
    {

        [BasicViewBagInitializer(CurrentController = "BankCard")]
        [SectionPermission(CrudOperation = CrudOperationType.Select, SectionType = AdminRoleSectionType.BankCardManagement)]
        public ActionResult Index(int currentPage = 1)
        {
            return View();
        }

        [HttpPost]
        [SectionPermission(CrudOperation = CrudOperationType.Insert, SectionType = AdminRoleSectionType.BankCardManagement)]
        [AjaxAntiForgeryValidateAttribute]
        public ActionResult Insert(BankCard args)
        {

            JsonResponse jr = new JsonResponse(false, "خطا در انجام عملیات ، مجددا تلاش کرده و در صورت تکرار موضوع را گزارش کنید");
            try
            {


                using (BankCardRepository opr = new BankCardRepository())
                {
                    args.xDepositStatus = "";
                    opr.Insert(args);
                }
                new SystemLogRepository().Log(SystemLogType.BO_BankCard, "ایجاد کارت بانکی", JsonConvert.SerializeObject(args,SectionInfo.JsonSerializerSettings), ((Admin)(Session["Admin"])).xID);
                jr.Message = "درج با موفقیت انجام شد";
                jr.Status = true;
                return Json(jr);

            }
            catch
            {
            }
            return Json(jr);

        }

        [HttpPost]
        [UniqueKeyDecryptor(UniqueKeyFieldName = "xID")]
        [SectionPermission(CrudOperation = CrudOperationType.Delete, SectionType = AdminRoleSectionType.BankCardManagement)]
        [AjaxAntiForgeryValidateAttribute]
        public ActionResult Delete(long xID = 0)
        {
            JsonResponse jr = new JsonResponse(false, "خطا در انجام عملیات ، دوباره تلاش کنید و در صورت تکرار موضوع را گزارش کنید.");
            try
            {
                string xAccountNumber = "";
                using (BankCardRepository opr = new BankCardRepository())
                {
                    var instance = opr.GetByID(xID);
                    xAccountNumber = instance.xAccountNumber ;
                    opr.Delete(xID);
                }
                new SystemLogRepository().Log(SystemLogType.BO_BankCard, "حذف کارت بانکی", JsonConvert.SerializeObject(xAccountNumber, SectionInfo.JsonSerializerSettings), ((Admin)(Session["Admin"])).xID);
                jr.Message = "حذف با موفقیت انجام شد";
                jr.Status = true;
            }
            catch (Exception e)
            {
                jr.Message = "حذف این کارت بانکی امکام پذیر نیست ، در صورت تمایل آنرا غیر فعال کنید";
            }

            return Json(jr);
        }

        [HttpGet]
        [UniqueKeyDecryptor(UniqueKeyFieldName = "xID")]
        [BasicViewBagInitializer(CurrentController = "BankCard")]
        [SectionPermission(CrudOperation = CrudOperationType.Update, SectionType = AdminRoleSectionType.BankCardManagement)]
        public ActionResult UpdateRender(long xID = 0)
        {
            ViewBag.xID = xID;
            return View();
        }

        [HttpGet]
        [BasicViewBagInitializer(CurrentController = "BankCard")]
        [SectionPermission(CrudOperation = CrudOperationType.Insert, SectionType = AdminRoleSectionType.BankCardManagement)]
        public ActionResult InsertRender()
        {
            return View();
        }

        [HttpPost]
        [UniqueKeyDecryptor(UniqueKeyFieldName = "xID")]
        [SectionPermission(CrudOperation = CrudOperationType.Update, SectionType = AdminRoleSectionType.BankCardManagement)]
        [AjaxAntiForgeryValidateAttribute]
        public ActionResult Update(BankCard args, long xID = 0)
        {
            JsonResponse jr = new JsonResponse(false, "خطا در انجام عملیات ، دوباره تلاش کنید و در صورت تکرار موضوع را گزارش کنید.");
            try
            {
                using (BankCardRepository ar = new BankCardRepository())
                {
                    
                    BankCard instance = ar.GetByID(xID);
                    new SystemLogRepository().Log(SystemLogType.BO_BankCard, "ویرایش اطلاعات کارت بانکی-قبل از تغییر", JsonConvert.SerializeObject(instance, SectionInfo.JsonSerializerSettings), ((Admin)(Session["Admin"])).xID);

                    instance.xType = args.xType;
                    instance.xBankName = args.xBankName;
                    instance.xAccountNumber = args.xAccountNumber;
                    instance.xCardNumber = args.xCardNumber;
                    instance.xInternetUsername = args.xInternetUsername;
                    instance.xInternetPassword = args.xInternetPassword;
                    instance.xMinToTransfer = args.xMinToTransfer;
                    instance.xSecondPassword = args.xSecondPassword;
                    instance.xCvv2 = args.xCvv2;
                    instance.xExpireMonth = args.xExpireMonth;
                    instance.xExpireYear = args.xExpireYear;
                    instance.xMinDeposit = args.xMinDeposit;
                    instance.xCardHolderName = args.xCardHolderName;
                    instance.xIsActive = args.xIsActive;
                    instance.xSpecialConfigs = args.xSpecialConfigs;

                    new SystemLogRepository().Log(SystemLogType.BO_BankCard, "ویرایش اطلاعات کارت بانکی-بعد از تغییر", JsonConvert.SerializeObject(instance, SectionInfo.JsonSerializerSettings), ((Admin)(Session["Admin"])).xID);
                    ar.Update(instance);
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


        [HttpPost]
        [SectionPermission(CrudOperation = CrudOperationType.Update, SectionType = AdminRoleSectionType.BankCardManagement)]
        [AjaxAntiForgeryValidateAttribute]
        public ActionResult CardToCard(long id, long toID, long amount)
        {
            JsonResponse jr = new JsonResponse(false, "خطا در انجام عملیات ، دوباره تلاش کنید و در صورت تکرار موضوع را گزارش کنید.");
            if(amount>3000000)
            {
                jr.Message = "سقف کارت به کارت 3 میلیون تومان می باشد";
                return Json(jr);
            }
            try
            {
                using (BankCardRepository bcr = new BankCardRepository(null, true))
                {
                    var instance = bcr.GetByID(id);
                    if (!SectionInfo.InternetBanks.ContainsKey(id))
                    {
                        SectionInfo.InternetBanks.Add(id, new InternetBankFactory().GetInstance(bcr.GetByID(id), SectionInfo.Setting.AntiCaptchaApiKey));
                    }

                    var balanceStatus = SectionInfo.InternetBanks[id].GetBalance();
                    instance.xBalance = balanceStatus.Item1;



                    
                    if (instance.xBalance - Convert.ToInt64(instance.xMinDeposit) > amount * 10)
                    {
                        jr.Message = "موجودی ناکافی";
                        return Json(jr);
                    }
                    var destCard = bcr.GetByID(toID);
                    var res = SectionInfo.InternetBanks[id].CardTransfer(destCard.xCardNumber, amount*10);
                    new SystemLogRepository().Log(SystemLogType.InternetBank, "AdminPanel Card Transfer", "From card : " + instance.xCardNumber + " | To card : " + destCard.xCardNumber + " | Amount : " + (amount*10) + " | Res:\r\n" + JsonConvert.SerializeObject(res), ((Admin)(Session["Admin"])).xID);

                    balanceStatus = SectionInfo.InternetBanks[id].GetBalance();
                    instance.xBalance = balanceStatus.Item1;
                    instance.xBannedAmount = balanceStatus.Item2;
                    instance.xDepositStatus = balanceStatus.Item3;

                    bcr.Update(instance);

                    jr.Message = instance.xBalance + " | " + instance.xDepositStatus;
                    jr.Status = true;
                    return Json(jr);
                }
            }
            catch (Exception e)
            {
                jr.Message = "خطا در انجام عملیات ، درصورت مشاهده مجدد با موضوع را اطلاع رسانی کنید.";
            }
            return Json(jr);
        }


        [HttpPost]
        [SectionPermission(CrudOperation = CrudOperationType.Update, SectionType = AdminRoleSectionType.BankCardManagement)]
        [AjaxAntiForgeryValidateAttribute]
        public ActionResult NormalTransfer(long id,long toID,long amount)
        {
            JsonResponse jr = new JsonResponse(false, "خطا در انجام عملیات ، دوباره تلاش کنید و در صورت تکرار موضوع را گزارش کنید.");
            try
            {
                using (BankCardRepository bcr = new BankCardRepository(null, true))
                {
                    var instance = bcr.GetByID(id);
                    if (!SectionInfo.InternetBanks.ContainsKey(id))
                    {
                        SectionInfo.InternetBanks.Add(id, new InternetBankFactory().GetInstance(bcr.GetByID(id), SectionInfo.Setting.AntiCaptchaApiKey));
                    }


                    var balanceStatus = SectionInfo.InternetBanks[id].GetBalance();
                    instance.xBalance = balanceStatus.Item1;


                    if(instance.xBalance - Convert.ToInt64(instance.xMinDeposit) > amount*10)
                    {
                        jr.Message = "موجودی ناکافی";
                        return Json(jr);
                    }

                    var destAccount = bcr.GetByID(toID);
                    var res = SectionInfo.InternetBanks[id].NormalTransfer(destAccount.xAccountNumber, amount*10, "AdminPanel Normal Transfer", "AdminPanel Normal Transfer");
                    new SystemLogRepository().Log(SystemLogType.InternetBank, "AdminPanel Normal Transfer","From account : "+instance.xAccountNumber+" | To Account : "+destAccount.xAccountNumber+" | Amount : "+(amount*10)+" | Res:\r\n"+ JsonConvert.SerializeObject(res),((Admin)(Session["Admin"])).xID);


                    balanceStatus = SectionInfo.InternetBanks[id].GetBalance();
                    instance.xBalance = balanceStatus.Item1;
                    instance.xBannedAmount = balanceStatus.Item2;
                    instance.xDepositStatus = balanceStatus.Item3;

                    bcr.Update(instance);

                    jr.Message = instance.xBalance + " | " + instance.xDepositStatus;
                    jr.Status = true;
                    return Json(jr);
                }
            }
            catch (Exception e)
            {
                jr.Message = "خطا در انجام عملیات ، درصورت مشاهده مجدد با موضوع را اطلاع رسانی کنید.";
            }
            return Json(jr);
        }


        [HttpPost]
        [SectionPermission(CrudOperation = CrudOperationType.Update, SectionType = AdminRoleSectionType.BankCardManagement)]
        [AjaxAntiForgeryValidateAttribute]
        public ActionResult GetBalance(long id)
        {
            JsonResponse jr = new JsonResponse(false, "خطا در انجام عملیات ، دوباره تلاش کنید و در صورت تکرار موضوع را گزارش کنید.");
            try
            {
                using (BankCardRepository bcr = new BankCardRepository(null, true))
                {
                    if (!SectionInfo.InternetBanks.ContainsKey(id))
                    {
                        SectionInfo.InternetBanks.Add(id, new InternetBankFactory().GetInstance(bcr.GetByID(id), SectionInfo.Setting.AntiCaptchaApiKey));
                    }

                    var instance = bcr.GetByID(id);
                    var balanceStatus= SectionInfo.InternetBanks[id].GetBalance();
                    instance.xBalance = balanceStatus.Item1;
                    instance.xBannedAmount = balanceStatus.Item2;
                    instance.xDepositStatus = balanceStatus.Item3;

                    bcr.Update(instance);

                    jr.Message = instance.xBalance+" | "+ instance.xDepositStatus;
                    jr.Status = true;
                    return Json(jr);
                }
            }
            catch (Exception e)
            {
                jr.Message = "خطا در انجام عملیات ، درصورت مشاهده مجدد با موضوع را اطلاع رسانی کنید.";
            }
            return Json(jr);
        }


        [HttpPost]
        [SectionPermission(CrudOperation = CrudOperationType.Update, SectionType = AdminRoleSectionType.BankCardManagement)]
        [AjaxAntiForgeryValidateAttribute]
        public ActionResult GetAdnUpdateHistory(long id,DateTime fromDate,DateTime toDate)
        {
            JsonResponse jr = new JsonResponse(false, "خطا در انجام عملیات ، دوباره تلاش کنید و در صورت تکرار موضوع را گزارش کنید.");
            try
            {
                using (BankCardRepository bcr = new BankCardRepository(null, true))
                {
                    if (!SectionInfo.InternetBanks.ContainsKey(id))
                    {
                        SectionInfo.InternetBanks.Add(id, new InternetBankFactory().GetInstance(bcr.GetByID(id), SectionInfo.Setting.AntiCaptchaApiKey));
                    }

                    var instance = bcr.GetByID(id);

                    var balanceStatus = SectionInfo.InternetBanks[id].GetBalance();
                    instance.xBalance = balanceStatus.Item1;
                    instance.xBannedAmount = balanceStatus.Item2;
                    instance.xDepositStatus = balanceStatus.Item3;

                    int numOfNewRecordFound = 0;
                    using (CartTransferHistoryRepository cthr = new CartTransferHistoryRepository())
                    {
                        foreach (var cItem in SectionInfo.InternetBanks[id].GetCartTransferHistory(fromDate, toDate))
                        {
                            if (!cthr.IsDocumentNumberExist(cItem.DocumentNumber))
                            {
                                numOfNewRecordFound++;
                                CartTransferHistory cthInsatnce = new CartTransferHistory();
                                cthInsatnce.xAmountIn = cItem.AmountIn;
                                cthInsatnce.xAmountOut = cItem.AmountOut;
                                cthInsatnce.xBankCardID = instance.xID;
                                cthInsatnce.xCardNumber = cItem.CardNumber;
                                cthInsatnce.xCodeErja = cItem.CodeErja;
                                cthInsatnce.xCodePeigiri = cItem.CodePeygiri;
                                cthInsatnce.xDate = DateTime.Now;
                                cthInsatnce.xDescription = cItem.Description;
                                cthInsatnce.xDocumentDate = cItem.Date;
                                cthInsatnce.xDocumentNumber = cItem.DocumentNumber;

                                cthr.Insert(cthInsatnce);
                            }
                        }
                    }



                    jr.Message = string.Format("{0} رکورد جدید پیدا و ذخیره شد", numOfNewRecordFound);
                    jr.Status = true;
                    return Json(jr);
                }
            }
            catch (Exception e)
            {
                jr.Message = "خطا در انجام عملیات ، درصورت مشاهده مجدد با موضوع را اطلاع رسانی کنید.";
            }
            return Json(jr);
        }

        [HttpPost]
        [AjaxAntiForgeryValidateAttribute]
        public ActionResult GetCardHolderName(string cardNumber)
        {
            JsonResponse jr = new JsonResponse(false, "خطا در انجام عملیات ، دوباره تلاش کنید و در صورت تکرار موضوع را گزارش کنید.");
            try
            {
                using (BankCardRepository bcr = new BankCardRepository(null, true))
                {
                    var bci = bcr.GetAppropriteForCheckHolderName();
                    if (!SectionInfo.InternetBanks.ContainsKey(bci.xID))
                    {
                        SectionInfo.InternetBanks.Add(bci.xID, new InternetBankFactory().GetInstance(bci, SectionInfo.Setting.AntiCaptchaApiKey));
                    }

                    jr.Message = SectionInfo.InternetBanks[bci.xID].GetCardHolderName(cardNumber);
                    jr.Status = true;
                    return Json(jr);
                }
            }
            catch (Exception e)
            {
                jr.Message = "خطا در انجام عملیات ، درصورت مشاهده مجدد با موضوع را اطلاع رسانی کنید.";
            }
            return Json(jr);
        }

        [BasicViewBagInitializer(CurrentController = "BankCard")]
        [SectionPermission(CrudOperation = CrudOperationType.Report, SectionType = AdminRoleSectionType.BankCardManagement)]
        public string Report(int currentPage = 1)
        {
            return "این ماژول پیاده سازی نشده است";
        }


    }
}
