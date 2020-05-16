namespace Hateoas
{
    public class LinkResult<T> : HateoasLinks
    {
        public T Value { get; protected set; }
        public LinkResult(T value)
        => Value = value;
    }
}