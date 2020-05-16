using System;
using AutoFixture;

namespace Hateoas.Tests
{
    class BuildPageHref
    {
        static Fixture fixture = new Fixture();

        public string Current { get; set; }
        public string Next { get; set; }
        public string Previous { get; set; }

        public BuildPageHref With(Action<BuildPageHref> change)
        {
            change(this);
            return this;
        }

        public static BuildPageHref AValidOne()
        => new BuildPageHref
        {
            Current = fixture.Create<string>(),
            Next = fixture.Create<string>(),
            Previous = fixture.Create<string>()
        };
        public PageHrefs Please()
        => new PageHrefs(Current, Next, Previous);
    }
}