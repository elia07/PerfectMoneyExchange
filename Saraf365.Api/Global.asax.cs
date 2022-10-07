using Newtonsoft.Json;
using Quartz;
using Quartz.Impl;
using RockCandy.Web.Framework.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using Telegram.Bot;

namespace Saraf365.Api
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            SectionInfo.init();

            TelegramBotClient tbc = new TelegramBotClient(SectionInfo.Setting.TelegramBotAccessToken);
            tbc.SetWebhookAsync(SectionInfo.Setting.TelegramCallBackAddress).Wait();
            //tbc.StartReceiving();

            IScheduler schedulerSetting = StdSchedulerFactory.GetDefaultScheduler();
            schedulerSetting.Start();

            IJobDetail jobSetting = JobBuilder.Create<SettingUpdater>().Build();

            ITrigger triggerSetting = TriggerBuilder.Create().WithSimpleSchedule(x => x.WithIntervalInMinutes(SectionInfo.Setting.SettingUpdatePeriod).RepeatForever()).WithDescription("SettingUpdater").Build();
            schedulerSetting.ScheduleJob(jobSetting, triggerSetting);
        }
        protected void Application_Error(object sender, EventArgs e)
        {
            //Exception lastError = Server.GetLastError();
            LogUtils.log(SectionInfo.LogAddress, "api exception : " + JsonConvert.SerializeObject(e));

            // Server.ClearError();

        }
    }
}
