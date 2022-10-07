using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.InputFiles;

namespace Saraf365.Core.Utils
{
    public class TelegramUtils
    {
        public async Task<bool> SendPhoto(byte[] data,string filename,string caption,string accessToken,string channelName)
        {
            if(accessToken=="" || channelName=="")
            {
                return false;
            }
            var botClient = new TelegramBotClient(accessToken);

            InputOnlineFile iof = new InputOnlineFile(new MemoryStream(data), filename);
            var res = await botClient.SendPhotoAsync("@"+channelName, iof, caption);
            return res.Photo.Length != 0;
        }

        public async Task<bool> SendMessageResponse(string accessToken,long chatID,string text,int messageID)
        {
            if (accessToken == "")
            {
                return false;
            }
            var botClient = new TelegramBotClient(accessToken);
            var res = await botClient.SendTextMessageAsync(chatID,text,Telegram.Bot.Types.Enums.ParseMode.Default,false,false, messageID);
            return res.MessageId >0;
        }

        public async Task<bool> SendMessage(string accessToken, long chatID, string text)
        {
            if (accessToken == "")
            {
                return false;
            }
            var botClient = new TelegramBotClient(accessToken);
            var res = await botClient.SendTextMessageAsync(chatID, text);
            return res.MessageId > 0;
        }

        public async Task<bool> SendMessage(string accessToken, string chatID, string text,string appendText)
        {
            if (accessToken == "")
            {
                return false;
            }
            text = text + appendText;
            var botClient = new TelegramBotClient(accessToken);
            var res = await botClient.SendTextMessageAsync("@"+chatID, text);
            return res.MessageId > 0;
        }
    }
}
