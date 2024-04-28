namespace IdentityServer.Records
{
    public record ScopeRecord
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Resources { get; set; }
    }

    public record ScopeSummaryRecord
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
    }
    public record EditScopeRecord
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Resources { get; set; }
    }
}
