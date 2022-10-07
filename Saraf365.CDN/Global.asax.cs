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

namespace Saraf365.CDN
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            SectionInfo.init();

            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
     

            IJobDetail jobImg = JobBuilder.Create<ImageUpdater>().Build();
            ITrigger triggerImg = TriggerBuilder.Create().WithSimpleSchedule(x => x.WithIntervalInMinutes(SectionInfo.Setting.CdnImageUpdatePeriodInMins).RepeatForever()).Build();
            scheduler.ScheduleJob(jobImg, triggerImg);

            new ImageUpdater().Manage();


            IJobDetail jobSetting = JobBuilder.Create<SettingUpdater>().Build();
            ITrigger triggerSetting = TriggerBuilder.Create().WithSimpleSchedule(x => x.WithIntervalInMinutes(SectionInfo.Setting.SettingUpdatePeriod).RepeatForever()).WithDescription("SettingUpdater").Build();
            scheduler.ScheduleJob(jobSetting, triggerSetting);


            scheduler.Start();

        }
        protected void Application_Error(object sender, EventArgs e)
        {

            Exception lastError = Server.GetLastError();
            string errorDetail = JsonConvert.SerializeObject(lastError);
            LogUtils.log(AppDomain.CurrentDomain.BaseDirectory, "website exception : " + errorDetail);
        }
    }
}
