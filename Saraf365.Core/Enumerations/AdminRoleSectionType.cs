using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saraf365.Core.Enumerations
{
    public enum AdminRoleSectionType
    {
        [Display(Name = "مدیریت مدیران")]
        AdminManagement,
        [Display(Name = "مدیریت کاربران")]
        UserManagement,
        [Display(Name = "مدیریت تیکت ها")]
        TicketManagement,
        [Display(Name = "مدیریت تسویه حساب ها")]
        WithdrawalManagement,
        [Display(Name = "مدیریت آپلود فایل ها")]
        SystemFileManagement,
        [Display(Name = "ارسال به شبکه های اجتماعی")]
        SendToSocial,
        [Display(Name = "مدیریت درگاه ها")]
        GatewayManagement,
        [Display(Name = "مدیریت تراکنش ها")]
        TransactionManagement,
        [Display(Name = "مدیریت کارت به کارت ها")]
        CartTransferHistoryManagement,
        [Display(Name = "مدیریت لاگ های سیستم")]
        SystemLogManagement,
        [Display(Name = "مدیریت لاگ های مالی")]
        FinancialLogManagement,
        [Display(Name = "مدیریت پیام های زمانبندی شده")]
        ScheduleMessageManagement,
        [Display(Name = "مدیریت ساپورت تلگرام")]
        TelegramSupportManagement,
        [Display(Name = "مدیریت تنظیمات")]
        SettingManagement,
        [Display(Name = "مدیریت مالی کاربران")]
        UserFinancialManagement,
        [Display(Name = "مدیریت کارت های بانکی")]
        BankCardManagement
    }
}
