using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace StockBot
{
    public class StockFinder
    {
        public static async Task<double?> GetStockPriceAsync(string stock)
        {
            try
            {
                string uri = $"{Settings.GetStockUri()}{stock}{Settings.GetStockUriTag()}";
                string uriResult = "";

                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage msg = await client.GetAsync(uri);

                    if (msg.IsSuccessStatusCode)
                    {
                        uriResult = await msg.Content.ReadAsStringAsync();
                    }
                }

                string price = GetPrice(uriResult);
                if (!string.IsNullOrEmpty(price))
                {
                    double stockPrice;
                    if (double.TryParse(price, out stockPrice))
                    {
                        return stockPrice;
                    }
                }
                return null;
            }
            catch (WebException ex)
            { 
                throw ex;
            }
        }

        private static string GetPrice(string uriResult)
        {
            if (uriResult.Length == 0)
                return string.Empty;

            var lineOne = uriResult.Split('\n')[0];
            return lineOne.Split(',')[1];
        }
    }
}