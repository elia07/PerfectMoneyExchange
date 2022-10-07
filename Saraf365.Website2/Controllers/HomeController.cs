using BotDetect.Web.Mvc;
using Saraf365.Core;
using RockCandy.Web.Framework.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using Saraf365.Website2.Filters;
using Saraf365.Website2.Utils;
using Saraf365.Core.Repositories;
using RockCandy.Web.Framework.Utilities.EmailUtils;
using RockCandy.Web.Framework.Core.ActionFilters;
using Saraf365.Core.Models;
using Newtonsoft.Json;
using Saraf365.Core.Enumerations;
using RockCandy.Web.Framework.Utilities;
using System.ComponentModel.DataAnnotations;
using Saraf365.Core.Utils;
using Saraf365.Core.InternetBankModules;

namespace Saraf365.Website2.Controllers
{
    [ViewBagInitalizer]
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index(string InviteCode = "")
        {


            if (new SessionUtils().GetSessionValue<long>(Session, SectionInfo.UserSessionName) != 0)
            {
                RedirectToAction("Index", "Profile", null);
            }
            ViewBag.InviteCode = InviteCode;
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        public ActionResult RecoverPassword()
        {
            return View();
        }

        public ActionResult Faq()
        {
            return View();
        }

        public ActionResult Rules()
        {
            return View();
        }

        public ActionResult ContactUs()
        {
            return View();
        }

        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [CaptchaValidation("CaptchaLogin", "CaptchaLogin", "")]
        [AjaxAntiForgeryValidateAttribute]
        public ActionResult Login(User args, bool captchaValid)
        {
            JsonResponse jr = new JsonResponse(false, "");

            if (SectionInfo.Setting.MaintenanceMode)
            {
                jr.Message = SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_MaintenanceMode"];
            }

            if (!captchaValid)
            {
                jr.Message = SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Err_CaptchaError"];
            }

            if (jr.Message != "")
            {
                jr.CustomFields.Add("title", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Error"]);
                jr.CustomFields.Add("confirmButtonText", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Ok"]);
                return Json(jr);
            }

            using (UserRepository ur = new UserRepository(null, true))
            {
                var saltInstance = ur.GetbyEmail(args.xEmail);
                if(saltInstance==null)
                {
                    jr.CustomFields.Add("title", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Error"]);
                    jr.CustomFields.Add("confirmButtonText", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Ok"]);
                    jr.Message = SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Err_UsernameOrPAsswordNotFound"];
                }
                else
                {
                    args.xPassword = new CryptoUtils().ComputeSha256Hash(args.xPassword + saltInstance.xSalt);
                    var userInstance = ur.Authentication(args);
                    if(userInstance!=null)
                    {
                        if (userInstance.xIsActive)
                        {
                            
                            jr.CustomFields.Add("title", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Success"]);
                            jr.CustomFields.Add("confirmButtonText", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Ok"]);
                            jr.Message = SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_SuccessLogin"];
                            jr.Status = true;
                            new SessionUtils().AddSession(Session, SectionInfo.UserSessionName, userInstance.xID);
                            userInstance.xLastSigninDate = DateTime.Now;
                            ur.Update(userInstance);
                        }
                        else
                        {
                            jr.CustomFields.Add("title", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Banned"]);
                            jr.CustomFields.Add("confirmButtonText", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Ok"]);
                            jr.Message = userInstance.xDescription;
                        }
                        

                    }
                    else
                    {
                        jr.CustomFields.Add("title", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Error"]);
                        jr.CustomFields.Add("confirmButtonText", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Ok"]);
                        jr.Message = SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Err_UsernameOrPAsswordNotFound"];
                    }
                }


            }


            return Json(jr);

        }

        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [CaptchaValidation("CaptchaRegister", "CaptchaRegister", "")]
        [AjaxAntiForgeryValidateAttribute]
        public ActionResult Register(User args, bool captchaValid, long inviteCode=-1)
        {
            
            JsonResponse jr = new JsonResponse(false, "");

            if(SectionInfo.Setting.MaintenanceMode)
            {
                jr.Message = SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_MaintenanceMode"]+"\n";
            }

            if(!captchaValid)
            {
                jr.Message = SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Err_CaptchaError"] + "\n";
            }


            if (!SectionInfo.Setting.RegisterEnabled)
            {
                jr.Message = SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Err_RegisterDisabled"] + "\n";
            }

            if (!new EmailAddressAttribute().IsValid(args.xEmail))
            {
                jr.CustomFields.Add("title", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Error"]);
                jr.CustomFields.Add("confirmButtonText", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Ok"]);
                jr.Message = SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_PleaseRetryLater"];
                return Json(jr);
            }
            if (args.xPassword.Length < 5)
            {
                jr.CustomFields.Add("title", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Error"]);
                jr.CustomFields.Add("confirmButtonText", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Ok"]);
                jr.Message = SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_PleaseRetryLater"];
                return Json(jr);
            }
            if (!(args.xCellphone.Length == 11 && args.xCellphone.Substring(0, 2) == "09"))
            {
                jr.CustomFields.Add("title", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Error"]);
                jr.CustomFields.Add("confirmButtonText", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Ok"]);
                jr.Message = SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_PleaseRetryLater"];
                return Json(jr);
            }
            

            if(CheckForInvaliChars(args.xFullName) || args.xFullName=="" || System.Text.ASCIIEncoding.GetEncoding(0).GetString(System.Text.ASCIIEncoding.GetEncoding(0).GetBytes(args.xFullName)) == args.xFullName)
            {
                jr.CustomFields.Add("title", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Error"]);
                jr.CustomFields.Add("confirmButtonText", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Ok"]);
                jr.Message = SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_PleaseRetryLater"];
                return Json(jr);
            }

            using (UserRepository ur = new UserRepository(null, true))
            {
                var existEmail = ur.GetbyEmail(args.xEmail);
                if (existEmail != null)
                {
                    jr.Message += SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Err_EmailExist"] + "\n";
                }


                var existCellphone = ur.GetbyCellphone(args.xCellphone);
                if (existCellphone != null)
                {
                    jr.Message += SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Err_CellphoneExist"] + "\n";
                }

                var agent = ur.GetbyInviteCode(inviteCode);
                if (inviteCode != -1 && agent == null)
                {
                    jr.Message += "معرف شناسایی نشد";
                }


                if (jr.Message != "")
                {
                    jr.CustomFields.Add("title", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Error"]);
                    jr.CustomFields.Add("confirmButtonText", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Ok"]);
                    return Json(jr);
                }
                
                if(agent!=null)
                {
                    args.xAgentID = agent.xID;
                }
                args.xIsActive = true;
                args.xSalt = new CryptoUtils().GenerateSalt();
                args.xPassword = new CryptoUtils().ComputeSha256Hash(args.xPassword + args.xSalt);
                args.xSignupDate = DateTime.Now.Date;
                args.xActivationCode = new Random().Next(1000000, 10000000).ToString() ;
                args.xCellphoneActivationCode = new Random().Next(1000000, 10000000).ToString();
                args.xActivationCodeExpireAt = DateTime.Now.AddMinutes(SectionInfo.Setting.ActivationCodeExpireInMins);
                args.xDescription = "";
                args.xNationalIDNumber = args.xPassword.Substring(0,50);
                args.xRefferalSharePercent = 20;


                try
                {
                    if (SectionInfo.Setting.EmailActivation)
                    {
                        new EmailWrapper().SendWelcomeEmail(args.xEmail, ViewBag.LanguageCode);
                        args.xLastSendEmailDate = DateTime.Now;
                        new EmailWrapper().SendActivationCode(args.xEmail, args.xActivationCode, ViewBag.LanguageCode);
                    }

                    if (SectionInfo.Setting.CellphoneActivation)
                    {
                        args.xLastSendSmsDate = DateTime.Now;
                        new SmsUtils().SendActivationCode(args.xCellphone, args.xCellphoneActivationCode, ViewBag.LanguageCode);
                    }
                }
                catch
                {

                }


                ur.Insert(args);
                ur.Save();
                args.xInviteID = Convert.ToInt64(args.xID.ToString() + new Random().Next(111111, 999999).ToString());
                ur.Update(args);
                jr.CustomFields.Add("title", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Success"]);
                jr.CustomFields.Add("confirmButtonText", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Ok"]);
                jr.Message = SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_RegistrationSuccessful"];
                jr.Status = true;

                /*jr.CustomFields.Add("title", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Error"]);
                jr.CustomFields.Add("confirmButtonText", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Ok"]);
                jr.Message = SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Err_ErrorInRegistering"];*/
            }

            //



            return Json(jr);
        }


        private bool CheckForInvaliChars(string str)
        {
            var inValidChars = new string[] { "\"", "/", "!", "|", "&", "=","\\" };
            foreach(var item in inValidChars)
            {
                if(str.Contains(item))
                {
                    return true;
                }
            }
            return false;
        }

        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [CaptchaValidation("SendForgetPasswordEmailCaptcha", "SendForgetPasswordEmailCaptcha", "")]
        [AjaxAntiForgeryValidateAttribute]
        public ActionResult SendForgetPasswordEmail(string email, bool captchaValid)
        {

            JsonResponse jr = new JsonResponse(false, "");

            if (SectionInfo.Setting.MaintenanceMode)
            {
                jr.Message = SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_MaintenanceMode"] + "\n";
            }

            if (!captchaValid)
            {
                jr.Message = SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Err_CaptchaError"] + "\n";
            }

            if(jr.Message!="")
            {
                jr.CustomFields.Add("title", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Error"]);
                jr.CustomFields.Add("confirmButtonText", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Ok"]);
                return Json(jr);
            }
            if (!new EmailAddressAttribute().IsValid(email))
            {
                jr.CustomFields.Add("title", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Error"]);
                jr.CustomFields.Add("confirmButtonText", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Ok"]);
                jr.Message = SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_PleaseRetryLater"];
                return Json(jr);
            }


            using (UserRepository ur = new UserRepository(null, true))
            {
                var userInstance = ur.GetbyEmail(email);
                if (userInstance == null)
                {
                    jr.Message = SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_EmailNotExist"] + "\n";
                    jr.CustomFields.Add("title", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Error"]);
                    jr.CustomFields.Add("confirmButtonText", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Ok"]);
                    return Json(jr);
                }
                userInstance.xActivationCode = new Random().Next(1000000, 10000000).ToString();
                userInstance.xActivationCodeExpireAt = DateTime.Now.AddMinutes(SectionInfo.Setting.ActivationCodeExpireInMins);
                ur.Update(userInstance);
                new EmailWrapper().SendRecoverPasswordActivationCode(userInstance.xEmail, userInstance.xActivationCode, ViewBag.LanguageCode);

               
                    jr.CustomFields.Add("title", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Success"]);
                    jr.CustomFields.Add("confirmButtonText", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Ok"]);
                    jr.Message = SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_RecoverEmailSent"];
                    jr.Status = true;
            }
            return Json(jr);
        }



        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [CaptchaValidation("ForgetPasswordCaptcha", "ForgetPasswordCaptcha", "")]
        [AjaxAntiForgeryValidateAttribute]
        public ActionResult RecoverPassword(string email,string activationCode,string password, bool captchaValid)
        {

            JsonResponse jr = new JsonResponse(false, "");

            if (SectionInfo.Setting.MaintenanceMode)
            {
                jr.Message = SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_MaintenanceMode"] + "\n";
            }

            if (!captchaValid)
            {
                jr.Message = SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Err_CaptchaError"] + "\n";
            }

            if (jr.Message != "")
            {
                jr.CustomFields.Add("title", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Error"]);
                jr.CustomFields.Add("confirmButtonText", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Ok"]);
                return Json(jr);
            }
            if (!new EmailAddressAttribute().IsValid(email))
            {
                jr.CustomFields.Add("title", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Error"]);
                jr.CustomFields.Add("confirmButtonText", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Ok"]);
                jr.Message = SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_PleaseRetryLater"];
                return Json(jr);
            }
            if (password.Length<5)
            {
                jr.CustomFields.Add("title", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Error"]);
                jr.CustomFields.Add("confirmButtonText", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Ok"]);
                jr.Message = SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_PleaseRetryLater"];
                return Json(jr);
            }

            using (UserRepository ur = new UserRepository(null, true))
            {
                var userInstance = ur.GetbyEmail(email);
                if (userInstance == null)
                {
                    jr.Message = SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_EmailNotExist"] + "\n";
                    jr.CustomFields.Add("title", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Error"]);
                    jr.CustomFields.Add("confirmButtonText", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Ok"]);
                    return Json(jr);
                }

                if (Convert.ToDateTime(userInstance.xActivationCodeExpireAt) < DateTime.Now)
                {
                    jr.Message = SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_ActivationCodeExpires"];
                    jr.CustomFields.Add("title", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Error"]);
                    jr.CustomFields.Add("confirmButtonText", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Ok"]);
                    return Json(jr);
                }
                else if (userInstance.xActivationCode != activationCode || userInstance.xActivationCode == "-1")
                {
                    jr.Message = SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_ActivationCodeIsWrong"];
                    jr.CustomFields.Add("title", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Error"]);
                    jr.CustomFields.Add("confirmButtonText", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Ok"]);
                    return Json(jr);
                }
                else
                {
                    userInstance.xActivationCode = "-1";
                    userInstance.xSalt = new CryptoUtils().GenerateSalt();
                    userInstance.xPassword = new CryptoUtils().ComputeSha256Hash(password + userInstance.xSalt);
                    ur.Update(userInstance);
                    jr.Message = SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_PasswordChanged"];
                    jr.Status = true;
                    jr.CustomFields.Add("title", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Success"]);
                    jr.CustomFields.Add("confirmButtonText", SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Ok"]);
                    return Json(jr);
                }
            }
        }


        [HttpPost]
        public ActionResult Callback(string invoice_key)
        {
            using (TransactionRepository tr = new TransactionRepository(null, true))
            {

                var transactionInstance = tr.getByInvoiceKey(invoice_key);
                if (transactionInstance == null)
                {
                    ViewBag.Message = SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_TransactionNotFound"];
                    ViewBag.Title = SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Error"];
                    ViewBag.ConfirmButtonText = SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Ok"];
                    ViewBag.Type = "error";
                    return View("~/Views/Home/Notify.cshtml");
                }
                if(transactionInstance.xVerified)
                {
                    ViewBag.Message = SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_TransactionAlreadySubmitted"];
                    ViewBag.Title = SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Error"];
                    ViewBag.ConfirmButtonText = SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Ok"];
                    ViewBag.Type = "warning";
                    return View("~/Views/Home/Notify.cshtml");
                }
                using (GatewayRepository gr = new GatewayRepository(null, true))
                {
                    Gateway gatewayInstance = gr.GetByID(transactionInstance.xGatewayID);
                    GatewayConfigModel config = JsonConvert.DeserializeObject<GatewayConfigModel>(gatewayInstance.xConfig);


                    G8Interface ipg = new G8Interface(config.IpgConfig.Url,config.IpgConfig.ApiKey,"");
                    var res = ipg.CheckAndVerify(invoice_key);
                    if (res.status == 1)
                    {


                        string bankCode = "";
                        if(res.data.ContainsKey("bank_code"))
                        {
                            bankCode= res.data["bank_code"];
                        }
                        else
                        {
                            bankCode = transactionInstance.xInvoice_key;
                        }
                        if(bankCode.Length>250)
                        {
                            bankCode = bankCode.Substring(0, 249);
                        }
                        if (tr.GetByBankRef(bankCode) != null)
                        {
                            ViewBag.Message = SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_TransactionAlreadySubmitted"];
                            ViewBag.Title = SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Error"];
                            ViewBag.ConfirmButtonText = SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Ok"];
                            ViewBag.Type = "warning";
                            return View("~/Views/Home/Notify.cshtml");
                        }
                        string paymentInfo = "";
                        if(res.data.ContainsKey("paymentInfo"))
                        {
                            paymentInfo = res.data["paymentInfo"];
                        }
                        
                        if (Convert.ToInt64(res.data["amount"]) == transactionInstance.xAmount*10)
                        {
                            using (UserRepository ur = new UserRepository(null, true))
                            {
                                var userInstance = ur.GetByID(transactionInstance.xUserID);


                                string arbitarySuccessMessage = "";

                                var PM = new PerfectMoney(SectionInfo.Setting.PerfectMoneyID, SectionInfo.Setting.PerfectMoneyPassword, SectionInfo.Setting.PerfectMoneyAccount);
                                //var requestedPM = Math.Round(((transactionInstance.xAmount) - (transactionInstance.xAmount * 1.0 * SectionInfo.Setting.PerfectWithdrawalCostPercent / 100)) / SectionInfo.Setting.PerfectMoneySellPrice, 2);
                                var requestedPM = Math.Round(transactionInstance.xAmount * 1.0 / SectionInfo.Setting.PerfectMoneySellPrice, 2);
                                if (Convert.ToDouble(PM.GetBalance()[1])>=  requestedPM)
                                {
                                    var voucher = PM.Create(requestedPM.ToString());

                                    if (voucher[0] == "")
                                    {

                                        new FinancialLogRepository().Log(Convert.ToInt64(ViewBag.UserID), FinancialLogType.UserVoucher, voucher[1], SectionInfo.LogAdminName, transactionInstance.xAmount);
                                        new FinancialLogRepository().Log(Convert.ToInt64(ViewBag.UserID), FinancialLogType.MiscCost, "هزینه صدور وچر پرفکت", SectionInfo.LogAdminName,Math.Round(transactionInstance.xAmount*1.0/SectionInfo.Setting.PerfectWithdrawalCostPercent,2));
                                        arbitarySuccessMessage = "وچر برای شما صادر گشت می توانید از قسمت وچر ها ، اطلاعات وچر را نیز بعدا ببینید";
                                        ViewBag.Voucher = voucher[1];
                                    }
                                    else
                                    {
                                        //userInstance.xWallet += transactionInstance.xAmount;
                                        new FinancialLogRepository().Log(Convert.ToInt64(ViewBag.UserID), FinancialLogType.ChargeWallet, "خطا در گرفتن پرفکت", SectionInfo.LogAdminName, transactionInstance.xAmount);
                                        arbitarySuccessMessage = "با عرض پوزش به دلیل کم بود پرفکت وچر صادر نشد اما ولت شما شارژ گردید به زودی پرفکت مانی شارژ می گردد و می توانید از ولت خود تقاضای دریافت وچر نمایید";
                                    }
                                }
                                else
                                {
                                    //userInstance.xWallet += transactionInstance.xAmount;
                                    new FinancialLogRepository().Log(Convert.ToInt64(ViewBag.UserID), FinancialLogType.ChargeWallet,"کم بود پرفکت", SectionInfo.LogAdminName, transactionInstance.xAmount);
                                    arbitarySuccessMessage = "با عرض پوزش به دلیل خطایی در تولید وچر ، وچر برای شما صادر نشده است اما ولت شما شارژ گردید لطفا دقایقی بعد دوباره تلاش کنید";
                                }
                                
                                

                                gatewayInstance.xLastSuccessTransactionDate = DateTime.Now;
                                gatewayInstance.xTodayTotalTransactionAmounts += transactionInstance.xAmount;
                                gr.Update(gatewayInstance);


                                if (transactionInstance.xComissionAmount != 0)
                                {
                                    new FinancialLogRepository().Log(Convert.ToInt64(ViewBag.UserID), FinancialLogType.GatewayCommision, SectionInfo.Setting.Languages[ViewBag.LanguageCode][EnumUtils.GetAttribute<DisplayAttribute>(FinancialLogType.GatewayCommision).Name] + " - " + gatewayInstance.xID + " - " + gatewayInstance.xCommisionPercent, SectionInfo.LogAdminName, transactionInstance.xComissionAmount);
                                }

                                userInstance.xLevel += transactionInstance.xAmount * 1.0 / SectionInfo.Setting.UserLevelIncreaseEach;
                                ur.Update(userInstance);

                                if(userInstance.xAgentID!=null)
                                {
                                    var agentInstance = ur.GetByID(userInstance.xAgentID);
                                    if(agentInstance!=null)
                                    {
                                        var amount = Convert.ToInt64(((SectionInfo.Setting.PerfectMoneySellPrice - SectionInfo.Setting.PerfectMoneyBuyPrice) * agentInstance.xRefferalSharePercent / 100) * requestedPM);
                                        agentInstance.xWallet += amount;
                                        new FinancialLogRepository().Log(agentInstance.xID, FinancialLogType.RefferalShare, string.Format("قیمت فروش {0} | قیمت خرید :{1} | میزان پرفکت در خواستی کاربر : {2} | درصد سهم شما :{3}", SectionInfo.Setting.PerfectMoneySellPrice, SectionInfo.Setting.PerfectMoneyBuyPrice, requestedPM, agentInstance.xRefferalSharePercent), SectionInfo.LogAdminName, amount);
                                        ur.Update(agentInstance);
                                    }
                                }

                                transactionInstance.xVerified = true;
                                transactionInstance.xPaymentInfo = paymentInfo;
                                transactionInstance.xBankRef = bankCode;
                                tr.Update(transactionInstance);

                                if (arbitarySuccessMessage == "")
                                {
                                    ViewBag.Message = arbitarySuccessMessage;// SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_PaymentSuccess"];
                                }
                                else
                                {
                                    ViewBag.Message = arbitarySuccessMessage;// SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_PaymentSuccessButTopupFailed"];
                                    
                                }
                                
                                ViewBag.Title=SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Success"];
                                ViewBag.ConfirmButtonText=SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Ok"];
                                ViewBag.Type = "success";

                            }
                            
                        }
                        else
                        {
                            ViewBag.Message = SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_TransactionAlreadySubmitted"];
                            ViewBag.Title = SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Error"];
                            ViewBag.ConfirmButtonText = SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Ok"];
                            ViewBag.Type = "warning";
                        }
                    }
                    else
                    {
                        ViewBag.Message = SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_PaymentVeirifcationException"];
                        ViewBag.Title = SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Error"];
                        ViewBag.ConfirmButtonText = SectionInfo.Setting.Languages[ViewBag.LanguageCode]["Txt_Ok"];
                        ViewBag.Type = "warning";
                    }


                }
                return View("~/Views/Home/Notify.cshtml");


            }
                
        }


        public ActionResult Page404()
        {
            return View();
        }

        public ActionResult Page500()
        {
            return View();
        }
        public ActionResult Notify()
        {
            return View();
        }

        public ActionResult MaintenanceMode()
        {
            if (!SectionInfo.Setting.MaintenanceMode)
            {
                return Redirect("Index");
            }
            return View();
        }
    }
}