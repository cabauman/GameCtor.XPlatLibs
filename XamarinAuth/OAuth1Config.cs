namespace GameCtor.XamarinAuth
{
    public struct OAuth1Config
    {
        public string ConsumerKey { get; set; }

        public string ConsumerSecret { get; set; }

        public string RequestTokenUrl { get; set; }

        public string AuthorizeUrl { get; set; }

        public string AccessTokenUrl { get; set; }

        public string CallbackUrl { get; set; }
    }
}
