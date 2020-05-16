using System.Net.Http;
using AutoFixture.Xunit2;
using FluentAssertions;
using Xunit;

namespace Hateoas.Tests
{
    public class HateoasLinksTests
    {

        [Theory, AutoData]
        public void WithLinkedCollection_Allways_AddsLinkToSelf(string currentPageHref)
        {
            var sut = BuildWithPageNavigationLinks<int>.AFakeOne()
                                                       .WithCurrentPageHref(currentPageHref)
                                                       .Please();

            sut.Links.Should().Contain(l => l.Href == currentPageHref
                                     && l.Rel == "self"
                                     && l.Type == HttpMethod.Get.Method);
        }

        [Theory, AutoData]
        public void WithLinkedCollection_WhenHasNext_AddsLinkToTheNextPage(string nextPageHref)
        {
            var sut = BuildWithPageNavigationLinks<int>.AFakeOne()
                                                       .WithNextPage()
                                                       .WithNextPageHref(nextPageHref)
                                                       .Please();

            sut.Links.Should().Contain(l => l.Href == nextPageHref
                                         && l.Rel == "next-page"
                                         && l.Type == HttpMethod.Get.Method);
        }

        [Theory, AutoData]
        public void WithLinkedCollection_WhenDoesNotHaveNext_DoesNotAddLinkToTheNextPage(string dummy)
        {
            var sut = BuildWithPageNavigationLinks<int>.AFakeOne()
                                                       .WithoutNextPage()
                                                       .WithNextPageHref(dummy)
                                                       .Please();
            sut.Links.Should().NotContain(l => l.Rel == "next-page");
        }


        [Theory, AutoData]
        public void WithLinkedCollection_WhenHasPrevious_AddsLinkToPreviousPage(string previousPageHref)
        {
            var sut = BuildWithPageNavigationLinks<int>.AFakeOne()
                                                       .WithPreviousPage()
                                                       .WithPreviousPageHref(previousPageHref)
                                                       .Please();
            sut.Links.Should().Contain(l => l.Href == previousPageHref
                                         && l.Rel == "previous-page"
                                         && l.Type == HttpMethod.Get.Method);
        }

        [Theory, AutoData]
        public void WithLinkedCollection_WhenDoesnotHavePrevious_DoesNotAddLinkToPreviousPage(string previousPageHref)
        {
            var sut = BuildWithPageNavigationLinks<int>.AFakeOne()
                                                        .WithoutPreviousPage()
                                                        .WithPreviousPageHref(previousPageHref)
                                                        .Please();
            sut.Links.Should().NotContain(l => l.Rel == "previous-page");
        }
    }
}
