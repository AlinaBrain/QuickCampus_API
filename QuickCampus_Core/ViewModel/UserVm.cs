using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using QuickCampus_DAL.Context;
using System.ComponentModel.DataAnnotations;

namespace QuickCampus_Core.ViewModel
{
    public class UserVm
    {

        public static explicit operator UserVm(TblUser item)
        {
            return new UserVm
            {
                //Id = item.Id,
                ClientId = item.ClientId,
                UserName = item.UserName,
                Name = item.Name,
               Password = item.Password,
                IsDelete = item.IsDelete,
                IsActive = item.IsActive,
                Email = item.Email,
                Mobile = item.Mobile,
                
            };

        }
        public int Id { get; set; }
        public int?ClientId { get; set; }
        
        public string? UserName { get; set; }
       // [Required, MaxLength(50)]
        public string? Name { get; set; }
        [Required]
        [DataType(DataType.Password)]
       
        public string? Password { get; set; }

        public bool? IsDelete { get; set; }

        public bool? IsActive { get; set; }
        [Required, MaxLength(100)]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email address.")]
        [Remote("EmailExist", "User", AdditionalFields = "Id", ErrorMessage = ("Email already exist!"))]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string? Email { get; set; }
        [Required,MaxLength(10)]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Mobile number must be a 10-digit number.")]
        public string? Mobile { get; set; }
        [Required]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }


        public TblUser toUserDBModel()
        {
            return new TblUser
            {
                //Id = Id,
                ClientId= ClientId,
                UserName = UserName,
                Name = Name,
                Password = Password,
                Email = Email,
                Mobile = Mobile,
                IsDelete = false,
                IsActive = true,
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
                           .Length(0, 20).WithMessage("Name lengh could not be greater than 20");

                RuleFor(x => x.UserName)
                  .Cascade(CascadeMode.StopOnFirstFailure)
                   .NotNull().WithMessage("Name could not be null")

                   .NotEmpty().WithMessage("Name could not be empty")
                   .Length(0, 20).WithMessage("Name lengh could not be greater than 20");

                RuleFor(x => x.Mobile)
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


            }
            //private async Task<bool> IsUniquename(string Name, CancellationToken token)
            //{
            //    bool isExistingname = await ClientRepo.UsernameExistsAsync(Name);
            //    return isExistingname;
            //}
        }
    }
}
