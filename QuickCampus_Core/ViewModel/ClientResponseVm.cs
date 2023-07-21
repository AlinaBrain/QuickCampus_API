using FluentValidation;
using QuickCampus_DAL.Context;

namespace QuickCampus_Core.ViewModel
{
    public class ClientResponseVm
    {
        public static explicit operator ClientResponseVm(TblClient items)
        {
            return new ClientResponseVm
            {
                Id = items.Id,
                Name = items.Name,
                Address = items.Address,
                Phone = items.Phone,
                Email = items.Email,
                CraetedBy = items.CraetedBy,
                CreatedDate = items.CreatedDate,
                ModifiedBy = items.ModifiedBy,
                ModofiedDate = items.ModofiedDate,
                SubscriptionPlan = items.SubscriptionPlan,
                Latitude = items.Latitude,
                Longitude = items.Longitude,
                IsActive = items.IsActive,
                IsDeleted = items.IsDeleted,
            };
        }
        public int Id { get; set; }

        public string? Name { get; set; }

        public int? CraetedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? ModofiedDate { get; set; }

        public bool? IsActive { get; set; }

        public bool? IsDeleted { get; set; }

        public string? Address { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public string? SubscriptionPlan { get; set; }

        public decimal? Longitude { get; set; }

        public decimal? Latitude { get; set; }
        
    }

    public class ClientUpdateRequest
    {
        public static explicit operator ClientUpdateRequest(TblClient items)
        {
            return new ClientUpdateRequest
            {
                Id = items.Id,
                Name = items.Name,
                Address = items.Address,
                Phone = items.Phone,
                Email = items.Email,
                SubscriptionPlan = items.SubscriptionPlan,
                Latitude = items.Latitude,
                Longitude = items.Longitude, 
            };
        }
        public int Id { get; set; }

        public string? Name { get; set; }
        public string? Address { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public string? SubscriptionPlan { get; set; }

        public decimal? Longitude { get; set; }

        public decimal? Latitude { get; set; }

    }
  
    public class ClientValidatorRequest : AbstractValidator<ClientUpdateRequest>
    {
        public ClientValidatorRequest()
        {
            RuleFor(x => x.Name)
                 .Cascade(CascadeMode.StopOnFirstFailure)
              .NotNull().WithMessage("Name could not be null")

              .NotEmpty().WithMessage("Name could not be empty")
        .Matches(@"^[A-Za-z\s]*$").WithMessage("'{PropertyName}' should only contain letters.")
        .Length(3, 30);
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
        }
    }

}
