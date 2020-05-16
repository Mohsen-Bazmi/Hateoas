namespace Hateoas
{
    public class LinkDto
    {
        public LinkDto(string href, string rel, string type)
        {
            this.Type = type;
            this.Rel = rel;
            this.Href = href;
        }
        public string Href { get; }
        public string Rel { get; }
        public string Type { get; }
    }
}