using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using Newtonsoft.Json;
using BadAPI.Helpers;

namespace BadAPI.Controllers
{
    [Route("api/TweetController")]
    public class TweetController : Controller
    {

        [HttpGet("[action]")]
        async public Task<Tweet[]> Tweets()
        {
            return await parseTweet();
        }

        private async Task<Tweet[]> parseTweet()
        {
            //The date range is currently what's defined in the requirements
            TweetDateHandler TweetDateHandler = new TweetDateHandler(new DateTime(2016, 1, 1), new DateTime(2018, 1, 1));
            List<Tweet[]> tweets = new List<Tweet[]>();

            while (TweetDateHandler.ReachedLastTweet())
            {
                using (var client = new HttpClient())
                {
                    try
                    {
                        client.BaseAddress = new Uri("https://badapi.iqvia.io");
                        
                        var response = await client.GetAsync($"/api/v1/Tweets?startDate={TweetDateHandler.startDate}&endDate={TweetDateHandler.endDate}");
                        response.EnsureSuccessStatusCode();

                        var stringResult = await response.Content.ReadAsStringAsync();

                        var tweet = JsonConvert.DeserializeObject<Tweet[]>(stringResult); 
                        if (tweet.Count() == 0)
                            break;

                        tweets.Add(tweet);
                        
                        TweetDateHandler.SetNextStartDate(tweet.Last().stamp);
                    }
                    catch (HttpRequestException httpRequestException)
                    {
                        throw httpRequestException;
                    }
                }
            }
            System.Console.WriteLine(tweets.Count() + " Requests made");
            return CombineResponses(tweets);
        }

        private Tweet[] CombineResponses(List<Tweet[]> tweets)
        {
            List<Tweet> combinedResponse = new List<Tweet>();
            for (int i = 0; i < tweets.Count(); i++)
            {
                combinedResponse.AddRange(tweets[i]);
            }

            return combinedResponse.ToArray();
        } //Formatted to a single, correct JSON result
    }
}
