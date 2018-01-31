using Discountapp.Domain.Models.Application;

namespace Discountapp.Domain.Models
{
    public interface INameMultiLanguageJson : INameable, IValuable<string>
    {
        string Name { get; set; }
        string Value { get; set; }
    }
}