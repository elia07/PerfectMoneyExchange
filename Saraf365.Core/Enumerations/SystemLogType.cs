using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saraf365.Core.Enumerations
{
    public enum SystemLogType
    {
        [Display(Name = "RingGameError")]
        RingGameError,
        [Display(Name = "TourneyError")]
        TourneyError,
        [Display(Name = "TourneyStart")]
        TourneyStart,
        [Display(Name = "TourneyCancel")]
        TourneyCancel,
        [Display(Name = "TourneyFinish")]
        TourneyFinish,
        [Display(Name = "BO_Administration")]
        BO_Administration,
        [Display(Name = "BO_User")]
        BO_User,
        [Display(Name = "BO_Withdrawal")]
        BO_Withdrawal,
        [Display(Name = "BO_Gateway")]
        BO_Gateway,
        [Display(Name = "BO_Setting")]
        BO_Setting,
        [Display(Name = "BO_Bot")]
        BO_Bot,
        [Display(Name = "BO_CashGame")]
        BO_CashGame,
        [Display(Name = "BO_Tourney")]
        BO_Tourney,
        [Display(Name = "BO_Login")]
        BO_Login,
        [Display(Name = "BO_BankCard")]
        BO_BankCard,
        [Display(Name = "LeaderBoardGift")]
        LeaderBoardGift,
        [Display(Name = "RackBackCalculateJob")]
        RackBackCalculateJob,
        [Display(Name = "CashGameTrackerJob")]
        CashGameTrackerJob,
        [Display(Name = "ClearTelegramSupportJob")]
        ClearTelegramSupportJob,
        [Display(Name = "ClearUserBalanceChangeJob")]
        ClearUserBalanceChangeJob,
        [Display(Name = "ResetBankCardTodayTransactionsJob")]
        ResetBankCardTodayTransactionsJob,
        [Display(Name = "ResetGatewayTodayTotalTransactionsJob")]
        ResetGatewayTodayTotalTransactionsJob,
        [Display(Name = "ResetTodayDailyGiftJob")]
        ResetTodayDailyGiftJob,
        [Display(Name = "ClearSystemLogJob")]
        ClearSystemLogJob,
        [Display(Name = "ScheduleMessageJob")]
        ScheduleMessageJob,
        [Display(Name = "PerfectMoneyJob")]
        PerfectMoneyJob,
        [Display(Name = "PerfectMoneyApiCall")]
        PerfectMoneyApiCall,
        [Display(Name = "InternetBank")]
        InternetBank,
        [Display(Name = "CardTransferHistory")]
        CardTransferHistory
    }
}
