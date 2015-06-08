using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Spring.Social.Twitter.Api;
using Spring.Social.Twitter.Api.Impl;

namespace Ao.Parklife.Services
{
    public class TwitterClient
    {
        private readonly TwitterTemplate _twitter;

        public TwitterClient()
        {
            string consumerKey = "85Er0GKY3GnqfLLAuB3e1Llu1"; // The application's consumer key
            string consumerSecret = "QeppIjR36DWJGqZpz5W5pW9EvNVZLvSCH8vMHC1y9ZyxQFSK5t"; // The application's consumer secret
            string accessToken = "3313418985-3DswZJghfMKBkXnKJ7qJt1AWjMKjrx65zaFMNKL"; // The access token granted after OAuth authorization
            string accessTokenSecret = "08JqQ0OSKVOPgM5WyW78mvkEvd15VaPbdEXbddyuV5sa1"; // The access token secret granted after OAuth authorization
            _twitter = new TwitterTemplate(consumerKey, consumerSecret, accessToken, accessTokenSecret);
        }

        public void Send(string message)
        {
            _twitter.TimelineOperations.UpdateStatusAsync(message);

        }
    }
}