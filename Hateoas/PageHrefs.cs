using System;

namespace Hateoas
{
    public class PageHrefs
    {
        public string Current { get; }
        public string Next { get; }
        public string Previous { get; set; }
        public PageHrefs(string current, string next, string previous)
        {
            if (previous == null) throw new ArgumentNullException(nameof(previous));
            if (current == null) throw new ArgumentNullException(nameof(current));
            if (next == null) throw new ArgumentNullException(nameof(next));
            this.Current = current;
            this.Previous = previous;
            this.Next = next;

        }
    }
}