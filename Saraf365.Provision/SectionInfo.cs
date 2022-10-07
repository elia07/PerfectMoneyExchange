using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RockCandy.Web.Framework.Core.Models;
using RockCandy.Web.Framework.Core.Enumerations;
using RockCandy.Web.Framework.Core;
using Saraf365.Core.Enumerations;
using System.Configuration;
using Saraf365.Core.Models;
using Quartz;
using Quartz.Impl;
using Saraf365.Core.Repositories;
using Quartz.Impl.Matchers;
using Saraf365.Core.InternetBankModules;

namespace Saraf365.Provision
{
    public static partial class SectionInfo
    {
        public static string LogAddress = AppDomain.CurrentDomain.BaseDirectory + "log.txt";
        public static string LogAdminName = "Provision Log";
        public static Setting Setting=null;
        public static string AssemblyDirectory = AppDomain.CurrentDomain.BaseDirectory;

        public static IScheduler SchedulerSetting = StdSchedulerFactory.GetDefaultScheduler();
        public static Dictionary<IJobDetail, ITrigger> dynamicTasks = new Dictionary<IJobDetail, ITrigger>();
        public static Dictionary<long,IInternetBank> InputCardsInternetBanks = new Dictionary<long, IInternetBank>();

        static SectionInfo()
        {
           
        }
        public static void init()
        {
            foreach(var item in dynamicTasks)
            {
                SchedulerSetting.DeleteJob(item.Key.Key);
            }

            dynamicTasks = new Dictionary<IJobDetail, ITrigger>();
            Setting = new Setting();
            if (!SchedulerSetting.IsStarted)
            {
                SchedulerSetting.Start();
            }

            
            using (ScheduleMessageRepository smr = new ScheduleMessageRepository())
            {
                foreach(var item in smr.GetAll().Where(x=>x.xIsActive))
                {
                    IJobDetail JobInstance = JobBuilder.Create<ScheduleMessage>().WithDescription(item.xTitle).UsingJobData("xID", item.xID).Build();
                    ITrigger triggerInstance = TriggerBuilder.Create().WithCronSchedule(item.xExpression).WithDescription(item.xTitle).Build();
                    //ITrigger triggerInstance = TriggerBuilder.Create().WithSimpleSchedule(x => x.WithIntervalInMinutes(1).WithRepeatCount(1)).WithDescription("HandEventParser").Build();
                    SchedulerSetting.ScheduleJob(JobInstance, triggerInstance);
                    dynamicTasks.Add(JobInstance, triggerInstance);
                }
            }


            


            

        }

    }
}