using Saraf365.Core;
using Saraf365.Core.Enumerations;
using Saraf365.Core.Repositories;
using Saraf365.Core.Utils;
using Quartz;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Saraf365.Provision
{
    public class PerfectMoney : IJob
    {
        private static int Worker = 0;
        public void Manage()
        {
            string imageAddress = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources\\Images\\perfectMoneyForSocial.png");
            if (System.IO.File.Exists(imageAddress))
            {
                new TelegramUtils().SendPhoto(System.IO.File.ReadAllBytes(imageAddress), Path.GetFileName(imageAddress), string.Format("قیمت امروز پرفکت مانی\r\n خرید از شما : {0} تومان \r\n فروش به شما : {2} تومان",SectionInfo.Setting.PerfectMoneyBuyPrice,SectionInfo.Setting.PerfectMoneySellPrice), SectionInfo.Setting.TelegramBotAccessToken, SectionInfo.Setting.TelegramChannel);
            }
            else
            {
                new TelegramUtils().SendMessage(SectionInfo.Setting.TelegramBotAccessToken, SectionInfo.Setting.TelegramChannel, string.Format("قیمت امروز پرفکت مانی\r\n خرید از شما : {0} تومان \r\n فروش به شما : {2} تومان", SectionInfo.Setting.PerfectMoneyBuyPrice, SectionInfo.Setting.PerfectMoneySellPrice), SectionInfo.Setting.TelegramMessageAppendText);
            }
            /*if(SectionInfo.Setting.PerfectBuyAvail && SectionInfo.Setting.PerfectSellAvail)
            {
                
            }
            else if(SectionInfo.Setting.PerfectSellAvail)
            {

            }
            else if (SectionInfo.Setting.PerfectBuyAvail)
            {

            }*/


            new SystemLogRepository().Log(SystemLogType.PerfectMoneyJob, "ارسال قیمت پرفکت مانی به شبکه های مجازی", "");
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