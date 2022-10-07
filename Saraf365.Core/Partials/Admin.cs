using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saraf365.Core
{
    public partial class Admin
    {
        public string GetSerializedData()
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("xEmail", this.xEmail);
            data.Add("xID", this.xID.ToString());
            data.Add("xIsActive", this.xIsActive.ToString());
            data.Add("xName", this.xName);
            data.Add("xTelegramID", this.xTelegramID.ToString());
            data.Add("xTelegramUsername", this.xTelegramUsername);
            data.Add("xType", this.xType.ToString());
            foreach(var item in AdminRole)
            {
                data.Add("xCrudPermission-"+ item.xSectionType, item.xCrudPermission);
            }
            

            return JsonConvert.SerializeObject(data);
        }
    }
}
