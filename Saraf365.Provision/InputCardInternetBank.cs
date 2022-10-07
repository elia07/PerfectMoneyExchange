using Saraf365.Core;
using Saraf365.Core.Enumerations;
using Saraf365.Core.InternetBankModules;
using Saraf365.Core.Repositories;
using Quartz;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Saraf365.Provision
{
    public class InputCardInternetBank : IJob
    {
        private static int Worker = 0;
        public void Manage()
        {
            SectionInfo.InputCardsInternetBanks.Clear();
            using (BankCardRepository bcr = new BankCardRepository())
            {
                foreach(var item in bcr.GetInputCards())
                {
                    var instance = new InternetBankFactory().GetInstance(item,SectionInfo.Setting.AntiCaptchaApiKey);
                    if(instance!=null)
                    {
                        SectionInfo.InputCardsInternetBanks.Add(item.xID, instance);
                    }
                    
                }
            }
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