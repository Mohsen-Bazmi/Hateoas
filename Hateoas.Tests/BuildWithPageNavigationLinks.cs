using System;
using System.Collections.Generic;
using AutoFixture;

namespace Hateoas.Tests
{
    class BuildWithPageNavigationLinks<T>
    {
        static IFixture fixture = new Fixture();
        static BuildWithPageNavigationLinks()
        {
            fixture.Register(() => HateoasLinks.WithContent<T>(fixture.Create<T>()));
        }
        string currentPageHref;
        string nextPageHref;
        string previousPageHref;
        bool hasNext;
        bool hasPrevious;
        public static BuildWithPageNavigationLinks<T> AFakeOne()
        => new BuildWithPageNavigationLinks<T>
        {
            currentPageHref = fixture.Create<string>(),
            nextPageHref = fixture.Create<string>(),
            previousPageHref = fixture.Create<string>(),

            hasNext = true,
            hasPrevious = true
        };
        BuildWithPageNavigationLinks<T> With(Action<BuildWithPageNavigationLinks<T>> change)
        {
            change(this);
            return this;
        }
        public BuildWithPageNavigationLinks<T> WithPreviousPageHref(string previousPageHref)
        => With(b => b.previousPageHref = previousPageHref);
        public BuildWithPageNavigationLinks<T> WithCurrentPageHref(string currentPageLink)
        => With(b => b.currentPageHref = currentPageLink);
        public BuildWithPageNavigationLinks<T> WithNextPageHref(string nextPageHref)
        => With(b => b.nextPageHref = nextPageHref);
        public BuildWithPageNavigationLinks<T> WithoutPreviousPage()
        => With(b => b.hasPrevious = false);
        public BuildWithPageNavigationLinks<T> WithPreviousPage()
        => With(b => b.hasPrevious = true);
        public BuildWithPageNavigationLinks<T> WithoutNextPage()
        => With(b => b.hasNext = false);
        public BuildWithPageNavigationLinks<T> WithNextPage()
        => With(b => b.hasNext = true);

        public LinkResult<IEnumerable<LinkResult<T>>> Please()
        => HateoasLinks.AddPageNavigationLinks<LinkResult<T>>(
                 value: fixture.CreateMany<LinkResult<T>>()
                 , new PageHrefs(currentPageHref, nextPageHref, previousPageHref)
                 , hasNext, hasPrevious);

    }
}
