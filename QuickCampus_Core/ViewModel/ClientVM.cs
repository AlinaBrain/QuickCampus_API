using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using QuickCampus_DAL.Context;
using System.ComponentModel.DataAnnotations;

namespace QuickCampus_Core.ViewModel
{
    public class ClientVM

    {
        public static explicit operator ClientVM(TblClient items)
        {
            return new ClientVM
            {
                Name = items.Name,
                Address = items.Address,
                Phone= items.Phone,
                Email = items.Email,
                SubscriptionPlan = items.SubscriptionPlan,
                Latitude = items.Latitude,
                Longitude = items.Longitude,
                UserName = items.UserName,
                Password = items.Password,
                ConfirmPassword = items.Password,
                IsActive=items.IsActive,
                IsDeleted=items.IsDeleted,
                CreatedDate=items.CreatedDate,
                ModifiedBy=items.ModifiedBy,
            };
        }
        public int Id { get; set; }
        public string? Name { get; set; }

       public int? CraetedBy { get; set; }

       public DateTime? CreatedDate { get; set; }

       public int? ModifiedBy { get; set; }

        public DateTime? ModofiedDate { get; set; }
        public string? Address { get; set; }
        [Required]
        [RegularExpression(@"^[1-9][0-9]{9}$", ErrorMessage = "Please enter a valid 10-digit mobile number that does not start with 0.")]
        public string? Phone { get; set; }

        public string? Email { get; set; }

        public string? SubscriptionPlan { get; set; }

        public bool? IsActive { get; set; }
        public decimal? Longitude { get; set; }

        public decimal? Latitude { get; set; }
        public bool? IsDeleted { get; set; }
        public string? UserName { get; set; }

        public string? Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }
        public TblClient ToClientDbModel()
        {
            return new TblClient
            {  
                Name = Name,
                Phone = Phone,
                Email = Email,
                Longitude = Longitude,
                Latitude = Latitude,
                SubscriptionPlan = SubscriptionPlan,
                ModifiedBy = ModifiedBy,
                ModofiedDate = Id>0 ?DateTime.UtcNow : null,
                CraetedBy = CraetedBy,
                CreatedDate = DateTime.UtcNow,
                Address = Address,
                IsActive = true,
                IsDeleted = false,
                UserName = UserName,
                Password = Password,
            };
        }

        public TblClient ToUpdateDbModel()
        {
            return new TblClient
            {
                Id = Id,
                Name = Name,
                Phone = Phone,
                Email = Email,
                SubscriptionPlan = SubscriptionPlan,
                Longitude=Longitude,
                Latitude=Latitude,
                ModifiedBy = ModifiedBy,
                ModofiedDate = DateTime.UtcNow,
                CraetedBy = CraetedBy,
                CreatedDate= Id<0? DateTime.Now : null,
                Address = Address,
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
            .Length(3, 100);
                RuleFor(x => x.Address)
                  .Cascade(CascadeMode.StopOnFirstFailure)
                  .NotNull().WithMessage("Address could not be null")
                  .NotEmpty().WithMessage("Address could not be empty")
                  .Length(0, 100).WithMessage("Address lengh could not be greater than 100");

                RuleFor(x => x.Phone)
                  .Cascade(CascadeMode.StopOnFirstFailure)
                  .NotNull().WithMessage("Phone could not be null")
                  .NotEmpty().WithMessage("Phone could not be empty");

                RuleFor(x => x.Email)
                  .Cascade(CascadeMode.StopOnFirstFailure).EmailAddress()
                  .NotNull().WithMessage("Email could not be null")
                  .NotEmpty().WithMessage("Email could not be empty");

                RuleFor(x => x.SubscriptionPlan)
                  .Cascade(CascadeMode.StopOnFirstFailure)
                  .Length(0, 20).WithMessage("SubscriptionPlan lengh could not be greater than 20");

                RuleFor(x => x.UserName)
               .Cascade(CascadeMode.StopOnFirstFailure).EmailAddress()
               .NotNull().WithMessage("UserName could not be null")
               .NotEmpty().WithMessage("UserName could not be empty")
               .Length(3, 100).WithMessage("UserName lengh could not be greater than 100");

                RuleFor(x => x.Password)
                  .Cascade(CascadeMode.StopOnFirstFailure)
                  .NotNull().WithMessage("Password could not be null")
                  .NotEmpty().WithMessage("Password could not be empty")
                  .Length(4, 50).WithMessage("Length Should be Greater Than 4 and less than 50");
            }
        }
    }
    public class ClientReponse
    {
        public static explicit operator ClientReponse(TblClient items)
        {
            return new ClientReponse
            {
                Id = items.Id,
                Name = items.Name,
                Address = items.Address,
                Phone = items.Phone,
                Email = items.Email,
                SubscriptionPlan = items.SubscriptionPlan,
                Latitude = items.Latitude,
                Longitude = items.Longitude,
                CreatedDate =items.CreatedDate,
                ModofiedDate = items.ModofiedDate,
                CraetedBy = items.CraetedBy,
                ModifiedBy = items.ModifiedBy,
                IsActive = items.IsActive,
                IsDeleted = items.IsDeleted
            };
        }
        public int Id { get; set; }

        [Remote("IsAlreadyExist", "Client", HttpMethod = "POST", ErrorMessage = "Name already exists in database.")]
        public string? Name { get; set; }

        public int? CraetedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? ModofiedDate { get; set; }
        public string? Address { get; set; }
        [Required]
        [RegularExpression(@"^[1-9][0-9]{9}$", ErrorMessage = "Please enter a valid 10-digit mobile number that does not start with 0.")]
        public string? Phone { get; set; }

        public string? Email { get; set; }

        public string? SubscriptionPlan { get; set; }

        public bool? IsActive { get; set; }
        public decimal? Longitude { get; set; }

        public decimal? Latitude { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
