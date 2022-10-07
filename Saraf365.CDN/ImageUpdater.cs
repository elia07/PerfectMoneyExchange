using Saraf365.Core;
using Saraf365.Core.Repositories;
using Quartz;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using RockCandy.Web.Framework.Utilities;
using Newtonsoft.Json;

namespace Saraf365.CDN
{
    public class ImageUpdater : IJob
    {
        private static int Worker = 0;
        public void Manage()
        {

            using (SystemFileRepository sfr = new SystemFileRepository())
            {
                var pathBase = System.Web.Hosting.HostingEnvironment.MapPath("~//Files//");
                foreach (var item in sfr.GetAll())
                {
                    string fileAddress = Path.Combine(pathBase, item.xFileName);
                    //LogUtils.log(SectionInfo.LogAddress, fileAddress);
                    bool isFileExist = false;
                    try
                    {
                        isFileExist = System.IO.File.Exists(fileAddress);
                    }
                    catch
                    {

                    }
                    //LogUtils.log(SectionInfo.LogAddress, isFileExist.ToString());
                    if (!isFileExist)
                    {
                        CreatFile(item.xID, item.xFileName);
                    }




                }
            }

        }
        private bool CreatFile(long fileID, string fileName)
        {
            try
            {
                using (SystemFileRepository sfr = new SystemFileRepository())
                {
                    var pathBase = System.Web.Hosting.HostingEnvironment.MapPath("~//Files//");
                    SystemFile instance = sfr.GetByID(fileID);
                    foreach(var item in instance.FileData)
                    {
                        string FileAddress = "";
                        if (item.xIsThumbnail)
                        {
                            FileAddress = Path.Combine(pathBase, Path.GetFileNameWithoutExtension(instance.xFileName)+"_Thumb"+Path.GetExtension(instance.xFileName));
                        }
                        else
                        {
                            FileAddress= Path.Combine(pathBase, instance.xFileName);
                        }
                        using (BinaryWriter bw = new BinaryWriter(System.IO.File.Open(FileAddress, FileMode.Create)))
                        {
                            bw.Write(item.xData);
                            bw.Flush();
                        }
                    }
                   

                }
                return true;
            }
            catch (Exception e){
                //LogUtils.log(SectionInfo.LogAddress, JsonConvert.SerializeObject(e));
                return false; }

        }

        void IJob.Execute(IJobExecutionContext context)
        {
            Worker++;
            if (Worker > 1)
            {
                Worker--;
                return;
            }
            try
            {
                Manage();
            }
            catch { }
            Worker--;
        }
    }
}