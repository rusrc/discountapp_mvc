using Discountapp.Domain.Models.Identity;

namespace Discountapp.Domain.Models.Application
{
    public class MobileUser : BaseEntity
    {
        public long? UserId { get; set; }
        public string DeviceImei { get; set; }
        public string PhoneNumber { get; set; }

        public virtual AppUser User { get; set; }
    }
}
