using Saraf365.Core;
using Saraf365.Core.Enumerations;
using Saraf365.Core.Repositories;
using Saraf365.Core.Utils;
using Quartz;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Saraf365.Provision
{
    public class ScheduleMessage : IJob
    {
        private static long xID = 0;
        private static int Worker = 0;
        public async Task<bool>  Manage()
        {
            using (ScheduleMessageRepository smr = new ScheduleMessageRepository())
            {
                var instance = smr.GetByID(xID);
                if(instance!=null)
                {
                    
                    if (instance.xSystemFileID!=null && instance.xBody.Length<=140)
                    {
                        var rest = await new TelegramUtils().SendPhoto(instance.SystemFile.FileData.Where(x=>x.xIsThumbnail==false).Single().xData, instance.SystemFile.xFileName,instance.xBody, SectionInfo.Setting.TelegramBotAccessToken, SectionInfo.Setting.TelegramChannel);
                    }
                    else
                    {
                        var rest = await new TelegramUtils().SendPhoto(instance.SystemFile.FileData.Where(x => x.xIsThumbnail == false).Single().xData, instance.SystemFile.xFileName, instance.xTitle, SectionInfo.Setting.TelegramBotAccessToken, SectionInfo.Setting.TelegramChannel);
                        var rest2 = await new TelegramUtils().SendMessage(SectionInfo.Setting.TelegramBotAccessToken, SectionInfo.Setting.TelegramChannel, instance.xBody, SectionInfo.Setting.TelegramMessageAppendText);
                    }
                    
                }
            }
                
            new SystemLogRepository().Log(SystemLogType.ScheduleMessageJob, "ارسال پیام زمانبندی شده", xID.ToString());
            return true;
        }
       

        void IJob.Execute(IJobExecutionContext context)
        {
            
            Worker++;
            JobDataMap dataMap = context.JobDetail.JobDataMap;
            xID = Convert.ToInt64(dataMap.GetLong("xID"));
            Manage();
            Worker--;
        }
    }
}