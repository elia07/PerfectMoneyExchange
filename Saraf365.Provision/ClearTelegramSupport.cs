using Newtonsoft.Json;
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
    public class ClearTelegramSupport : IJob
    {
        private static int Worker = 0;
        public void Manage()
        {
            List<long> FilesToDetele = new List<long>();
            using (SettingRepository sr = new SettingRepository())
            {
                
                Dictionary<string, long> users = new Dictionary<string, long>();
                try
                {
                    users = JsonConvert.DeserializeObject<Dictionary<string, long>>(sr.GetByKey("TelegramUsernames"));
                }
                catch { }
                using (TelegramSupportRepository tsr = new TelegramSupportRepository())
                {
                    foreach (var item in tsr.GetAllBeforeDate(DateTime.Now.Date.AddDays(-1 * SectionInfo.Setting.ClearTelegramSupportAfterDays)))
                    {
                        if (!users.ContainsKey(item.xUsername))
                        {
                            users.Add(item.xUsername, item.xChatID);
                        }
                            if (item.xSystemFileID != null)
                            {
                                FilesToDetele.Add(Convert.ToInt64(item.xSystemFileID));
                            }

                        tsr.Delete(item);

                    }
                }
                var telegramUsersInstance = sr.GetBy("TelegramUsernames");
                telegramUsersInstance.xValue = JsonConvert.SerializeObject(users);
                sr.Update(telegramUsersInstance);
            }
            
            foreach (var item in FilesToDetele)
            {
                try
                {
                    using (SystemFileRepository sfr = new SystemFileRepository())
                    {
                        sfr.DeleteWithFileData(item);
                    }
                }
                catch
                {

                }
            }


            new SystemLogRepository().Log(SystemLogType.ClearTelegramSupportJob, "پاکسازی سوابق ساپورت تلگرام", "");
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