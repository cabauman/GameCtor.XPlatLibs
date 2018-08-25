namespace GameCtor.XamarinAuth
{
    public struct OAuth2Config
    {
        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public string Scope { get; set; }

        public string AuthorizeUrl { get; set; }

        public string RedirectUrl { get; set; }

        public string AccessTokenUrl { get; set; }
    }
}
