using Newtonsoft.Json;
using Saraf365.Core;
using Saraf365.Core.Enumerations;
using Saraf365.Core.Models;
using Saraf365.Core.Repositories;
using Saraf365.Core.Utils;
using Saraf365.Website2.Filters;
using Saraf365.Website2.Utils;
using RockCandy.Web.Framework.Core.ActionFilters;
using RockCandy.Web.Framework.Core.Models;
using RockCandy.Web.Framework.Utilities;
using RockCandy.Web.Framework.Utilities.Encryption;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Saraf365.Website2.Controllers
{
    [ViewBagInitalizer]
    [UserAuthorize]
    public class ProfileController : Controller
    {
        // GET: Profile

        public ActionResult Logout()
        {
            new SessionUtils().RemoveSession(Session, SectionInfo.UserSessionName);
            return RedirectToAction("Index","Home");
        }
        public ActionResult Index()
        {
            ViewBag.PerfectMoneyAmount = new PerfectMoney(SectionInfo.Setting.PerfectMoneyID, SectionInfo.Setting.PerfectMoneyPassword, SectionInfo.Setting.PerfectMoneyAccount).GetBalance();
            if (ViewBag.PerfectMoneyAmount[0].Contains("disabled"))
            {
                ViewBag.PerfectMoneyAmount = new string[] { "0", "0" };
            }
            long userID = new SessionUtils().GetSessionValue<long>(Session, SectionInfo.UserSessionName);
            using (UserRepository ur = new UserRepository())
            {
                var instance = ur.GetByID(userID);


                ViewBag.UserID = instance.xID;

                
            }
            return View();
        }

    


        [HttpPost]
        [AjaxAntiForgeryValidateAttribute]
        public ActionResult SendActivationEmail()
        {
            JsonResponse jr = new JsonResponse(false, "");
            using (UserRepository ur = new UserRepository(null, true))
            {
                var userInstance = ur.GetByID(Convert.ToInt64(ViewBag.UserID));

                if (Convert.ToDateTime(userInstance.xLastSendEmailDate).AddMinutes(SectionInfo.Setting.ActivationResendInMins) > DateTime.Now)
                {
                    jr.Message = SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_SendActivationPeriodNotComplete"];
                    jr.CustomFields.Add("CountDown", Math.Abs(Convert.ToInt32((Convert.ToDateTime(userInstance.xLastSendEmailDate).AddMinutes(SectionInfo.Setting.ActivationResendInMins) - DateTime.Now).TotalSeconds)).ToString());
                    jr.CustomFields.Add("title", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Error"]);
                    jr.CustomFields.Add("confirmButtonText", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Ok"]);
                    return Json(jr);
                }
                
                userInstance.xActivationCode = new Random().Next(1000000, 10000000).ToString();
                userInstance.xActivationCodeExpireAt = DateTime.Now.AddMinutes(SectionInfo.Setting.ActivationCodeExpireInMins);
                userInstance.xLastSendEmailDate = DateTime.Now;
                jr.CustomFields.Add("CountDown", Math.Abs(Convert.ToInt32((Convert.ToDateTime(userInstance.xLastSendEmailDate).AddMinutes(SectionInfo.Setting.ActivationResendInMins) - DateTime.Now).TotalSeconds)).ToString());
                new EmailWrapper().SendActivationCode(userInstance.xEmail, userInstance.xActivationCode, ViewBag.LanguageCode);

                ur.Update(userInstance);
            }
            jr.Message = SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_EmailActivationSent"];
            jr.Status = true;
            jr.CustomFields.Add("title", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Success"]);
            jr.CustomFields.Add("confirmButtonText", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Ok"]);
            return Json(jr);
        }

        [HttpPost]
        [AjaxAntiForgeryValidateAttribute]
        public ActionResult ActivationEmail(string emailActivationCode)
        {
            JsonResponse jr = new JsonResponse(false, "");
            using (UserRepository ur = new UserRepository(null, true))
            {
                User userInstance = ur.GetByID(Convert.ToInt64(ViewBag.UserID));
                if (Convert.ToDateTime(userInstance.xActivationCodeExpireAt) < DateTime.Now)
                {
                    jr.Message = SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_ActivationCodeExpires"];
                    jr.CustomFields.Add("title", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Error"]);
                    jr.CustomFields.Add("confirmButtonText", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Ok"]);
                    return Json(jr);
                }
                else if(userInstance.xActivationCode!= emailActivationCode || userInstance.xActivationCode=="-1")
                {
                    jr.Message = SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_ActivationCodeIsWrong"];
                    //userInstance.xActivationCode = "-1";
                    jr.CustomFields.Add("title", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Error"]);
                    jr.CustomFields.Add("confirmButtonText", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Ok"]);
                    //ur.Update(userInstance);
                    return Json(jr);
                }
                else
                {
                    userInstance.xActivationCode ="-1";
                    userInstance.xIsEmailValidated = true;
                    ur.Update(userInstance);
                    jr.Message = SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_EmailValidated"];
                    jr.Status = true;
                    jr.CustomFields.Add("title", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Success"]);
                    jr.CustomFields.Add("confirmButtonText", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Ok"]);
                    return Json(jr);
                }
            }
        }


        [HttpPost]
        [AjaxAntiForgeryValidateAttribute]
        public ActionResult ActivationCellphone(string smsActivationCode)
        {
            JsonResponse jr = new JsonResponse(false, "");
            using (UserRepository ur = new UserRepository(null, true))
            {
                User userInstance = ur.GetByID(Convert.ToInt64(ViewBag.UserID));
                if (Convert.ToDateTime(userInstance.xActivationCodeExpireAt) < DateTime.Now)
                {
                    jr.Message = SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_ActivationCodeExpires"];
                    jr.CustomFields.Add("title", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Error"]);
                    jr.CustomFields.Add("confirmButtonText", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Ok"]);
                    return Json(jr);
                }
                else if (userInstance.xCellphoneActivationCode != smsActivationCode || userInstance.xCellphoneActivationCode == "-1")
                {
                    jr.Message = SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_ActivationCodeIsWrong"];
                    //userInstance.xCellphoneActivationCode = "-1";
                    jr.CustomFields.Add("title", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Error"]);
                    jr.CustomFields.Add("confirmButtonText", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Ok"]);
                    //ur.Update(userInstance);
                    return Json(jr);
                }
                else
                {
                    userInstance.xCellphoneActivationCode = "-1";
                    userInstance.xCellphoneActivated = true;
                    ur.Update(userInstance);
                    jr.Message = SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_CellphoneValidated"];
                    jr.Status = true;
                    jr.CustomFields.Add("title", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Success"]);
                    jr.CustomFields.Add("confirmButtonText", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Ok"]);
                    return Json(jr);
                }
            }
        }


        [HttpPost]
        [AjaxAntiForgeryValidateAttribute]
        public ActionResult SendActivationSms()
        {

            JsonResponse jr = new JsonResponse(false, "");
            using (UserRepository ur = new UserRepository(null, true))
            {
                var userInstance = ur.GetByID(Convert.ToInt64(ViewBag.UserID));

                if (Convert.ToDateTime(userInstance.xLastSendSmsDate).AddMinutes(SectionInfo.Setting.ActivationResendInMins) > DateTime.Now)
                {
                    jr.Message = SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_SendActivationPeriodNotComplete"];
                    jr.CustomFields.Add("CountDown", Math.Abs(Convert.ToInt32((Convert.ToDateTime(userInstance.xLastSendSmsDate).AddMinutes(SectionInfo.Setting.ActivationResendInMins) - DateTime.Now).TotalSeconds)).ToString());
                    jr.CustomFields.Add("title", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Error"]);
                    jr.CustomFields.Add("confirmButtonText", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Ok"]);
                    return Json(jr);
                }

                userInstance.xCellphoneActivationCode = new Random().Next(1000000, 10000000).ToString();
                userInstance.xActivationCodeExpireAt = DateTime.Now.AddMinutes(SectionInfo.Setting.ActivationCodeExpireInMins);
                userInstance.xLastSendSmsDate = DateTime.Now;
                jr.CustomFields.Add("CountDown", Math.Abs(Convert.ToInt32((Convert.ToDateTime(userInstance.xLastSendSmsDate).AddMinutes(SectionInfo.Setting.ActivationResendInMins) - DateTime.Now).TotalSeconds)).ToString());
                new SmsUtils().SendActivationCode(userInstance.xCellphone, userInstance.xCellphoneActivationCode, ViewBag.LanguageCode);

                ur.Update(userInstance);
            }
            jr.Message = SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_SmsActivationSent"];
            jr.Status = true;
            jr.CustomFields.Add("title", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Success"]);
            jr.CustomFields.Add("confirmButtonText", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Ok"]);
            return Json(jr);

        }



        [HttpPost]
        [AjaxAntiForgeryValidateAttribute]
        public ActionResult ChangePassword(string currentPassword,string newPassword)
        {

            JsonResponse jr = new JsonResponse(false, "");
            using (UserRepository ur = new UserRepository(null, true))
            {
                User userInstance = ur.GetByID(Convert.ToInt64(ViewBag.UserID));

                if (newPassword.Length<5)
                {
                    jr.CustomFields.Add("title", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Error"]);
                    jr.CustomFields.Add("confirmButtonText", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Ok"]);
                    jr.Message = SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_PleaseRetryLater"];
                    return Json(jr);
                }

                if (ur.Authentication(userInstance.xEmail, new CryptoUtils().ComputeSha256Hash(currentPassword + userInstance.xSalt))!=null)
                {
                    userInstance.xSalt = new CryptoUtils().GenerateSalt();
                    userInstance.xPassword = new CryptoUtils().ComputeSha256Hash(newPassword + userInstance.xSalt);
                    ur.Update(userInstance);
                    jr.Message = SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_PasswordChanged"];
                    jr.Status = true;
                    jr.CustomFields.Add("title", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Success"]);
                    jr.CustomFields.Add("confirmButtonText", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Ok"]);
                }
                else
                {
                    jr.Message = SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_CurrentPasswordIsWrong"];
                    jr.CustomFields.Add("title", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Error"]);
                    jr.CustomFields.Add("confirmButtonText", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Ok"]);
                    
                    
                }
               
            }
            new SessionUtils().RemoveSession(Session, SectionInfo.UserSessionName);
            return Json(jr);




        }

        [HttpPost]
        [AjaxAntiForgeryValidateAttribute]
        public ActionResult PaymentRequest(long amount,long gateWayID, string cardNumber="",string toCardNumber = "",string voucherCode="", string voucherActivationCode = "")
        {
            JsonResponse jr = new JsonResponse(false, "");
            var minAmount= Math.Round(((SectionInfo.Setting.PerfectMoneySellPrice) + (SectionInfo.Setting.PerfectMoneySellPrice * 1.0 * SectionInfo.Setting.PerfectWithdrawalCostPercent / 100)) / SectionInfo.Setting.PerfectMoneySellPrice, 2);
            if(amount<minAmount)
            {
                jr.Message =string.Format("کم ترین میزان خرید مبلغ {0} تومان می باشد", minAmount);
                jr.CustomFields.Add("title", "کف خرید را رعایت فرمایید");
                jr.CustomFields.Add("confirmButtonText", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Ok"]);
                return Json(jr);
            }

            toCardNumber = toCardNumber.Replace("-", "");
            
            using (UserRepository ur = new UserRepository(null, true))
            {
                Saraf365.Core.User userInstance = ur.GetByIDAndLoadUserBankAccount(Convert.ToInt64(ViewBag.UserID));
                if (ProfileValidation.Validate(userInstance)!=0)
                {
                    jr.Message = SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_CompeletYouProfileAlertDescription"];
                    jr.CustomFields.Add("title", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_CompeletYouProfileAlertTitle"]);
                    jr.CustomFields.Add("confirmButtonText", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Ok"]);
                    return Json(jr);
                }
                var userVerifiedCardNumbers = new UserBankAccountRepository().GetUserBankAccounts(userInstance.xID).Select(x => x.xCartNumber).ToList();
                if(userVerifiedCardNumbers.IndexOf(cardNumber)<0)
                {
                    jr.Message = SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_PleaseRetryLater"];
                    jr.CustomFields.Add("title", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Error"]);
                    jr.CustomFields.Add("confirmButtonText", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Ok"]);
                    return Json(jr);
                }

                using (GatewayRepository gr = new GatewayRepository(null, true))
                {
                    Gateway gatewayInstance = gr.GetByID(gateWayID);
                   
                    if (gatewayInstance.xType!=(byte)GatewayType.PerfectMoney && (gatewayInstance.xIsVip && !userInstance.xIsVip))
                    {
                        jr.Message = SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_PleaseRetryLater"];
                        jr.CustomFields.Add("title", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Error"]);
                        jr.CustomFields.Add("confirmButtonText", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Ok"]);
                        return Json(jr);
                    }
                    if(gatewayInstance.xType != (byte)GatewayType.PerfectMoney && amount <gatewayInstance.xMinBuyIn)
                    {
                        jr.Message = SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_InvalidMinBuyin"];
                        jr.Message = jr.Message.Replace("###", gatewayInstance.xMinBuyIn.ToString());
                        jr.CustomFields.Add("title", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Error"]);
                        jr.CustomFields.Add("confirmButtonText", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Ok"]);
                        return Json(jr);
                    }

                    using (TransactionRepository tr = new TransactionRepository(null, true))
                    {
                        Transaction transactionInstance = new Transaction();
                        transactionInstance.xUserID = userInstance.xID;
                        transactionInstance.xAmount = amount;
                        if(gatewayInstance.xCommisionPercent==0)
                        {
                            transactionInstance.xComissionAmount = 0;
                        }
                        else
                        {
                            transactionInstance.xComissionAmount = gatewayInstance.xCommisionPercent * amount / 100;
                        }
                        transactionInstance.xComissionPercent = gatewayInstance.xCommisionPercent;
                        transactionInstance.xDate = DateTime.Now;
                        transactionInstance.xGatewayID = gatewayInstance.xID;


                        string cardToCardWarning = "";
                        List<long> updateCardtToCardByTransactionID = new List<long>();
                        switch (gatewayInstance.xType)
                        {
                            
                            case (byte)GatewayType.IPG:
                                #region g8
                                using (var client = new System.Net.WebClient())
                                {
                                    GatewayConfigModel config = JsonConvert.DeserializeObject<GatewayConfigModel>(gatewayInstance.xConfig);

                                    var values = new NameValueCollection();
                                    values["api_key"] = config.IpgConfig.ApiKey;
                                    values["amount"] = (amount*10).ToString();
                                    values["return_url"] = config.IpgConfig.Callback;


                                    try
                                    {
                                        var response = JsonConvert.DeserializeObject<Dictionary<string, string>>(Encoding.Default.GetString(client.UploadValues(config.IpgConfig.Url + "invoice/request", values)));

                                        G8InterfaceApiRes res = new G8InterfaceApiRes();
                                        foreach (var item in response)
                                        {
                                            if (item.Key == "status")
                                            {
                                                res.status = Convert.ToInt32(item.Value);
                                            }
                                            else
                                            {
                                                res.data.Add(item.Key, item.Value);
                                            }
                                        }
                                        //Redirect
                                        if(res.status==1)
                                        {
                                            jr.CustomFields.Add("Redirect",config.IpgConfig.Url+"invoice/pay/"+res.data["invoice_key"]);
                                            transactionInstance.xInvoice_key = res.data["invoice_key"];
                                            if(tr.getByInvoiceKey(res.data["invoice_key"])!=null)
                                            {
                                                jr.CustomFields = new Dictionary<string, string>();
                                                jr.Message = SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_PleaseRetryLater"];
                                                jr.CustomFields.Add("title", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Error"]);
                                                jr.CustomFields.Add("confirmButtonText", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Ok"]);
                                                return Json(jr);
                                            }
                                        }
                                        else
                                        {
                                            jr.Message = SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_PleaseRetryLater"];
                                            jr.CustomFields.Add("title", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Error"]);
                                            jr.CustomFields.Add("confirmButtonText", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Ok"]);
                                            return Json(jr);
                                        }
                                        
                                    }
                                    catch (Exception e) 
                                    {
                                        jr.CustomFields = new Dictionary<string, string>();
                                        jr.Message = SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_PleaseRetryLater"];
                                        jr.CustomFields.Add("title", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Error"]);
                                        jr.CustomFields.Add("confirmButtonText", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Ok"]);
                                        return Json(jr);
                                    }



                                }
                                #endregion
                                break;
                            case (byte)GatewayType.CartBeCart:
                                #region cartbecart
                                    using (CartTransferHistoryRepository cthr = new CartTransferHistoryRepository(null,true))
                                    {

                                    var unProccessedCardPayments = cthr.GetUnProccessedByCardNumber(cardNumber).ToList();
                                    var unProccessedCardPayment = cthr.GetUnProccessedByCardNumberAndAmount(cardNumber,amount*10);
                                    if(unProccessedCardPayment!=null)
                                    {
                                        transactionInstance.xBankRef = unProccessedCardPayment.xDocumentNumber;
                                        transactionInstance.xPaymentInfo = cardNumber + " | " + unProccessedCardPayment.xDocumentNumber + " | " + unProccessedCardPayment.xDocumentDate.ToString();
                                        updateCardtToCardByTransactionID.Add(unProccessedCardPayment.xID);


                                        transactionInstance.xVerified = true;
                                        if(unProccessedCardPayment.BankCard.xCardNumber!=toCardNumber)
                                        {
                                            cardToCardWarning = SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_CardToCardToAlternativeCardWarning"];
                                        }
                                        jr.Message = SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_CartBeCartTransactionSubmitted"] + "\r\n" + cardToCardWarning;
                                        
                                    }
                                    else if(unProccessedCardPayments.Select(x=>x.xAmountIn).Sum()==amount*10)
                                    {
                                        transactionInstance.xBankRef = string.Join(",",unProccessedCardPayments.Select(x=>x.xDocumentNumber).ToList());
                                        transactionInstance.xPaymentInfo = cardNumber + " | " + string.Join(",", unProccessedCardPayments.Select(x => x.xDocumentNumber).ToList()) + " | " + string.Join(",", unProccessedCardPayments.Select(x => x.xDocumentDate.ToString()).ToList());
                                        foreach(var item in unProccessedCardPayments)
                                        {
                                            updateCardtToCardByTransactionID.Add(item.xID);
                                            if (item.BankCard.xCardNumber != toCardNumber)
                                            {
                                                if(cardToCardWarning=="")
                                                {
                                                    cardToCardWarning = SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_CardToCardToAlternativeCardWarning"];
                                                }
                                                
                                            }
                                        }
                                        transactionInstance.xVerified = true;
                                        jr.Message = SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_CartBeCartTransactionSubmitted"] + "\r\n" + cardToCardWarning;
                                    }
                                    else
                                    {
                                        jr.Message = SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_CartPaymentNotDetectYet"];
                                        jr.CustomFields.Add("title", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Error"]);
                                        jr.CustomFields.Add("confirmButtonText", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Ok"]);
                                        return Json(jr);
                                    }

                                    
                                    if (tr.isBankRefExist(transactionInstance.xBankRef))
                                    {
                                        jr.Message = SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_CartPaymentAlreadySubmitted"];
                                        jr.CustomFields.Add("title", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Error"]);
                                        jr.CustomFields.Add("confirmButtonText", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Ok"]);
                                        return Json(jr);
                                    }



                                    }
                                #endregion
                                break;
                            case (byte)GatewayType.Bitcoin:
                            case (byte)GatewayType.Voucher:
                            default:
                                jr.Message = SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_PleaseRetryLater"];
                                jr.CustomFields.Add("title", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Error"]);
                                jr.CustomFields.Add("confirmButtonText", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Ok"]);
                                return Json(jr);
                        }

                        if (transactionInstance.xVerified)
                        {

                            string arbitarySuccessMessage = "";
                            var PM = new PerfectMoney(SectionInfo.Setting.PerfectMoneyID, SectionInfo.Setting.PerfectMoneyPassword, SectionInfo.Setting.PerfectMoneyAccount);
                            //var requestedPM = Math.Round(((transactionInstance.xAmount) - (transactionInstance.xAmount*1.0 * SectionInfo.Setting.PerfectWithdrawalCostPercent / 100)) / SectionInfo.Setting.PerfectMoneySellPrice, 2);
                            var requestedPM = Math.Round(transactionInstance.xAmount*1.0 / SectionInfo.Setting.PerfectMoneySellPrice, 2);

                            if (userInstance.xAgentID != null)
                            {
                                var agentInstance = ur.GetByID(userInstance.xAgentID);
                                if (agentInstance != null)
                                {
                                    var shareamount = Convert.ToInt64(((SectionInfo.Setting.PerfectMoneySellPrice - SectionInfo.Setting.PerfectMoneyBuyPrice) * agentInstance.xRefferalSharePercent / 100) * requestedPM);
                                    agentInstance.xWallet += shareamount;
                                    new FinancialLogRepository().Log(agentInstance.xID, FinancialLogType.RefferalShare, string.Format("قیمت فروش {0} | قیمت خرید :{1} | میزان پرفکت در خواستی کاربر : {2} | درصد سهم شما :{3}", SectionInfo.Setting.PerfectMoneySellPrice, SectionInfo.Setting.PerfectMoneyBuyPrice, requestedPM, agentInstance.xRefferalSharePercent), SectionInfo.LogAdminName, shareamount);
                                    ur.Update(agentInstance);
                                }
                            }


                            if (Convert.ToDouble(PM.GetBalance()[1]) >= requestedPM)
                            {
                                var voucher = PM.Create(requestedPM.ToString());

                                if (voucher[0] == "")
                                {

                                    new FinancialLogRepository().Log(Convert.ToInt64(ViewBag.UserID), FinancialLogType.UserVoucher, voucher[1], SectionInfo.LogAdminName, transactionInstance.xAmount);
                                    new FinancialLogRepository().Log(Convert.ToInt64(ViewBag.UserID), FinancialLogType.MiscCost, "هزینه صدور وچر پرفکت", SectionInfo.LogAdminName, Math.Round(transactionInstance.xAmount * 1.0 / SectionInfo.Setting.PerfectWithdrawalCostPercent, 2));
                                    arbitarySuccessMessage = "وچر برای شما صادر گشت می توانید از قسمت وچر ها ، اطلاعات وچر را نیز بعدا ببینید";
                                    jr.CustomFields.Add("Voucher", voucher[1]);

                                    gatewayInstance.xLastSuccessTransactionDate = DateTime.Now;
                                    gatewayInstance.xTodayTotalTransactionAmounts += transactionInstance.xAmount;
                                    gr.Update(gatewayInstance);

                                    new FinancialLogRepository().Log(Convert.ToInt64(ViewBag.UserID), FinancialLogType.BuyIn, SectionInfo.Setting.Languages[ViewBag.LanguageCode][EnumUtils.GetAttribute<DisplayAttribute>((GatewayType)gatewayInstance.xType).Name], SectionInfo.LogAdminName, amount);

                                    if (transactionInstance.xComissionAmount != 0)
                                    {
                                        new FinancialLogRepository().Log(Convert.ToInt64(ViewBag.UserID), FinancialLogType.GatewayCommision, SectionInfo.Setting.Languages[ViewBag.LanguageCode][EnumUtils.GetAttribute<DisplayAttribute>(FinancialLogType.GatewayCommision).Name] + " - " + gatewayInstance.xID + " - " + gatewayInstance.xCommisionPercent, SectionInfo.LogAdminName, transactionInstance.xComissionAmount);
                                    }
                                    userInstance.xLevel += transactionInstance.xAmount * 1.0 / SectionInfo.Setting.UserLevelIncreaseEach;
                                    ur.Update(userInstance);

                                    tr.Insert(transactionInstance);
                                    tr.Save();
                                    using (CartTransferHistoryRepository cthr = new CartTransferHistoryRepository(null, true))
                                    {
                                        foreach (var item in updateCardtToCardByTransactionID)
                                        {

                                            var instance = cthr.GetByID(item);
                                            instance.xTransactionID = transactionInstance.xID;
                                            cthr.Update(instance);
                                        }
                                    }



                                    jr.Status = true;
                                    jr.CustomFields.Add("title", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Success"]);
                                    jr.CustomFields.Add("confirmButtonText", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Ok"]);
                                    jr.Message = arbitarySuccessMessage;


                                }
                                else
                                {
                                    //userInstance.xWallet += transactionInstance.xAmount;
                                    //new FinancialLogRepository().Log(Convert.ToInt64(ViewBag.UserID), FinancialLogType.ChargeWallet, "خطا در گرفتن پرفکت", SectionInfo.LogAdminName, transactionInstance.xAmount);
                                    arbitarySuccessMessage = "با عرض پوزش به دلیل کم بود پرفکت وچر صادر نشد اما ولت شما شارژ گردید به زودی پرفکت مانی شارژ میگردد و می توانید از ولت خود تقاضای دریافت وچر نمایید";
                                    jr.Message = arbitarySuccessMessage;
                                    jr.CustomFields.Add("title", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Error"]);
                                    jr.CustomFields.Add("confirmButtonText", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Ok"]);
                                }



                                

                            }
                            else
                            {
                                //userInstance.xWallet += transactionInstance.xAmount;
                                //new FinancialLogRepository().Log(Convert.ToInt64(ViewBag.UserID), FinancialLogType.ChargeWallet, "کم بود پرفکت", SectionInfo.LogAdminName, transactionInstance.xAmount);
                                arbitarySuccessMessage = "با عرض پوزش به دلیل خطایی در تولید وچر ، وچر برای شما صادر نشده است اما ولت شما شارژ گردید لطفا دقایقی بعد دوباره تلاش کنید.";
                                jr.Message = arbitarySuccessMessage;
                                jr.CustomFields.Add("title", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Error"]);
                                jr.CustomFields.Add("confirmButtonText", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Ok"]);
                            }

                        }

                        

                        
                            
                        
                    }


                }
                
            }

            return Json(jr);

        }




        [HttpPost]
        [AjaxAntiForgeryValidateAttribute]
        public ActionResult SubmitTicket(string title,string body,byte type=0, long ticketID=0)
        {

            JsonResponse jr = new JsonResponse(false, "");
            using (TicketRepository tr = new TicketRepository(null, true))
            {
                if(ticketID==0)
                {
                    Ticket tInstance = new Ticket();

                    tInstance.xUserID = Convert.ToInt64(ViewBag.UserID);
                    tInstance.xDate = DateTime.Now;
                    tInstance.xStatus = (byte)TicketStatusType.WaitingForResponse;
                    tInstance.xTitle = title;
                    tInstance.xType = type;

                    Conversation cInstance = new Conversation();
                    cInstance.xDate = DateTime.Now;
                    cInstance.xBody = body;

                    tInstance.Conversation.Add(cInstance);
                    tr.Insert(tInstance);
                }
                else
                {
                    var tInsatnce = tr.GetByID(ticketID);
                    tInsatnce.xStatus= (byte)TicketStatusType.WaitingForResponse;
                    tr.Update(tInsatnce);

                    using (ConversationRepository cr = new ConversationRepository())
                    {
                        Conversation cInstance = new Conversation();
                        cInstance.xTicketID = ticketID;
                        cInstance.xDate = DateTime.Now;
                        cInstance.xBody = body;

                        cr.Insert(cInstance);
                    }

                }
                

            }
            new AdminNotificationRepository().Insert(AdminNotificationType.NewTicket,title, SectionInfo.Setting.AdminNotificationSetting);
            jr.Message = SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_TicketSubmited"];
            jr.Status = true;
            jr.CustomFields.Add("title", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Success"]);
            jr.CustomFields.Add("confirmButtonText", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Ok"]);
            return Json(jr);



        }


        [HttpPost]
        [AjaxAntiForgeryValidateAttribute]
        public ActionResult AddBankAccount(UserBankAccount args)
        {

            JsonResponse jr = new JsonResponse(false, "");
            if(args.xCartNumber.Length!=16)
            {
                jr.Message = SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_PleaseRetryLater"];
                jr.CustomFields.Add("title", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Error"]);
                jr.CustomFields.Add("confirmButtonText", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Ok"]);
                return Json(jr);
            }
            if (args.xAccountNumber=="")
            {
                jr.Message = SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_PleaseRetryLater"];
                jr.CustomFields.Add("title", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Error"]);
                jr.CustomFields.Add("confirmButtonText", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Ok"]);
                return Json(jr);
            }
            if (args.xShebaNumber.Length != 24)
            {
                jr.Message = SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_PleaseRetryLater"];
                jr.CustomFields.Add("title", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Error"]);
                jr.CustomFields.Add("confirmButtonText", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Ok"]);
                return Json(jr);
            }
            args.xShebaNumber = "IR" + args.xShebaNumber;
            using (UserBankAccountRepository ubr = new UserBankAccountRepository())
            {

                if (SectionInfo.Setting.BankTypes.IndexOf(args.xBankType) < 0)
                {
                    jr.Message = SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_BankTypeNotFound"];
                    jr.CustomFields.Add("title", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Error"]);
                    jr.CustomFields.Add("confirmButtonText", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Ok"]);
                    return Json(jr);
                }
                if(ubr.IsCardNumberExist(args.xCartNumber))
                {
                    jr.Message = SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_CardNumberExist"];
                    jr.CustomFields.Add("title", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Error"]);
                    jr.CustomFields.Add("confirmButtonText", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Ok"]);
                    return Json(jr);
                }
                if (ubr.IsShebaNumberExist(args.xShebaNumber))
                {
                    jr.Message = SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_ShebaNumberExist"];
                    jr.CustomFields.Add("title", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Error"]);
                    jr.CustomFields.Add("confirmButtonText", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Ok"]);
                    return Json(jr);
                }
                if (ubr.IsAccountNumberExist(args.xAccountNumber))
                {
                    jr.Message = SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_AccountNumberExist"];
                    jr.CustomFields.Add("title", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Error"]);
                    jr.CustomFields.Add("confirmButtonText", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Ok"]);
                    return Json(jr);
                }

                args.xUserID = Convert.ToInt64(ViewBag.USerID);
                ubr.Insert(args);
            }
            new AdminNotificationRepository().Insert(AdminNotificationType.AddUserBankAccount, new UserRepository().GetByID(args.xUserID).xEmail, SectionInfo.Setting.AdminNotificationSetting);
            jr.Message = SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_BankAccountSuccessfullyAdded"];
            jr.Status = true;
            jr.CustomFields.Add("title", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Success"]);
            jr.CustomFields.Add("confirmButtonText", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Ok"]);
            return Json(jr);

        }
        private long CalculateWithdrawalWithCashCost(long amount)
        {
            foreach (var item in SectionInfo.Setting.CashWithdrawalCost)
            {
                if (item.Value.Item1 <= amount && item.Value.Item2 > amount)
                {
                    return item.Key;
                }
            }
            return 500;
        }

        [HttpPost]
        [AjaxAntiForgeryValidateAttribute]
        public ActionResult AddNewWithdrawal(long userBankAccountID,string voucherCode,string voucherActivationCode)
        {
            /*ToDo change withdrawal type , get pm voucher*/
            JsonResponse jr = new JsonResponse(false, "");
            if (ProfileValidation.Validate(new UserRepository().GetByID(ViewBag.UserID))>0)
            {
                jr.Message = SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_CompeletYouProfileAlertDescription"];
                jr.CustomFields.Add("title", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_CompeletYouProfileAlertTitle"]);
                jr.CustomFields.Add("confirmButtonText", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Ok"]);
                return Json(jr);
            }
            if(!SectionInfo.Setting.PerfectBuyAvail)
            {
                jr.Message = "با عرض پوزش سقف خرید روزانه ما پر شده است ، در صورت تمایل در روز بعد مراجعه نمایید";
                jr.CustomFields.Add("title", "سقف خرید روزانه ما پر شده است");
                jr.CustomFields.Add("confirmButtonText", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Ok"]);
                return Json(jr);
            }
            using (UserBankAccountRepository ubr = new UserBankAccountRepository())
            {
                UserBankAccount baInstance = ubr.GetByID(userBankAccountID);


                if (baInstance.xUserID != Convert.ToInt64(ViewBag.UserID))
                {
                    jr.Message = SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_WithdrawalNotValid"];
                    jr.CustomFields.Add("title", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Error"]);
                    jr.CustomFields.Add("confirmButtonText", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Ok"]);
                    return Json(jr);
                }

                using (UserRepository ur = new UserRepository())
                {
                    Saraf365.Core.User userInstance = ur.GetByID(Convert.ToInt64(ViewBag.UserID));

                    var resPerfect = new PerfectMoney(SectionInfo.Setting.PerfectMoneyID, SectionInfo.Setting.PerfectMoneyPassword, SectionInfo.Setting.PerfectMoneyAccount).Activation(voucherCode, voucherActivationCode);
                    new SystemLogRepository().Log(SystemLogType.PerfectMoneyApiCall, voucherCode + " | " + voucherActivationCode, JsonConvert.SerializeObject(resPerfect));
                    if (resPerfect[0] == "")
                    {
                        jr.Message = SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_PerfectMoneyVoucherAccepted"];
                        //resPerfect[1];


                        var amount = Convert.ToInt32(SectionInfo.Setting.PerfectMoneyBuyPrice * Convert.ToDouble(resPerfect[2]));
                        amount = amount - Convert.ToInt32(CalculateWithdrawalWithCashCost(amount));


                        new AdminNotificationRepository().Insert(AdminNotificationType.NewWithdrawal, userInstance.xEmail, SectionInfo.Setting.AdminNotificationSetting);
                        new FinancialLogRepository().Log(userInstance.xID, FinancialLogType.WithdrawalAccept, JsonConvert.SerializeObject(resPerfect) + " | " + SectionInfo.Setting.PerfectMoneyBuyPrice, SectionInfo.LogAdminName, amount);
                        new WithdrawalRepository().InsertWithdrawalWithBankType(userInstance.xID, userBankAccountID, amount);
                    }
                    else
                    {
                        jr.Message = SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_PerfectMoneyVoucherNotAccepted"];
                        jr.CustomFields.Add("title", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Error"]);
                        jr.CustomFields.Add("confirmButtonText", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Ok"]);
                        return Json(jr);
                    }




                    jr.Message = SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_WithdrawalSubmitted"];
                    jr.Status = true;
                    jr.CustomFields.Add("title", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Success"]);
                    jr.CustomFields.Add("confirmButtonText", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Ok"]);
                    return Json(jr);

                }


            }



        }

        


        

        [HttpPost]
        [AjaxAntiForgeryValidateAttribute]
        public ActionResult UploadIDCard()
        {
            JsonResponse jr = new JsonResponse(false, "");
            using (UserRepository ur = new UserRepository(null, true))
            {
                User userInstance = ur.GetByIDAndLoadUserBankAccount(Convert.ToInt64(ViewBag.UserID));

                if(userInstance.UserBankAccount.Count()==0)
                {
                    jr.Message = SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_InsertAtleastOneBankCartAtFrirst"];
                    jr.CustomFields.Add("title", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Error"]);
                    jr.CustomFields.Add("confirmButtonText", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Ok"]);
                    return Json(jr);
                }

                using (SystemFileRepository sfr = new SystemFileRepository())
                {
                    long systemFileID = sfr.InsertImage(Request.Files[0] as HttpPostedFileBase,SectionInfo.Setting.ValidFileExtentionsForIDCart,SectionInfo.Setting.MaxFileSizeForIDCard);
                    switch(systemFileID)
                    {
                        case -1:
                            jr.Message = SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_InvalidImage"];
                            break;
                        case -2:
                            jr.Message = SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_ThumbnailNotGenerated"];
                            break;
                        case -3:
                            jr.Message = SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_FileExtentionISNotValid"];
                            break;
                        case -4:
                            jr.Message = SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_MaxFileSizeReached"];
                            break;
                    }

                    if(jr.Message!="")
                    {
                        jr.CustomFields.Add("title", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Error"]);
                        jr.CustomFields.Add("confirmButtonText", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Ok"]);
                        return Json(jr);
                    }


                    
                    userInstance.xNationalIDImage = systemFileID;
                    ur.Update(userInstance);
                    new AdminNotificationRepository().Insert(AdminNotificationType.ValidateUserIDCart, userInstance.xEmail, SectionInfo.Setting.AdminNotificationSetting);
                    jr.Message = SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_UploadSeccessfully"];
                    jr.Status = true;
                    jr.CustomFields.Add("title", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Success"]);
                    jr.CustomFields.Add("confirmButtonText", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Ok"]);
                    return Json(jr);
                }
            }
        }

        public ActionResult GetPartialView(string viewName)
        {
            return View("~/Views/Profile/"+viewName+".cshtml");
        }
    }
}