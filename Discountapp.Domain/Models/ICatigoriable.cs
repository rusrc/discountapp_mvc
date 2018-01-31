using Discountapp.Domain.Models.Application;

namespace Discountapp.Domain.Models
{
    /// <summary>
    /// Entity who should has the CategoryId property
    /// </summary>
    public interface ICatigoriable
    {
        long CategoryId { get; set; }
        Category Category { get; set; }
    }
}
