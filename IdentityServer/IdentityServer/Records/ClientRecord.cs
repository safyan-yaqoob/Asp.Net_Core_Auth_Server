namespace IdentityServer.Records
{
    public record ClientRecord
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string DisplayName { get; set; }
        public string RedirectUri { get; set; }
        public string PostLogoutRedirectUris { get; set; }
    }

    public record ClientsSummaryRecord
    {
        public string Id { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string DisplayName { get; set; }
    }

    public record EditClientRecord
    {
        public string Id { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string DisplayName { get; set; }
        public string RedirectUri { get; set; }
        public string PostLogoutRedirectUris { get; set; }
    }
}
