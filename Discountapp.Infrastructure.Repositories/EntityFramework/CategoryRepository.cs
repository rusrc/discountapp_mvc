using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discountapp.Domain;
using Discountapp.Domain.Models.Application;

namespace Discountapp.Infrastructure.Repositories.EntityFramework
{
    using Langx = Discountapp.Infrastructure.Resources.Localization.Lang;
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(DiscountappDbContext context) : base(context) { }

        public DiscountappDbContext DiscountAppDbContext => this._context as DiscountappDbContext;
    }
}
