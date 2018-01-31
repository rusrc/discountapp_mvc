using System;
using System.ComponentModel.DataAnnotations;

namespace Discountapp.Domain.Models
{
    public enum ActiveStatus
    {
        [Display(Name = "Неактивный")] Inactive = 0,
        [Display(Name = "Активный")] Active = 1
    }

    public enum PromotionFilterType
    {
        [Display(Name = "Только новые")]
        NewOnly = 1,
        [Display(Name = "Высокий рейтинг")]
        HighRate = 2,
        [Display(Name = "Низкий рейтинг")]
        LowRate = 4
    }

    public enum LikeType
    {
        Like = 1,
        Dislike = -1
    }

    [Flags]
    public enum RoleType
    {
        Admin = 2,
        Manager = 4
    }
}
