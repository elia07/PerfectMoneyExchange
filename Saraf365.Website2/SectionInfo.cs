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
using Saraf365.Core.InternetBankModules;
using System.Threading;

namespace Saraf365.Website2
{
    public static partial class SectionInfo
    {
        public static string LogAddress = AppDomain.CurrentDomain.BaseDirectory + "log.txt";
        public static string LogAdminName = "Website2 Log";
        public static Setting Setting=null;
        public static string UserSessionName = "User";

        static SectionInfo()
        {
           
        }

        public static void init()
        {
            Setting = new Setting();
            

            DomainInfo.ControllerMentionFields = new Dictionary<string, List<MentionField>>();
            List<MentionField> temp = new List<MentionField>();

            /*temp = new List<MentionField>();
            temp.Add(new MentionField("xType", "نوع", typeof(AdminType)));
            temp.Add(new MentionField("xEmail", "عنوان", typeof(string)));
            temp.Add(new MentionField("xName", "نام", typeof(string)));
            DomainInfo.ControllerMentionFields.Add("Admin", temp);

            temp = new List<MentionField>();
            DomainInfo.ControllerMentionFields.Add("User", temp);


            temp = new List<MentionField>();
            DomainInfo.ControllerMentionFields.Add("History", temp);


            temp = new List<MentionField>();
            temp.Add(new MentionField("xUserID", "", typeof(Int64), false));
            DomainInfo.ControllerMentionFields.Add("Ticket", temp);

            temp = new List<MentionField>();
            temp.Add(new MentionField("xUserID", "", typeof(Int64), false));
            DomainInfo.ControllerMentionFields.Add("Voucher", temp);

            temp = new List<MentionField>();
            //temp.Add(new MentionField("xUserID", "", typeof(Int64), false));
            temp.Add(new MentionField("xStartDate", "از تاریخ", typeof(DateTime), false));
            temp.Add(new MentionField("xEndDate", "تا تاریخ", typeof(DateTime), false));
            temp.Add(new MentionField("xCode", "کد", typeof(string), false));
            DomainInfo.ControllerMentionFields.Add("Income", temp);

            temp = new List<MentionField>();
            temp.Add(new MentionField("xUserID","",typeof(Int64),false));
            DomainInfo.ControllerMentionFields.Add("Withdrawal", temp);

            temp = new List<MentionField>();
            temp.Add(new MentionField("xUserID", "", typeof(Int64), false));
            DomainInfo.ControllerMentionFields.Add("BankAccount", temp);

            temp = new List<MentionField>();
            temp.Add(new MentionField("xUserID", "", typeof(Int64), false));
            DomainInfo.ControllerMentionFields.Add("ActivationHistory", temp);*/
            


        }

    }
}