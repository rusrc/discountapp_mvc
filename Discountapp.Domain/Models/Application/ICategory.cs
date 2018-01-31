namespace Discountapp.Domain.Models.Application
{
    public interface ICategory : IIdentifiable
    {
        long? ParentId { get; set; }
    }
}