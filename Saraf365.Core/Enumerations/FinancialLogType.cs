using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saraf365.Core.Enumerations
{
    public enum FinancialLogType
    {
        [Display(Name = "Txt_BuyIn")]
        BuyIn,
        [Display(Name = "Txt_GatewayCommision")]
        GatewayCommision,
        [Display(Name = "Txt_ShareHolderWithdrawal")]
        ShareHolderWithdrawal,
        [Display(Name = "Txt_MiscCost")]
        MiscCost,
        [Display(Name = "Txt_UserVoucher")]
        UserVoucher,
        [Display(Name = "Txt_ChargeWallet")]
        ChargeWallet,
        [Display(Name = "Txt_WithdrawalAccept")]
        WithdrawalAccept,
        [Display(Name = "Txt_RefferalShare")]
        RefferalShare

    }
}
