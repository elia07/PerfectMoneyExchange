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
    public class ResetGatewayTodayTotalTransactions : IJob
    {
        private static int Worker = 0;
        public void Manage()
        {
            using (GatewayRepository gr = new GatewayRepository(null, true))
            {
                foreach (var item in gr.GetAll())
                {
                    item.xTodayTotalTransactionAmounts = 0;
                    gr.Update(item);
                }
            }

            new SystemLogRepository().Log(SystemLogType.ResetGatewayTodayTotalTransactionsJob, "پاکسازی حجم تراکنش های امروز درگاه ها", "");
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