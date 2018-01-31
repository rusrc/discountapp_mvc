namespace Discountapp.Domain.Models.Application
{
    public interface IValuable<T>
    {
        T Value { get; set; }
    }
}
