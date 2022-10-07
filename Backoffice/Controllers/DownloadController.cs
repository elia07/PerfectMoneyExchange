using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using RockCandy.Web.Framework.Core;
using RockCandy.Web.Framework.Core.Enumerations;
using RockCandy.Web.Framework.Core.ActionFilters;
using Saraf365.Backoffice.Filters;
using Saraf365.Core.Enumerations;
using Saraf365.Core;
using RockCandy.Web.Framework.Core.Models;
using Saraf365.Core.Repositories;
using RockCandy.Web.Framework.Utilities.Encryption;
using RockCandy.Web.Framework.Utilities;
using Saraf365.Backoffice;
namespace Saraf365.Backoffice.Controllers
{
    public class DownloadController : Controller
    {
        [AdminAuthorize]
        public FileContentResult GetFile(long xFileId, string title = "")
        {
            try
            {
                SystemFile sf = new Core.Repositories.SystemFileRepository().GetByID(xFileId);
                return File(sf.FileData.Where(x=>x.xIsThumbnail==false).Single().xData.ToArray(), sf.xContentType, sf.xFileName);
            }
            catch { return null; }
        }
    }
}