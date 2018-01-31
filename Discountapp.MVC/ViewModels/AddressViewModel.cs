using System.ComponentModel.DataAnnotations;
using Discountapp.Domain.Models;
using Discountapp.Domain.Models.Application;
using Discountapp.Domain.Models.Identity;
using Discountapp.Domain.Models.Location;

namespace Discountapp.MVC.ViewModels
{
    using System;
    using System.Collections.Generic;
    using Langx = Infrastructure.Resources.ModelLocalization.Lang;
    public class AddressViewModel : BaseViewModel, IAddress
    {
        public long AddressId { get; set; }
        //public long? MerchantId { get; set; }
        public long UserId { get; set; }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Langx))]
        [Display(Name = nameof(AddressViewModel) + nameof(CityId), ResourceType = typeof(Langx))]
        public long CityId { get; set; }

        //[Required]
        [Display(Name = nameof(AddressViewModel) + nameof(Description), ResourceType = typeof(Langx))]
        public override string Description { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Langx))]
        [Display(Name = nameof(AddressViewModel) + nameof(Information), ResourceType = typeof(Langx))]
        public override string Information { get; set; }
        public string MapJsonCoord { get; set; }

        [DataType(DataType.Time)]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Langx))]
        [Display(Name = nameof(WorkTime), ResourceType = typeof(Langx))]
        public WorkTime WorkTime { get; set; }

        //[DataType(DataType.Time)]
        [Display(Name = nameof(WorkTimeSaturday), ResourceType = typeof(Langx))]
        public WorkTime WorkTimeSaturday { get; set; }

        //[DataType(DataType.Time)]
        [Display(Name = nameof(WorkTimeSunday), ResourceType = typeof(Langx))]
        public WorkTime WorkTimeSunday { get; set; }

        //[Display(Name = nameof(AddressViewModel) + nameof(City), ResourceType = typeof(Langx))]
        public City City { get; set; } = null;
        //public ICollection<AddressMerchant> Merchants { get; set; }
        public AppUser User { get; set; }

        public static explicit operator Address(AddressViewModel model)
            => new Address
            {
                AddressId = model.AddressId,
                CityId = model.CityId,
                //UserId = model.UserId,
                Information = model.Information,
                Description = model.Description,
                MapJsonCoord = model.MapJsonCoord,
                WorkTime = model.WorkTime,
                WorkTimeSaturday = model.WorkTimeSaturday,
                WorkTimeSunday = model.WorkTimeSunday,
                City = model.City,
                //Merchants = model.Merchants,
                //User = model.User,
            };

        public static explicit operator AddressViewModel(Address model)
            => new AddressViewModel
            {
                AddressId = model.AddressId,
                CityId = model.CityId,
                //UserId = model.UserId,
                Information = model.Information,
                Description = model.Description,
                MapJsonCoord = model.MapJsonCoord,
                WorkTime = model.WorkTime,
                WorkTimeSaturday = model.WorkTimeSaturday,
                WorkTimeSunday = model.WorkTimeSunday,
                //City = model.City
            };
    }
}