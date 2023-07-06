using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using QuickCampus_Core.Services;
using QuickCampus_DAL.Context;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace QuickCampus_Core.ViewModel
{
    public class ClientVM

    {
        public static explicit operator ClientVM(TblClient items)
        {
            return new ClientVM
            {
                Id = items.Id,
                Name = items.Name,
                //CraetedBy = items.CraetedBy,
                //CreatedDate = items.CreatedDate,
                // ModifiedBy = items.ModifiedBy,
                //ModofiedDate = items.ModofiedDate,
                Address = items.Address,
                Phone= items.Phone,
                Email = items.Email,
                SubscriptionPlan = items.SubscriptionPlan,
                Geolocation = items.Geolocation,
                //IsActive = items.IsActive,
                //IsDeleted = items.IsDeleted,
            };
        }
        public int Id { get; set; }
        //[Required(ErrorMessage = "Name is required")]
        //[RegularExpression(@"^[a-zA-Z][a-zA-Z\s]+$", ErrorMessage = "Only characters allowed.")]
        [Remote("IsAlreadyExist", "Client", HttpMethod = "POST", ErrorMessage = "Name already exists in database.")]
        public string? Name { get; set; }

       // public int? CraetedBy { get; set; }

        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
       // public DateTime CreatedDate { get; set; }

      //  public int? ModifiedBy { get; set; }

      //  [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
       // public DateTime ModofiedDate { get; set; }
        public string? Address { get; set; }
        [Required]
        [RegularExpression(@"^[1-9][0-9]{9}$", ErrorMessage = "Please enter a valid 10-digit mobile number that does not start with 0.")]
        public string? Phone { get; set; }

        public string? Email { get; set; }

        public string? Geolocation { get; set; }

        public string? SubscriptionPlan { get; set; }

        //public bool? IsActive { get; set; }

        //public bool? IsDeleted { get; set; }


        //ublic IEnumerable<SelectListItem> rec { get; set; }

        public TblClient ToClientDbModel()
        {
            return new TblClient
            {
                Name = Name,
                Phone = Phone,
                Email = Email,
                Geolocation = Geolocation,
                SubscriptionPlan = SubscriptionPlan,
                //ModifiedBy = ModifiedBy,
                //ModofiedDate = ModofiedDate,
                //CraetedBy = CraetedBy,
                Address = Address,
               // CreatedDate = CreatedDate,
                IsActive = true,
                IsDeleted = false,



               
            };
        }

        public TblClient ToUpdateDbModel()
        {
            return new TblClient
            {
                Id = Id,
                Name = Name,
                Phone= Phone,
                Email = Email,
                SubscriptionPlan = SubscriptionPlan,
                Geolocation = Geolocation,
               // ModifiedBy = ModifiedBy,
               //ModofiedDate = ModofiedDate,
               //CraetedBy = CraetedBy,
               // CreatedDate = CreatedDate,
                IsActive = true,
                IsDeleted = false,
            };
        }

        public class ClientValidator : AbstractValidator<ClientVM>
        {
            public ClientValidator()
            {
                RuleFor(x => x.Name)
                   .Cascade(CascadeMode.StopOnFirstFailure)
                   .NotNull().WithMessage("Name could not be null")
                   
                   .NotEmpty().WithMessage("Name could not be empty")
                   .Length(0, 20).WithMessage("Name lengh could not be greater than 20");

                RuleFor(x => x.Address)
                  .Cascade(CascadeMode.StopOnFirstFailure)
                  .NotNull().WithMessage("Address could not be null")
                  .NotEmpty().WithMessage("Address could not be empty")
                  .Length(0, 100).WithMessage("Address lengh could not be greater than 100");

                RuleFor(x => x.Phone)
                  .Cascade(CascadeMode.StopOnFirstFailure)
                  .NotNull().WithMessage("Phone could not be null")
                  .NotEmpty().WithMessage("Phone could not be empty");
                  
       //           .MinimumLength(10).WithMessage("PhoneNumber must not be less than 10 characters.")
       //.MaximumLength(20).WithMessage("PhoneNumber must not exceed 50 characters.")
       //.Matches(new Regex(@"((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}")).WithMessage("PhoneNumber not valid");

                RuleFor(x => x.Email)
                  .Cascade(CascadeMode.StopOnFirstFailure).EmailAddress()
                  .NotNull().WithMessage("Email could not be null")
                  .NotEmpty().WithMessage("Email could not be empty");
                  

                RuleFor(x => x.SubscriptionPlan)
                  .Cascade(CascadeMode.StopOnFirstFailure)
                  .NotNull().WithMessage("SubscriptionPlan could not be null")
                  .NotEmpty().WithMessage("SubscriptionPlan could not be empty")
                  .Length(0, 20).WithMessage("SubscriptionPlan lengh could not be greater than 20");

                RuleFor(x => x.Geolocation)
                 .Cascade(CascadeMode.StopOnFirstFailure)
                 .NotNull().WithMessage("Geolocation could not be null")
                 .NotEmpty().WithMessage("Geolocation could not be empty")
                 .Length(0, 20).WithMessage("Geolocation lengh could not be greater than 20");
            }
            //private async Task<bool> IsUniquename(string Name, CancellationToken token)
            //{
            //    bool isExistingname = await ClientRepo.UsernameExistsAsync(Name);
            //    return isExistingname;
            //}
        }


    }
}
