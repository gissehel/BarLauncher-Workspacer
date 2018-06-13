namespace Wox.Workspacer.DomainModel
{
    public class WoxQuery
    {
        public object InternalQuery { get; set; }

        public string RawQuery { get; set; }

        public string Search { get; set; }

        public string[] SearchTerms { get; set; }

        public string Command { get; set; }

        public string FirstTerm => SearchTerms.Length > 0 ? SearchTerms[0] : string.Empty;
    }
}