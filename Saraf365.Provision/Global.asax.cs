using Newtonsoft.Json;
using Quartz;
using RockCandy.Web.Framework.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Saraf365.Provision
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            SectionInfo.init();

            string expreission = "0 0 12 * * ?";



            IJobDetail jobClearSystemLog = JobBuilder.Create<ClearSystemLogs>().Build();
            ITrigger triggerClearSystemLog = TriggerBuilder.Create().WithCronSchedule(expreission).WithDescription("ClearSystemLogs").Build();
            SectionInfo.SchedulerSetting.ScheduleJob(jobClearSystemLog, triggerClearSystemLog);






            IJobDetail jobClearTelegramSupport = JobBuilder.Create<ClearTelegramSupport>().Build();
            ITrigger triggerClearTelegramSupport = TriggerBuilder.Create().WithCronSchedule(expreission).WithDescription("ClearTelegramSupport").Build();
            SectionInfo.SchedulerSetting.ScheduleJob(jobClearTelegramSupport, triggerClearTelegramSupport);


            IJobDetail jobResetResetGatewayTodayTotalTransactions = JobBuilder.Create<ResetGatewayTodayTotalTransactions>().Build();
            ITrigger triggerResetResetGatewayTodayTotalTransactions = TriggerBuilder.Create().WithCronSchedule(expreission).WithDescription("ResetGatewayTodayTotalTransactions").Build();
            SectionInfo.SchedulerSetting.ScheduleJob(jobResetResetGatewayTodayTotalTransactions, triggerResetResetGatewayTodayTotalTransactions);


            /*IJobDetail jobPerfectMoney = JobBuilder.Create<PerfectMoney>().Build();
            ITrigger triggerPerfectMoney = TriggerBuilder.Create().WithCronSchedule(expreission).WithDescription("PerfectMoney").Build();
            SectionInfo.SchedulerSetting.ScheduleJob(jobPerfectMoney, triggerPerfectMoney);*/



            IJobDetail jobSetting = JobBuilder.Create<SettingUpdater>().Build();

            ITrigger triggerSetting = TriggerBuilder.Create().WithSimpleSchedule(x => x.WithIntervalInMinutes(SectionInfo.Setting.SettingUpdatePeriod).RepeatForever()).WithDescription("SettingUpdater").Build();
            SectionInfo.SchedulerSetting.ScheduleJob(jobSetting, triggerSetting);



            IJobDetail jobInputCardInternetBank = JobBuilder.Create<InputCardInternetBank>().Build();

            ITrigger triggerInputCardInternetBank = TriggerBuilder.Create().WithSimpleSchedule(x => x.WithIntervalInMinutes(SectionInfo.Setting.InputCardInternetBankTaskPeriod).RepeatForever()).WithDescription("InputCardInternetBank").Build();
            SectionInfo.SchedulerSetting.ScheduleJob(jobInputCardInternetBank, triggerInputCardInternetBank);


            IJobDetail jobCardToCardManager = JobBuilder.Create<CardToCardManager>().Build();

            ITrigger triggerCardToCardManager = TriggerBuilder.Create().WithSimpleSchedule(x => x.WithIntervalInMinutes(5).RepeatForever()).WithDescription("CardToCardManager").Build();
            SectionInfo.SchedulerSetting.ScheduleJob(jobCardToCardManager, triggerCardToCardManager);

        }


        protected void Application_Error(object sender, EventArgs e)
        {

            Exception lastError = Server.GetLastError();
            string errorDetail = JsonConvert.SerializeObject(lastError);
            LogUtils.log(AppDomain.CurrentDomain.BaseDirectory, "website exception : " + errorDetail);
        }
    }
}
