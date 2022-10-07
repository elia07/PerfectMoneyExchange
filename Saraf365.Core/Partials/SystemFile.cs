using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saraf365.Core
{
    public partial class SystemFile
    {
        public string GetAddress(string cdnAddress,bool isHttps, bool thumb=false)
        {
            if(isHttps)
            {
                cdnAddress = cdnAddress.Replace("http","https");
            }
            if (thumb && FileData.Where(x=>x.xIsThumbnail).Any())
            {
                return string.Format("{0}Files/{1}", cdnAddress, Path.GetFileNameWithoutExtension(xFileName)+"_Thumb"+Path.GetExtension(xFileName));
            }
            else
            {
                return string.Format("{0}Files/{1}", cdnAddress, xFileName);
            }
        }
    }
}
