using FluentValidation;

namespace QuickCampus_Core.ViewModel
{
    public class AdminLogin
    {
        public string? UserName { get; set; }
        public String? Password { get; set; }
    }


    public class AdminLoginValidator : AbstractValidator<AdminLogin>
    {
        public AdminLoginValidator()
        {
            RuleFor(x => x.UserName)
               .Cascade(CascadeMode.StopOnFirstFailure)
               .NotNull().WithMessage("UserName could not be null")
               .NotEmpty().WithMessage("UserName could not be empty")
               .Length(0, 100).WithMessage("UserName lengh could not be greater than 100");

            RuleFor(x => x.Password)
              .Cascade(CascadeMode.StopOnFirstFailure)
              .NotNull().WithMessage("Password could not be null")
              .NotEmpty().WithMessage("Password could not be empty")
              .Length(0, 500).WithMessage("Password lengh could not be greater than 500");
            
        }
    }
}
