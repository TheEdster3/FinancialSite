using System;

namespace BadAPI.Helpers
{
    public class TweetDateHandler
    {
        public string startDate { get; set; }
        public string endDate { get; set; }
        const string dateFormat = "yyyy-MM-dd'T'HH'%3A'mm'%3A'ss";
        public TweetDateHandler(DateTime startDate, DateTime endDate)
        {
            this.startDate = startDate.ToUniversalTime().ToString(dateFormat);
            this.endDate = endDate.AddTicks(-1).ToUniversalTime().ToString(dateFormat);
        } //Chooses the earliest and latest moments respectively
        public void SetNextStartDate(string latestTweetTime)
        {
            DateTime tweetStamp;
            if (DateTime.TryParse(latestTweetTime, out tweetStamp))
            {
                startDate = tweetStamp.AddSeconds(1).ToUniversalTime().ToString(dateFormat);
            } //To avoid adding duplicates we progress one second past the last tweet
            else
            {
                startDate = "";
            }
        } //We are limited to return only 100 entries, this ensures efficient progress through them.

        public bool ReachedLastTweet()
        {
            if(startDate == "")
                return false;
            return true;
        } //For code clarity
    }
}