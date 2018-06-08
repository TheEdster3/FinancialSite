using System;
using Xunit;
using BadAPI.Helpers;

namespace BadAPI.Tests
{
    public class TweetDateHandlerTest
    {
        private readonly TweetDateHandler _tweetdatehandler;

        public TweetDateHandlerTest()
        {
            _tweetdatehandler = new TweetDateHandler(new DateTime(2000,1,1), new DateTime(2018,1,1));
        }

        [Fact]
        public void ReturnsBlankWhenGivenABadDate()
        {
            _tweetdatehandler.SetNextStartDate("");
            var result = _tweetdatehandler.startDate;
            Assert.Equal("", result);
        }
        [Fact]
        public void ReturnsFalseWhenStartDateContainsNoData()
        {
            var tweetRemaining = _tweetdatehandler.ReachedLastTweet();
            Assert.True(tweetRemaining == true);
        }
    }
}