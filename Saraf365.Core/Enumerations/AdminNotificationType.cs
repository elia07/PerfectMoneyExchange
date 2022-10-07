using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saraf365.Core.Enumerations
{
    public enum AdminNotificationType
    {
        [Display(Name = "صحت سنجی مدارک کاربر")]
        ValidateUserIDCart,
        [Display(Name = "کش اوت جدید")]
        NewWithdrawal,
        [Display(Name = "تیکت جدید")]
        NewTicket,
        [Display(Name = "TelegramSupport")]
        TelegramSupport,
        [Display(Name = "حساب بانکی کاربر")]
        AddUserBankAccount
    }
}
