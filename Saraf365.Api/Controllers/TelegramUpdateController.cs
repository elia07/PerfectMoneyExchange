using Newtonsoft.Json;
using Saraf365.Core;
using Saraf365.Core.Enumerations;
using Saraf365.Core.Repositories;
using RockCandy.Web.Framework.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Saraf365.Api.Controllers
{
    static class Bot
    {
        public static readonly TelegramBotClient bot = new TelegramBotClient(SectionInfo.Setting.TelegramBotAccessToken);
    }
    [CustomExceptionFilter]
    public class TelegramUpdateController : ApiController
    {
        public async Task<IHttpActionResult> Post(Update update)
        {
            //LogUtils.log(SectionInfo.LogAddress, JsonConvert.SerializeObject(update));
            try
            {
                var message = update.Message;
                if (message.Type == MessageType.Text)
                {
                    using (TelegramSupportRepository tsr = new TelegramSupportRepository())
                    {
                        TelegramSupport instance = new TelegramSupport();
                        instance.xChatID = message.Chat.Id;
                        instance.xUsername = message.Chat.Username;
                        instance.xDate = DateTime.Now;
                        instance.xMessage = message.Text;
                        instance.xMessageID = message.MessageId;

                        if (instance.xMessage == null)
                        {
                            instance.xMessage = "";
                        }

                        tsr.Insert(instance);
                    }
                }
                else if (message.Type == MessageType.Photo)
                {
                    MemoryStream ms = new MemoryStream();
                    var file = await Bot.bot.GetInfoAndDownloadFileAsync(message.Photo.LastOrDefault()?.FileId, ms);

                    using (SystemFileRepository sfr = new SystemFileRepository())
                    {
                        long sfInstance = sfr.InsertImageFormTelegramBot(ms, new Random().Next(111111, 999999).ToString() + ".jpg");

                        using (TelegramSupportRepository tsr = new TelegramSupportRepository())
                        {
                            TelegramSupport instance = new TelegramSupport();
                            instance.xChatID = message.Chat.Id;
                            instance.xUsername = message.Chat.Username;
                            instance.xDate = DateTime.Now;
                            instance.xMessage = message.Caption;
                            instance.xMessageID = message.MessageId;
                            instance.xSystemFileID = sfInstance;
                            if (instance.xMessage == null)
                            {
                                instance.xMessage = "";
                            }
                            tsr.Insert(instance);
                        }
                    }
                }
                new AdminNotificationRepository().Insert(AdminNotificationType.TelegramSupport, message.Chat.Username, SectionInfo.Setting.AdminNotificationSetting);
            }
            catch (Exception e)
            {

            }
            
            return Ok();
        }
    }
}
