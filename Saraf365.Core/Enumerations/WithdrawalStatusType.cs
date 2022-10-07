using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saraf365.Core.Enumerations
{
    public enum WithdrawalStatusType
    {
        [Display(Name = "Txt_WaitingForAccept")]
        WaitingForAccept,
        [Display(Name = "Txt_InPaymentList")]
        InPaymentList,
        [Display(Name = "Txt_Rejected")]
        Rejected,
        [Display(Name = "Txt_Paied")]
        Paied,

    }
}
