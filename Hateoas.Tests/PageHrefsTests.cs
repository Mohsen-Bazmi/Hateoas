using System;
using AutoFixture.Xunit2;
using FluentAssertions;
using Xunit;

namespace Hateoas.Tests
{
    public class PageHrefsTests
    {
        [Theory, AutoData]
        public void Current_ByDefault_ReturnsCurrent(string current)
        {
            var sut = BuildPageHref.AValidOne().With(s => s.Current = current).Please();
            sut.Current.Should().Be(current);
        }
        [Fact]
        public void Constractor_WhenCurrentPageHrefIsNull_ThrowsArgumentException()
        {
            Action act = () => BuildPageHref.AValidOne().With(h => h.Current = null).Please();
            act.Should().Throw<ArgumentException>();
        }

        [Theory, AutoData]
        public void Next_ByDefault_ReturnsNext(string next)
        {
            var sut = BuildPageHref.AValidOne().With(s => s.Next = next).Please();
            sut.Next.Should().Be(next);
        }
        [Fact]
        public void Constractor_WhenNextPageHrefIsNull_ThrowsArgumentException()
        {
            Action act = () => BuildPageHref.AValidOne().With(h => h.Next = null).Please();
            act.Should().Throw<ArgumentException>();
        }


        [Theory, AutoData]
        public void Previous_ByDefault_ReturnsPrevious(string previous)
        {
            var sut = BuildPageHref.AValidOne().With(s => s.Previous = previous).Please();
            sut.Previous.Should().Be(previous);
        }
        [Fact]
        public void Constractor_WhenPreviousPageHrefIsNull_ThrowsArgumentException()
        {
            Action act = () => BuildPageHref.AValidOne().With(h => h.Previous = null).Please();
            act.Should().Throw<ArgumentException>();
        }
    }
}