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

namespace Saraf365.Website2
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_BeginRequest(object sender, EventArgs e)
        {

            if (!Request.Url.Host.StartsWith("www") && !Request.Url.IsLoopback)
            {
                UriBuilder builder = new UriBuilder(Request.Url);
                builder.Host = "www." + Request.Url.Host;
                Response.StatusCode = 301;
                Response.AddHeader("Location", builder.ToString());
                Response.End();
            }

            /*if (SectionInfo.Setting.StrictHttps && !Request.Url.IsLoopback && !Request.Url.ToString().Contains("https"))
            {
                UriBuilder builder = new UriBuilder(Request.Url.ToString().Replace("http", "https"));
                Response.StatusCode = 301;
                Response.AddHeader("Location", builder.ToString());
                Response.End();
            }*/

            if (Request.Url.ToString().Contains("saraf365.com") || Request.Url.ToString().Contains("saraf365.net"))
            {
                UriBuilder builder = new UriBuilder(SectionInfo.Setting.DomainAddress);
                Response.StatusCode = 301;
                Response.AddHeader("Location", builder.ToString());
                Response.End();
            }
        }
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

            LogUtils.log(SectionInfo.LogAddress, "Website exception : " + JsonConvert.SerializeObject(lastError));
            //SectionInfo.Logger.Trace(lastError ,"UnhandledException");
            // Server.ClearError();

        }
    }
}
