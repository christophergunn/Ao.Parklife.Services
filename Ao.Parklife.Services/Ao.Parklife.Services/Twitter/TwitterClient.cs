
namespace Ao.Parklife.Services.Twitter
{
    public class TwitterClient
    {
        private readonly TinyTwitter _twitter;

        public TwitterClient()
        {
            string consumerKey = "tXGGAIbJyv8JSc14dvdpWN9h5"; // The application's consumer key
            string consumerSecret = "xojHAwVhlZnjg4k4bXmt5K8SyI2k12St1O3ZPgPIC53jDaMWBg";
                // The application's consumer secret
            string accessToken = "3313418985-Ti5LJ7rksnHaJs4V2SzdoQxQaHpDSGehI1crHqL";
                // The access token granted after OAuth authorization
            string accessTokenSecret = "SRKfxk5fjvSS8n8ZFUqSdLNmlAGFgWMVYSuSWlF7INQ0e";
                // The access token secret granted after OAuth authorization

            var oauth = new OAuthInfo
            {
                AccessToken = accessToken,
                AccessSecret = accessTokenSecret,
                ConsumerKey = consumerKey,
                ConsumerSecret = consumerSecret
            };

            _twitter = new TinyTwitter(oauth);
        }

        public void Send(string message)
        {
            _twitter.UpdateStatus(message);
        }
    }
}