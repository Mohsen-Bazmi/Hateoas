using System.Collections.Generic;
using System.Net.Http;

namespace Hateoas
{
    public class HateoasLinks
    {
        public static HateoasLinks WithNoContent
        => new HateoasLinks();
        public IList<LinkDto> Links { get; } = new List<LinkDto>();
        internal HateoasLinks AddLink(string href, string rel, string type)
        {
            Links.Add(new LinkDto(href, rel, type));
            return this;
        }

        protected HateoasLinks() { }

        public static LinkResult<T> WithContent<T>(T value)
        => new LinkResult<T>(value);

        public static LinkResult<IEnumerable<T>> AddPageNavigationLinks<T>(
            IEnumerable<T> value
            , PageHrefs pageHrefs
            , bool hasNext, bool hasPrevious
            ) where T : HateoasLinks
        {
            var result = WithContent(value);
            result.AddLink(pageHrefs.Current, "self", HttpMethod.Get.Method);
            if (hasNext)
                result.AddLink(pageHrefs.Next, "next-page", HttpMethod.Get.Method);
            if (hasPrevious)
                result.AddLink(pageHrefs.Previous, "previous-page", HttpMethod.Get.Method);
            return result;
        }


    }
}