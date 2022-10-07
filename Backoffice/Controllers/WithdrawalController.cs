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
using Saraf365.Core.InternetBankModules;

namespace Saraf365.Backoffice.Controllers
{

    [NoCache]
    [AdminAuthorize]
    public class WithdrawalController : Controller
    {

        [BasicViewBagInitializer(CurrentController = "Withdrawal")]
        [SectionPermission(CrudOperation = CrudOperationType.Select, SectionType = AdminRoleSectionType.WithdrawalManagement)]
        public ActionResult Index(int currentPage = 1)
        {
            return View();
        }

       
        [HttpGet]
        [UniqueKeyDecryptor(UniqueKeyFieldName = "xID")]
        [BasicViewBagInitializer(CurrentController = "Withdrawal")]
        [SectionPermission(CrudOperation = CrudOperationType.Update, SectionType = AdminRoleSectionType.WithdrawalManagement)]
        public ActionResult UpdateRender(long xID = 0)
        {
            ViewBag.xID = xID;
            return View();
        }

        private long CalculateWithdrawalWithCashCost(int amount)
        {
            foreach(var item in SectionInfo.Setting.CashWithdrawalCost)
            {
                if(item.Value.Item1<= amount && item.Value.Item2>amount)
                {
                    return item.Key;
                }
            }
            return 500;
        }


        [HttpPost]
        [UniqueKeyDecryptor(UniqueKeyFieldName = "xID")]
        [SectionPermission(CrudOperation = CrudOperationType.Update, SectionType = AdminRoleSectionType.WithdrawalManagement)]
        [AjaxAntiForgeryValidateAttribute]
        public ActionResult PayByBankMethodTransfer(long xID = 0)
        {
            JsonResponse jr = new JsonResponse(false, "خطا در انجام عملیات ، دوباره تلاش کنید و در صورت تکرار موضوع را گزارش کنید.");
            try
            {
                throw new Exception("not yet implemented");
                using (WithdrawalRepository cr = new WithdrawalRepository(null, true))
                {
                    Withdrawal instance = cr.GetByID(xID);
                    new SystemLogRepository().Log(SystemLogType.BO_Withdrawal, "ویرایش اطلاعات تسویه-قبل از تغییر", instance.GetSerializedData(), ((Admin)(Session["Admin"])).xID);

                    
                    new SystemLogRepository().Log(SystemLogType.BO_Withdrawal, "ویرایش اطلاعات تسویه-بعد از تغییر", instance.GetSerializedData(), ((Admin)(Session["Admin"])).xID);
                    cr.Update(instance);


                }


                jr.Message = "پرداخت شد";
                jr.Status = true;
                return Json(jr);



            }
            catch { }
            return Json(jr);
        }



        [HttpPost]
        [UniqueKeyDecryptor(UniqueKeyFieldName = "xID")]
        [SectionPermission(CrudOperation = CrudOperationType.Update, SectionType = AdminRoleSectionType.WithdrawalManagement)]
        [AjaxAntiForgeryValidateAttribute]
        public ActionResult PayByPerfectMoney(long xID = 0)
        {
            JsonResponse jr = new JsonResponse(false, "خطا در انجام عملیات ، دوباره تلاش کنید و در صورت تکرار موضوع را گزارش کنید.");
            try
            {
                using (WithdrawalRepository cr = new WithdrawalRepository(null, true))
                {
                    Withdrawal instance = cr.GetByID(xID);
                    new SystemLogRepository().Log(SystemLogType.BO_Withdrawal, "ویرایش اطلاعات تسویه-قبل از تغییر", instance.GetSerializedData(), ((Admin)(Session["Admin"])).xID);

                    if(instance.xWithdrawalType==(byte)WithdrawalType.PerfectMoney && (instance.xStatus!=(byte)WithdrawalStatusType.Paied || instance.xStatus != (byte)WithdrawalStatusType.Rejected))
                    {
                        var perfectRes = new PerfectMoney(SectionInfo.Setting.PerfectMoneyID, SectionInfo.Setting.PerfectMoneyPassword, SectionInfo.Setting.PerfectMoneyAccount).Create(instance.xSecondCurrencyAmmount.ToString());
                        if(perfectRes[0]=="")
                        {
                            instance.xStatus = (byte)WithdrawalStatusType.Paied;
                            instance.xWithdrawalCost = instance.xAmount * SectionInfo.Setting.PerfectWithdrawalCostPercent/100;
                            instance.xDescription = perfectRes[1];
                            instance.xSettlementDate = DateTime.Now;


                        }
                        else
                        {
                            jr.Message = perfectRes[0];
                            return (Json(jr));
                        }
                    }
                    else
                    {
                        return (Json(jr));
                    }

                    new SystemLogRepository().Log(SystemLogType.BO_Withdrawal, "ویرایش اطلاعات تسویه-بعد از تغییر", instance.GetSerializedData(), ((Admin)(Session["Admin"])).xID);
                    cr.Update(instance);


                }


                jr.Message = "پرداخت شد";
                jr.Status = true;
                return Json(jr);



            }
            catch { }
            return Json(jr);
        }



        [HttpPost]
        [UniqueKeyDecryptor(UniqueKeyFieldName = "xID")]
        [SectionPermission(CrudOperation = CrudOperationType.Update, SectionType = AdminRoleSectionType.WithdrawalManagement)]
        [AjaxAntiForgeryValidateAttribute]
        public ActionResult PayByShetab(long fromDeposit, string method,long xID = 0)
        {
            JsonResponse jr = new JsonResponse(false, "خطا در انجام عملیات ، دوباره تلاش کنید و در صورت تکرار موضوع را گزارش کنید.");

            try
            {
                using (WithdrawalRepository cr = new WithdrawalRepository(null, true))
                {
                    Withdrawal instance = cr.GetByID(xID);
                    if (method == "CardTransfer" && instance.xAmount > 3000000)
                    {
                        jr.Message = "سقف کارت به کارت 3 میلیون تومان می باشد";
                        return Json(jr);
                    }


                    new SystemLogRepository().Log(SystemLogType.BO_Withdrawal, "ویرایش اطلاعات تسویه-قبل از تغییر", instance.GetSerializedData(), ((Admin)(Session["Admin"])).xID);

                    if (instance.xWithdrawalType == (byte)WithdrawalType.Cash && (instance.xStatus != (byte)WithdrawalStatusType.Paied || instance.xStatus != (byte)WithdrawalStatusType.Rejected))
                    {

                        
                        try
                        {
                            using (BankCardRepository bcr = new BankCardRepository(null, true))
                            {
                                var bcInstance = bcr.GetByID(fromDeposit);
                                if (SectionInfo.InternetBanks[fromDeposit] == null)
                                {
                                    SectionInfo.InternetBanks.Add(fromDeposit, new InternetBankFactory().GetInstance(bcr.GetByID(fromDeposit), SectionInfo.Setting.AntiCaptchaApiKey));
                                }

                                var balanceStatus = SectionInfo.InternetBanks[fromDeposit].GetBalance();
                                bcInstance.xBalance = balanceStatus.Item1;

                                if (bcInstance.xBalance - Convert.ToInt64(bcInstance.xMinDeposit) > instance.xAmount * 10)
                                {
                                    jr.Message = "موجودی  سپرده انتخابی کافی نیست";
                                    return Json(jr);
                                }

                                Tuple<bool, string> payRes = null;
                                if(method == "CardTransfer")
                                {
                                    payRes = SectionInfo.InternetBanks[fromDeposit].CardTransfer(instance.UserBankAccount.xCartNumber, instance.xAmount * 10);
                                    new SystemLogRepository().Log(SystemLogType.InternetBank, "AdminPanel Withdrawal With Card Transfer","Widthdrawal ID "+instance.xID+ " | From card : " + bcInstance.xCardNumber + " | To Card : " + instance.UserBankAccount.xCartNumber + " | Amount : " + (instance.xAmount * 10) + " | Res:\r\n" + JsonConvert.SerializeObject(payRes), ((Admin)(Session["Admin"])).xID);
                                }
                                else
                                {
                                    payRes = SectionInfo.InternetBanks[fromDeposit].NormalTransfer(instance.UserBankAccount.xAccountNumber, instance.xAmount * 10);
                                    new SystemLogRepository().Log(SystemLogType.InternetBank, "AdminPanel Withdrawal With Normal Transfer", "Widthdrawal ID " + instance.xID + " | From account : " + bcInstance.xAccountNumber + " | To Account : " + instance.UserBankAccount.xAccountNumber + " | Amount : " + (instance.xAmount * 10) + " | Res:\r\n" + JsonConvert.SerializeObject(payRes), ((Admin)(Session["Admin"])).xID);
                                }
                                
                                

                                balanceStatus = SectionInfo.InternetBanks[fromDeposit].GetBalance();
                                bcInstance.xBalance = balanceStatus.Item1;
                                bcInstance.xBannedAmount = balanceStatus.Item2;
                                bcInstance.xDepositStatus = balanceStatus.Item3;

                                bcr.Update(bcInstance);

                                if(payRes.Item1)
                                {
                                    instance.xStatus = (byte)WithdrawalStatusType.Paied;
                                    instance.xWithdrawalCost = CalculateWithdrawalWithCashCost(instance.xAmount);
                                    instance.xDescription = payRes.Item2;
                                    instance.xSettlementDate = DateTime.Now;
                                }


                                jr.Message =payRes.Item2+ " | "+ bcInstance.xBalance + " | " + bcInstance.xDepositStatus;
                                jr.Status = true;
                                return Json(jr);
                            }
                        }
                        catch (Exception e)
                        {
                            jr.Message = "خطا در انجام عملیات ، درصورت مشاهده مجدد با موضوع را اطلاع رسانی کنید.";
                        }
                        






                    }
                    else
                    {
                        return (Json(jr));
                    }

                    new SystemLogRepository().Log(SystemLogType.BO_Withdrawal, "ویرایش اطلاعات تسویه-بعد از تغییر", instance.GetSerializedData(), ((Admin)(Session["Admin"])).xID);
                    cr.Update(instance);


                }


                jr.Message = "پرداخت شد";
                jr.Status = true;
                return Json(jr);



            }
            catch { }
            return Json(jr);
        }


        [HttpPost]
        [UniqueKeyDecryptor(UniqueKeyFieldName = "xID")]
        [SectionPermission(CrudOperation = CrudOperationType.Update, SectionType = AdminRoleSectionType.WithdrawalManagement)]
        [AjaxAntiForgeryValidateAttribute]
        public async Task<ActionResult> Update(Withdrawal args, long xID = 0,bool calculateCost=true)
        {
            JsonResponse jr = new JsonResponse(false, "خطا در انجام عملیات ، دوباره تلاش کنید و در صورت تکرار موضوع را گزارش کنید.");
            try
            {
                using (WithdrawalRepository cr = new WithdrawalRepository(null, true))
                {
                    Withdrawal instance = cr.GetByID(xID);
                    new SystemLogRepository().Log(SystemLogType.BO_Withdrawal, "ویرایش اطلاعات تسویه-قبل از تغییر", instance.GetSerializedData(), ((Admin)(Session["Admin"])).xID);
                    

                    if (instance.xStatus != (byte)WithdrawalStatusType.Rejected && args.xStatus == (byte)WithdrawalStatusType.Rejected)
                    {
                        
                        instance.xStatus = args.xStatus;
                    }
                    else if(instance.xStatus == (byte)WithdrawalStatusType.Rejected || instance.xStatus == (byte)WithdrawalStatusType.Paied)
                    {
                        jr.Message = "تغییر از این وضعیت غیر قابل انجام است";
                    }
                    else if(args.xStatus==(byte)WithdrawalStatusType.Paied)
                    {
                        instance.xSettlementDate = DateTime.Now;
                        if (calculateCost)
                        {
                            if(instance.xWithdrawalType==(byte)WithdrawalType.Cash)
                            {
                                instance.xWithdrawalCost = CalculateWithdrawalWithCashCost(instance.xAmount);
                            }
                            else
                            {
                                instance.xWithdrawalCost = instance.xAmount/SectionInfo.Setting.PerfectWithdrawalCostPercent;
                            }
                        }
                        instance.xStatus = args.xStatus;
                    }
                    else
                    {
                        instance.xStatus = args.xStatus;
                    }
                    new SystemLogRepository().Log(SystemLogType.BO_Withdrawal, "ویرایش اطلاعات تسویه-بعد از تغییر", instance.GetSerializedData(), ((Admin)(Session["Admin"])).xID);
                    instance.xDescription = args.xDescription;

                    cr.Update(instance);


                }
                

                jr.Message = "ذخیره با موفقیت انجام شد";
                jr.Status = true;
                return Json(jr);



            }
            catch{}
            return Json(jr);
        }

       


    }
}
