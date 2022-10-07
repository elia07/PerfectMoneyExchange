using Saraf365.Core.Enumerations;
using Saraf365.Core.Repositories;
using RockCandy.Web.Framework.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Saraf365.Backoffice.DomainUtils
{
    public static class MultiLevelCustomDropDown
    {
        public static string MultiAdminRoleSectionTypeDropDown(this HtmlHelper helper, string id)
        {
                StringBuilder sb = new StringBuilder();
                sb.Append(string.Format("<select class='selectpicker form-control' multiple title='سطوح دسته بندی بخشی' id='{0}' name='{0}'>", id));
                foreach (var item in Enum.GetValues(typeof(AdminRoleSectionType)))
                {
                    sb.Append(string.Format("<option value='{0}'>{1}</option>", ((byte)(AdminRoleSectionType)item).ToString(), EnumUtils.GetAttribute<DisplayAttribute>((AdminRoleSectionType)item).Name));
                }
                sb.Append("</select>");


                return sb.ToString();
        }


    }
}