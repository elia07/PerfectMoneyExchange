using Newtonsoft.Json;
using Quartz;
using Quartz.Impl;
using RockCandy.Web.Framework.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Saraf365.Backoffice
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            SectionInfo.init();

            IScheduler schedulerSetting = StdSchedulerFactory.GetDefaultScheduler();
            schedulerSetting.Start();

            IJobDetail jobSetting = JobBuilder.Create<SettingUpdater>().Build();

            ITrigger triggerSetting = TriggerBuilder.Create().WithSimpleSchedule(x => x.WithIntervalInMinutes(SectionInfo.Setting.SettingUpdatePeriod).RepeatForever()).WithDescription("SettingUpdater").Build();
            schedulerSetting.ScheduleJob(jobSetting, triggerSetting);
        }
        protected void Application_Error(object sender, EventArgs e)
        {
            Exception lastError = Server.GetLastError();
            LogUtils.log(SectionInfo.LogAddress, "Backoffice exception : " + JsonConvert.SerializeObject(lastError));
            // Server.ClearError();

        }
    }
}
