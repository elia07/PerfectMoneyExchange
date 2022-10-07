using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RockCandy.Web.Framework.Core.Models;
using RockCandy.Web.Framework.Core.Enumerations;
using RockCandy.Web.Framework.Core;
using Saraf365.Core.Enumerations;
using System.Configuration;
using Saraf365.Core;
using Saraf365.Core.Models;
using Saraf365.Core.Repositories;
using Newtonsoft.Json;
using Saraf365.Core.InternetBankModules;

namespace Saraf365.Backoffice
{
    public static partial class SectionInfo
    {
        public static string LogAddress = AppDomain.CurrentDomain.BaseDirectory + "log.txt";
        public static string LogAdminName = "Business Logic";
        public static Saraf365.Core.Models.Setting Setting = null;
        public static Dictionary<long, IInternetBank> InternetBanks = new Dictionary<long, IInternetBank>();

        public static JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Formatting = Formatting.Indented
        };
        static SectionInfo()
        {
           
        }
        public static void init()
        {

            Setting = new Saraf365.Core.Models.Setting();

            DomainInfo.ControllerMentionFields = new Dictionary<string, List<MentionField>>();
            List<MentionField> temp = new List<MentionField>();

            temp = new List<MentionField>();
            temp.Add(new MentionField("xType", "نوع", typeof(AdminType)));
            temp.Add(new MentionField("xEmail", "عنوان", typeof(string)));
            temp.Add(new MentionField("xName", "نام", typeof(string)));
            DomainInfo.ControllerMentionFields.Add("Admin", temp);


            temp = new List<MentionField>();
            temp.Add(new MentionField("xEmail", "پست الکترونیک", typeof(string)));
            temp.Add(new MentionField("xNationalIDNumber", "کد ملی", typeof(string)));
            temp.Add(new MentionField("xFullName", "نام", typeof(string)));
            temp.Add(new MentionField("xCellphone", "تلفن همراه", typeof(string)));
            temp.Add(new MentionField("xLevel", "لول", typeof(long)));
            temp.Add(new MentionField("xIPGRestriction", "محدودیت استفاده از در گاه", typeof(DateTime)));
            temp.Add(new MentionField("xSignupDate", "تاریخ ثبت نام", typeof(DateTime)));
            temp.Add(new MentionField("xLastSigninDate", "تاریخ آخرین ورود", typeof(DateTime)));
            temp.Add(new MentionField("xIsActive", "فعال", typeof(bool)));
            temp.Add(new MentionField("xIsEmailValidated", "صحت سنجی ایمیل", typeof(bool)));
            temp.Add(new MentionField("xIsNationalIDValidated", "صحت سنجی کد ملی", typeof(bool)));
            temp.Add(new MentionField("xCellphoneActivated", "صحت سنجی موبایل", typeof(bool)));
            temp.Add(new MentionField("xIsVip", "کاربر Vip", typeof(bool)));
            temp.Add(new MentionField("xHasNotVerifiedBankAccount", "وضعیت حساب های بانکی", typeof(bool)));
            DomainInfo.ControllerMentionFields.Add("User", temp);



            
            temp = new List<MentionField>();
            temp.Add(new MentionField("xEmail", "نام کاربری", typeof(string)));
            temp.Add(new MentionField("xTitle", "عنوان", typeof(string)));
            temp.Add(new MentionField("xDate", "تاریخ", typeof(DateTime)));
            temp.Add(new MentionField("xType", "نوع", typeof(TicketType)));
            temp.Add(new MentionField("xStatus", "وضعیت", typeof(TicketStatusType)));
            DomainInfo.ControllerMentionFields.Add("Ticket", temp);


            temp = new List<MentionField>();
            temp.Add(new MentionField("xEmail", "نام کاربری", typeof(string)));
            temp.Add(new MentionField("xCartNumber", "شماره کارت", typeof(string)));
            temp.Add(new MentionField("xAccountNumber", "شماره حساب", typeof(string)));
            temp.Add(new MentionField("xShebaNumber", "شماره شبا", typeof(string)));
            temp.Add(new MentionField("xDate", "تاریخ", typeof(DateTime)));
            temp.Add(new MentionField("xSettlementDate", "تاریخ واریز", typeof(DateTime)));
            temp.Add(new MentionField("xAmount", "مبلغ", typeof(long)));
            temp.Add(new MentionField("xStatus", "وضعیت", typeof(TicketStatusType)));
            DomainInfo.ControllerMentionFields.Add("Withdrawal", temp);








            temp = new List<MentionField>();
            temp.Add(new MentionField("xFileName", "نام فایل", typeof(string)));
            DomainInfo.ControllerMentionFields.Add("SystemFile", temp);



            temp = new List<MentionField>();
            temp.Add(new MentionField("xName", "نام", typeof(string)));
            temp.Add(new MentionField("xType", "نوع", typeof(GatewayType)));
            temp.Add(new MentionField("xMaxDailyAmount", "سقف روزانه", typeof(long)));
            temp.Add(new MentionField("xActiveforLevelsAbove", "فعال برای کاربران بالای لول", typeof(long)));
            temp.Add(new MentionField("xCommisionPercent", "درصد کارمزد", typeof(double)));
            temp.Add(new MentionField("xMinBuyIn", "کف خرید", typeof(long)));
            temp.Add(new MentionField("xTodayTotalTransactionAmounts", "میزان تراکنش های امروز", typeof(long)));
            temp.Add(new MentionField("xAllowCustomAmountForBuyIn", "خرید با مبلغ دلخواه", typeof(bool)));
            temp.Add(new MentionField("xIsActive", "فعال", typeof(bool)));
            temp.Add(new MentionField("xIsPrimary", "اصلی", typeof(bool)));
            temp.Add(new MentionField("xIsVip", "برای کاربران Vip", typeof(bool)));
            temp.Add(new MentionField("xLastSuccessTransactionDate", "تاریخ آخرین تراکنش موفق", typeof(DateTime)));            
            DomainInfo.ControllerMentionFields.Add("Gateway", temp);



            temp = new List<MentionField>();
            temp.Add(new MentionField("xEmail", "نام کاربر", typeof(string)));
            temp.Add(new MentionField("xGatewayName", "نام درگاه", typeof(string)));
            temp.Add(new MentionField("xPaymentInfo", "توضیحات تراکنش", typeof(string)));
            temp.Add(new MentionField("xBankRef", "xBankRef", typeof(string)));
            temp.Add(new MentionField("xInvoice_key", "xInvoice_key", typeof(string)));
            temp.Add(new MentionField("xDate", "تاریخ", typeof(DateTime)));
            temp.Add(new MentionField("xAmount", "مبلغ", typeof(long)));          
            temp.Add(new MentionField("xVerified", "تایید شده", typeof(bool)));
            temp.Add(new MentionField("xIsSuspicious", "مشکوک", typeof(bool)));
            temp.Add(new MentionField("xValidationCheck", "صحت سنجی شده", typeof(bool)));
            temp.Add(new MentionField("xCommisionPercent", "درصد کارمزد", typeof(double)));
            temp.Add(new MentionField("xComissionAmount", "میزان کارمزد", typeof(long)));
            DomainInfo.ControllerMentionFields.Add("Transaction", temp);



            temp = new List<MentionField>();
            temp.Add(new MentionField("xEmail", "کاربر", typeof(string)));
            temp.Add(new MentionField("xCardNumber", "ش.ک کاربر", typeof(string)));
            temp.Add(new MentionField("xDocumentNumber", "شماره سند", typeof(string)));
            temp.Add(new MentionField("xCodePeigiri", "کد پیگیری", typeof(string)));
            temp.Add(new MentionField("xCodeErja", "کد ارجا", typeof(string)));
            temp.Add(new MentionField("xBankCardNumber", "ش.ک", typeof(string)));
            temp.Add(new MentionField("xBankAccountNumber", "ش.ح", typeof(string)));
            temp.Add(new MentionField("xTransactionID", "شناسه تراکنش", typeof(long)));
            temp.Add(new MentionField("xAmountIn", "مبلغ واریز", typeof(long)));
            temp.Add(new MentionField("xAmountOut", "مبلغ برداشت", typeof(long)));
            temp.Add(new MentionField("xDate", "تاریخ", typeof(DateTime)));
            temp.Add(new MentionField("xDocumentDate", "تاریخ سند", typeof(DateTime)));
            DomainInfo.ControllerMentionFields.Add("CartTransferHistory", temp);





            temp = new List<MentionField>();
            temp.Add(new MentionField("xShortDetail", "خلاصه لاگ", typeof(string)));
            temp.Add(new MentionField("xAdminEmail", "ایمیل مدیر", typeof(string)));
            temp.Add(new MentionField("xType", "نوع", typeof(SystemLogType)));
            temp.Add(new MentionField("xDate", "تاریخ", typeof(DateTime)));
            DomainInfo.ControllerMentionFields.Add("SystemLog", temp);



            temp = new List<MentionField>();
            temp.Add(new MentionField("xEmail", "نام کاربری", typeof(string)));
            temp.Add(new MentionField("xDescription", "توضیحات", typeof(string)));
            temp.Add(new MentionField("xAddBy", "ایجاد سده توسط", typeof(string)));
            temp.Add(new MentionField("xHandNumber", "شماره دست", typeof(string)));
            temp.Add(new MentionField("xType", "نوع", typeof(SystemLogType)));
            temp.Add(new MentionField("xDate", "تاریخ", typeof(DateTime)));
            temp.Add(new MentionField("xAmount", "مبلغ", typeof(long)));
            DomainInfo.ControllerMentionFields.Add("FinancialLog", temp);



            DomainInfo.ControllerMentionFields.Add("AdminNotification", new List<MentionField>());


            temp = new List<MentionField>();
            temp.Add(new MentionField("xTitle", "عنوان", typeof(string)));
            temp.Add(new MentionField("xIsActive", "فعال", typeof(bool)));
            DomainInfo.ControllerMentionFields.Add("ScheduleMessage", temp);



            temp = new List<MentionField>();
            temp.Add(new MentionField("xUsername", "نام کاربری", typeof(string)));
            temp.Add(new MentionField("xMessage", "پیام", typeof(string)));
            temp.Add(new MentionField("xDate", "تاریخ", typeof(DateTime)));
            temp.Add(new MentionField("xResponseDate", "تاریخ پاسخ", typeof(DateTime)));
            DomainInfo.ControllerMentionFields.Add("TelegramSupport", temp);


            temp = new List<MentionField>();
            temp.Add(new MentionField("xKey", "عنوان", typeof(string)));
            DomainInfo.ControllerMentionFields.Add("Setting", temp);








            temp = new List<MentionField>();
            temp.Add(new MentionField("xCardHolderName", "نام صاحب کارت", typeof(string)));
            temp.Add(new MentionField("xBankName", "نام بانک", typeof(string)));
            temp.Add(new MentionField("xAccountNumber", "شماره حساب", typeof(string)));
            temp.Add(new MentionField("xCardNumber", "شماره کارت", typeof(string)));
            temp.Add(new MentionField("xBalance", "موجودی", typeof(long)));
            temp.Add(new MentionField("xType", "نوع", typeof(BankCardType)));
            temp.Add(new MentionField("xIsActive", "فعال", typeof(bool)));
            DomainInfo.ControllerMentionFields.Add("BankCard", temp);


        }

    }
}