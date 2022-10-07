using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saraf365.Core.Enumerations
{
    public enum GatewayType
    {
        [Display(Name = "Txt_IPG")]
        IPG,
        [Display(Name = "Txt_CartBeCart")]
        CartBeCart,
        [Display(Name = "Txt_PerfectMoney")]
        PerfectMoney,
        [Display(Name = "Txt_Voucher")]
        Voucher,
        [Display(Name = "Txt_Bitcoin")]
        Bitcoin,


    }
}
