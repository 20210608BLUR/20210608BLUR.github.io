using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Web;
using HolyShong.BackStage.Services;
using HolyShong.BackStage.Services.Interfaces;
using Newtonsoft.Json;

namespace isRock.Template
{
    public class LineLUISWebHookController : isRock.LineBot.LineWebHookControllerBase
    {
        private readonly ILinebotService _linebotService;

        public LineLUISWebHookController(ILinebotService linebot)
        {
            _linebotService = linebot;
        }
        //Line DEV也改
        //const string key = "889c58ab2efb4b0a827db33dea2bbf3e";//要改
        //const string endpoint = "hsbot20211027.cognitiveservices.azure.com/";//要改
        //const string appId = "c7368fb6-f60a-4142-bc2b-a1ff34d5394e";//要改

        const string key = "45a2af4421714ef1958473207194696e";//要改
        const string endpoint = "testluisalbert20211020.cognitiveservices.azure.com/";//要改
        const string appId = "8a0e8c40-add7-4b31-865f-7eb524f1a2ba";//要改

        [Route("api/LineLUIS")]
        [HttpPost]
        public IActionResult POST()
        {
            var AdminUserId = "Uac968f898a81205316372c3c771ecbac";

            try
            {
                //設定ChannelAccessToken
                this.ChannelAccessToken = "KQz1+k02D8/Q084PplEnqH+MgtyBb6K6VT9v+2Kt9vXnTe8SKyERSSswhcr7w2UOB3yEFOnWR4SS5CNj42AkuZGTYG4Ey4ZirVzb+eYaN8tSE6cC/ei8YiEsp7JH0PW+LNv/BdyarR+u3hisqGdsfQdB04t89/1O/w1cDnyilFU=";

                //bot instance
                var bot = new isRock.LineBot.Bot(this.ChannelAccessToken);
                //配合Line Verify
                if (ReceivedMessage.events == null || ReceivedMessage.events.Count() <= 0 ||
                    ReceivedMessage.events.FirstOrDefault().replyToken == "00000000000000000000000000000000") return Ok();
                //取得Line Event
                var LineEvent = this.ReceivedMessage.events.FirstOrDefault();
                var responseMsg = "";
                var UserId = this.ReceivedMessage.events[0].source.userId;

                //準備回覆訊息
                if (LineEvent.type.ToLower() == "message" && LineEvent.message.type == "text")
                {
                    var ret = MakeRequest(LineEvent.message.text);
                    responseMsg = $"好啦!請參考! ";

                    foreach (var item in ret.intents)
                    {
                        if (item.intent == "找餐廳")
                        {
                            //抓取entity和type
                            foreach (var entityItem in ret.entities)
                            {
                                //傳LINE搜尋出來的關鍵字(EX 牛肉麵...)
                                var result = _linebotService.SearchRestaurant((entityItem.entity).ToString());

                                //建立Actions
                                var Actions = new List<isRock.LineBot.TemplateActionBase>();
                                Actions.Add(new isRock.LineBot.UriAction() //UriAction
                                {
                                    label = "前往餐廳",
                                    uri = new Uri($"{result.StoreUrl}")
                                });

                                //建立發送訊息
                                isRock.LineBot.ButtonsTemplate btnMsg =
                                new isRock.LineBot.ButtonsTemplate()
                                {
                                    thumbnailImageUrl = new Uri($"{result.StoreImg}"),
                                    text = $"{result.StoreName}",
                                    title = $"{result.StoreName}",
                                    actions = Actions,
                                };
                                bot.PushMessage(UserId, btnMsg);
                            }
                        }
                    }
                }
                else if (LineEvent.type.ToLower() == "message")
                {
                    responseMsg = $"收到 event : {LineEvent.type} type: {LineEvent.message.type} ";
                }
                else { responseMsg = $"收到 event : {LineEvent.type} "; }
                    
                //回覆訊息
                this.ReplyMessage(LineEvent.replyToken, responseMsg);
                //response OK
                return Ok();
            }
            catch (Exception ex)
            {
                //回覆訊息
                this.PushMessage(AdminUserId, "發生錯誤:\n" + ex.Message);
                //response OK
                return Ok();
            }
        }

        static LUISResult MakeRequest(string utterance)
        {
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            var endpointUri = String.Format(
                "https://{0}/luis/v2.0/apps/{1}?verbose=true&timezoneOffset=0&subscription-key={3}&q={2}",
                endpoint, appId, utterance, key);

            var response = client.GetAsync(endpointUri).Result;

            var strResponseContent = response.Content.ReadAsStringAsync().Result;
            var Result = Newtonsoft.Json.JsonConvert.DeserializeObject<LUISResult>(strResponseContent);
            // Display the JSON result from LUIS
            return Result;
        }
    }

    #region "LUIS Model"

    public class TopScoringIntent
    {
        public string intent { get; set; }
        public double score { get; set; }
    }

    public class Intent
    {
        public string intent { get; set; }
        public double score { get; set; }
    }

    public class Resolution
    {
        public string value { get; set; }
    }

    public class Entity
    {
        public string entity { get; set; }
        public string type { get; set; }
        public int startIndex { get; set; }
        public int endIndex { get; set; }
        public double score { get; set; }
        public Resolution resolution { get; set; }
    }

    public class LUISResult
    {
        public string query { get; set; }
        public TopScoringIntent topScoringIntent { get; set; }
        public List<Intent> intents { get; set; }
        public List<Entity> entities { get; set; }
    }

    public class Source
    {
        public string userId { get; set; }
    }
    #endregion
}