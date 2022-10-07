using Saraf365.Core;
using Saraf365.Core.Repositories;
using Quartz;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Saraf365.Backoffice
{
    public class SettingUpdater : IJob
    {
        private static int Worker = 0;
        public void Manage()
        {

            SectionInfo.init();

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