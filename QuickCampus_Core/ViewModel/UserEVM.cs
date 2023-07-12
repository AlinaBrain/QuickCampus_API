using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using QuickCampus_DAL.Context;
using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace QuickCampus_Core.ViewModel
{
    public class UserEVm
    {

        public static explicit operator UserEVm(TblUser item)
        {
            return new UserEVm
            {
                Id = item.Id,
                ClientId = item.ClientId,
                UserName = item.UserName,
                Name = item.Name,
               // Password = item.Password,
                //IsDelete = item.IsDelete,
               // IsActive = item.IsActive,
                Email = item.Email,
                Mobile = item.Mobile,

            };

        }
        public int Id { get; set; }
        public int? ClientId { get; set; }

        public string? UserName { get; set; }
       
        public string? Name { get; set; }
       
       // public bool? IsDelete { get; set; }

      //  public bool? IsActive { get; set; }
       
        public string? Email { get; set; }
       // public string? Password { get; set; }
        public string? Mobile { get; set; }

        //public string? ConfirmPassword { get; set; }


        public TblUser toUserDBModel()
        {
            return new TblUser
            {
                Id = Id,
                ClientId = ClientId,
                UserName = UserName,
                Name = Name,
               // Password = Password,
                Email = Email,
                Mobile = Mobile,
                //IsDelete = false,
               // IsActive = true,
            };
        }

        public TblUser ToUpdateDbModel()
        {
            return new TblUser
            {
                Id = Id,
                UserName = UserName,
                Name = Name,
               // Password = Password,
                Email = Email,
                Mobile = Mobile,
            };
        }


        public class UserValidator : AbstractValidator<UserVm>
        {
            public UserValidator()
            {
                RuleFor(x => x.Name)
                   .Cascade(CascadeMode.StopOnFirstFailure)
                  .NotNull().WithMessage("Name could not be null")

                  .NotEmpty().WithMessage("Name could not be empty")
            .Matches(@"^[A-Za-z\s]*$").WithMessage("'{PropertyName}' should only contain letters.")
            .Length(3, 30);


                RuleFor(x => x.UserName)
                  .Cascade(CascadeMode.StopOnFirstFailure)
                   .NotNull().WithMessage("UserName could not be null")

                   .NotEmpty().WithMessage("UserName could not be empty")
            .Matches(@"^[A-Za-z\s]*$").WithMessage("'{PropertyName}' should only contain letters.")
            .Length(3, 30);

                RuleFor(x => x.Mobile)
                  .Cascade(CascadeMode.StopOnFirstFailure)
                  .NotNull().WithMessage("Phone could not be null")
                  .NotEmpty().WithMessage("Phone could not be empty");

                //.MinimumLength(10).WithMessage("PhoneNumber must not be less than 10 characters.")
                //.MaximumLength(20).WithMessage("PhoneNumber must not exceed 50 characters.")
                //.Matches(new Regex(@"((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}")).WithMessage("PhoneNumber not valid");

                RuleFor(x => x.Email)
                  .Cascade(CascadeMode.StopOnFirstFailure).EmailAddress()
                  .NotNull().WithMessage("Email could not be null")
                  .NotEmpty().WithMessage("Email could not be empty");


            }
            //private async Task<bool> IsUniquename(string Name, CancellationToken token)
            //{
            //    bool isExistingname = await ClientRepo.UsernameExistsAsync(Name);
            //    return isExistingname;
            //}
        }
    }
}
