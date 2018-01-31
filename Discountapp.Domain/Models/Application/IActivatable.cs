namespace Discountapp.Domain.Models.Application
{
    public interface IActivatable
    {
        ActiveStatus ActiveStatus { get; set; }
    }
}
