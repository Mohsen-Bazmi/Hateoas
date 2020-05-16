namespace Hateoas
{
    public static class HateoasExtensions
    {
        public static T AddLink<T>(this T link, string href, string rel, string type) where T : HateoasLinks
        => link.AddLink(href, rel, type) as T;
    }
}