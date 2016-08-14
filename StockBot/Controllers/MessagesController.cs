using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using StockBot.Luis;

namespace StockBot.Controllers
{
    // [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity.Type == ActivityTypes.Message)
            {
                try
                {
                    ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));

                    //talk to Luis
                    string result;

                    LuisResponse luisResponse = await GetLuisEntity(activity.Text);

                    if (luisResponse.intents.Any() && luisResponse.entities.Any())
                    {
                        switch (luisResponse.intents[0].intent)
                        {
                            case "GetStockPrice":
                                result = await GetStock(luisResponse.entities[0].entity);
                                break;
                            default:
                                result = "Sorry, I am not getting you...";
                                break;
                        }
                    }
                    else
                    {
                        result = "Sorry, I didn't understand...";
                    }

                    // return our reply to the user
                    Activity reply = activity.CreateReply(result);
                    await connector.Conversations.ReplyToActivityAsync(reply);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }



        private async Task<string> GetStock(string stock)
        {
            double? stockPrice = await StockFinder.GetStockPriceAsync(stock);
            if (stockPrice == null)
            {
                return $"{stock} is not valid as a stock item. Please try again....";
            }
            return $"Stock **{stock.ToUpper()}** has a price of {stockPrice}";
        }

        private async Task<LuisResponse> GetLuisEntity(string query)
        {
            query = Uri.EscapeDataString(query);
            LuisResponse luisData = new LuisResponse();

           using (HttpClient client = new HttpClient())
            {
                string uri = $"{Settings.GetLuisUri()}{query}";
                HttpResponseMessage response = await client.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    luisData = JsonConvert.DeserializeObject<LuisResponse>(json);
                }
            }

            return luisData;
        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }
}