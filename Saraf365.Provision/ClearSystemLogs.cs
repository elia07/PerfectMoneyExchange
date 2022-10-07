using Saraf365.Core;
using Saraf365.Core.Enumerations;
using Saraf365.Core.Repositories;
using Quartz;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Saraf365.Provision
{
    public class ClearSystemLogs : IJob
    {
        private static int Worker = 0;
        public void Manage()
        {
            using (SystemLogRepository slr = new SystemLogRepository())
            {
                slr.DeleteByDateAndType(DateTime.Now.Date.AddDays(-1 * SectionInfo.Setting.ClearBackOfficeLogsAfterDays), Core.Enumerations.SystemLogType.BO_Administration);
                slr.DeleteByDateAndType(DateTime.Now.Date.AddDays(-1 * SectionInfo.Setting.ClearBackOfficeLogsAfterDays), Core.Enumerations.SystemLogType.BO_Bot);
                slr.DeleteByDateAndType(DateTime.Now.Date.AddDays(-1 * SectionInfo.Setting.ClearBackOfficeLogsAfterDays), Core.Enumerations.SystemLogType.BO_CashGame);
                slr.DeleteByDateAndType(DateTime.Now.Date.AddDays(-1 * SectionInfo.Setting.ClearBackOfficeLogsAfterDays), Core.Enumerations.SystemLogType.BO_Gateway);
                slr.DeleteByDateAndType(DateTime.Now.Date.AddDays(-1 * SectionInfo.Setting.ClearBackOfficeLogsAfterDays), Core.Enumerations.SystemLogType.BO_Login);
                slr.DeleteByDateAndType(DateTime.Now.Date.AddDays(-1 * SectionInfo.Setting.ClearBackOfficeLogsAfterDays), Core.Enumerations.SystemLogType.BO_Setting);
                slr.DeleteByDateAndType(DateTime.Now.Date.AddDays(-1 * SectionInfo.Setting.ClearBackOfficeLogsAfterDays), Core.Enumerations.SystemLogType.BO_Tourney);
                slr.DeleteByDateAndType(DateTime.Now.Date.AddDays(-1 * SectionInfo.Setting.ClearBackOfficeLogsAfterDays), Core.Enumerations.SystemLogType.BO_User);
                slr.DeleteByDateAndType(DateTime.Now.Date.AddDays(-1 * SectionInfo.Setting.ClearBackOfficeLogsAfterDays), Core.Enumerations.SystemLogType.BO_Withdrawal);
                slr.DeleteByDateAndType(DateTime.Now.Date.AddDays(-1 * SectionInfo.Setting.ClearBackOfficeLogsAfterDays), Core.Enumerations.SystemLogType.BO_BankCard);
                slr.DeleteByDateAndType(DateTime.Now.Date.AddDays(-1 * SectionInfo.Setting.ClearBackOfficeLogsAfterDays), Core.Enumerations.SystemLogType.CashGameTrackerJob);
                slr.DeleteByDateAndType(DateTime.Now.Date.AddDays(-1 * SectionInfo.Setting.ClearBackOfficeLogsAfterDays), Core.Enumerations.SystemLogType.ClearSystemLogJob);
                slr.DeleteByDateAndType(DateTime.Now.Date.AddDays(-1 * SectionInfo.Setting.ClearBackOfficeLogsAfterDays), Core.Enumerations.SystemLogType.ClearTelegramSupportJob);
                slr.DeleteByDateAndType(DateTime.Now.Date.AddDays(-1 * SectionInfo.Setting.ClearBackOfficeLogsAfterDays), Core.Enumerations.SystemLogType.ClearUserBalanceChangeJob);
                slr.DeleteByDateAndType(DateTime.Now.Date.AddDays(-1 * SectionInfo.Setting.ClearBackOfficeLogsAfterDays), Core.Enumerations.SystemLogType.LeaderBoardGift);
                slr.DeleteByDateAndType(DateTime.Now.Date.AddDays(-1 * SectionInfo.Setting.ClearBackOfficeLogsAfterDays), Core.Enumerations.SystemLogType.RackBackCalculateJob);
                slr.DeleteByDateAndType(DateTime.Now.Date.AddDays(-1 * SectionInfo.Setting.ClearBackOfficeLogsAfterDays), Core.Enumerations.SystemLogType.ResetBankCardTodayTransactionsJob);
                slr.DeleteByDateAndType(DateTime.Now.Date.AddDays(-1 * SectionInfo.Setting.ClearBackOfficeLogsAfterDays), Core.Enumerations.SystemLogType.ResetGatewayTodayTotalTransactionsJob);
                slr.DeleteByDateAndType(DateTime.Now.Date.AddDays(-1 * SectionInfo.Setting.ClearBackOfficeLogsAfterDays), Core.Enumerations.SystemLogType.ResetTodayDailyGiftJob);
                slr.DeleteByDateAndType(DateTime.Now.Date.AddDays(-1 * SectionInfo.Setting.ClearBackOfficeLogsAfterDays), Core.Enumerations.SystemLogType.RingGameError);
                slr.DeleteByDateAndType(DateTime.Now.Date.AddDays(-1 * SectionInfo.Setting.ClearBackOfficeLogsAfterDays), Core.Enumerations.SystemLogType.TourneyCancel);
                slr.DeleteByDateAndType(DateTime.Now.Date.AddDays(-1 * SectionInfo.Setting.ClearBackOfficeLogsAfterDays), Core.Enumerations.SystemLogType.TourneyError);
                slr.DeleteByDateAndType(DateTime.Now.Date.AddDays(-1 * SectionInfo.Setting.ClearBackOfficeLogsAfterDays), Core.Enumerations.SystemLogType.TourneyFinish);
                slr.DeleteByDateAndType(DateTime.Now.Date.AddDays(-1 * SectionInfo.Setting.ClearBackOfficeLogsAfterDays), Core.Enumerations.SystemLogType.TourneyStart);
                slr.DeleteByDateAndType(DateTime.Now.Date.AddDays(-1 * SectionInfo.Setting.ClearBackOfficeLogsAfterDays), Core.Enumerations.SystemLogType.PerfectMoneyJob);
                slr.DeleteByDateAndType(DateTime.Now.Date.AddDays(-1 * SectionInfo.Setting.ClearBackOfficeLogsAfterDays), Core.Enumerations.SystemLogType.ScheduleMessageJob);
                slr.DeleteByDateAndType(DateTime.Now.Date.AddDays(-1 * SectionInfo.Setting.ClearBackOfficeLogsAfterDays), Core.Enumerations.SystemLogType.PerfectMoneyApiCall);
                slr.DeleteByDateAndType(DateTime.Now.Date.AddDays(-1 * SectionInfo.Setting.ClearBackOfficeLogsAfterDays), Core.Enumerations.SystemLogType.InternetBank);
                slr.DeleteByDateAndType(DateTime.Now.Date.AddDays(-1 * SectionInfo.Setting.ClearBackOfficeLogsAfterDays), Core.Enumerations.SystemLogType.CardTransferHistory);

            }

            new SystemLogRepository().Log(SystemLogType.ClearSystemLogJob, "پاکسازی لاگ ها", "");
        }
       

        void IJob.Execute(IJobExecutionContext context)
        {
            Worker++;
            if (Worker > 1)
            {
                Worker--;
                return;
            }
            try
            {
                Manage();
            }
            catch { }
            Worker--;
        }
    }
}