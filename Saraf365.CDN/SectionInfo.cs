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

namespace Saraf365.CDN
{
    public static partial class SectionInfo
    {
        public static string LogAddress = AppDomain.CurrentDomain.BaseDirectory + "log.txt";
        public static string LogAdminName = "CDN Log";
        public static Setting Setting=null;

        static SectionInfo()
        {
           
        }
        public static void init()
        {
            Setting = new Setting();
        }

    }
}