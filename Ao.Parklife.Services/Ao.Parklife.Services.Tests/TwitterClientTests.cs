using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TinyTwitter;

namespace Ao.Parklife.Services.Tests
{
    [TestFixture]
    public class TwitterClientTests
    {
        [Test]
        public void CanMakePosts()
        {
            var t = new TwitterClient();
            t.Send("Test");
        }

        [Test]
        public void ProfileInfo()
        {

            string consumerKey = "tXGGAIbJyv8JSc14dvdpWN9h5"; // The application's consumer key
            string consumerSecret = "xojHAwVhlZnjg4k4bXmt5K8SyI2k12St1O3ZPgPIC53jDaMWBg"; // The application's consumer secret
            string accessToken = "3313418985-Ti5LJ7rksnHaJs4V2SzdoQxQaHpDSGehI1crHqL"; // The access token granted after OAuth authorization
            string accessTokenSecret = "SRKfxk5fjvSS8n8ZFUqSdLNmlAGFgWMVYSuSWlF7INQ0e"; // The access token secret granted after OAuth authorization

            var oauth = new OAuthInfo
            {
                AccessToken = accessToken,
                AccessSecret = accessTokenSecret,
                ConsumerKey = consumerKey,
                ConsumerSecret = consumerSecret
            };

            var twitter = new TinyTwitter.TinyTwitter(oauth);

            // Update status, i.e, post a new tweet
            twitter.UpdateStatus("I'm tweeting from C#");

            // Get home timeline tweets
            var tweets = twitter.GetHomeTimeline();

            foreach (var tweet in tweets)
                Console.WriteLine("{0}: {1}", tweet.UserName, tweet.Text);
        }
    }
}
