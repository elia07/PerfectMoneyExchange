using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RockCandy.Web.Framework.Utilities;
using System.Net;  
using System.Net.Http;  
using System.Web.Http.Filters;
using Newtonsoft.Json;

namespace Saraf365.Api
{
    public class CustomExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            string exceptionMessage = string.Empty;
            if (actionExecutedContext.Exception.InnerException == null)
            {
                exceptionMessage = actionExecutedContext.Exception.Message;
            }
            else
            {
                exceptionMessage = actionExecutedContext.Exception.InnerException.Message;
            }
            try
            {
                LogUtils.log(SectionInfo.LogAddress, "input : " + JsonConvert.SerializeObject(actionExecutedContext.ActionContext.ActionArguments["args"]));
            }
            catch { }
            
            LogUtils.log(SectionInfo.LogAddress, "api exception : " + JsonConvert.SerializeObject(actionExecutedContext.Exception));
            var response = new HttpResponseMessage(HttpStatusCode.InternalServerError){
                Content = new StringContent("An unhandled exception was thrown by service."), ReasonPhrase = "Internal Server Error.Please Contact your Administrator."
            };
            actionExecutedContext.Response = response;
        }
    }
}