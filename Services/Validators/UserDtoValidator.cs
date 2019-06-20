using FluentValidation;
using Services.Dtos;

namespace Services.Validators
{
    public class UserDtoValidator : AbstractValidator<UserCreationDto>
    {
        public UserDtoValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty();
            RuleFor(x => x.Email).NotNull().NotEmpty();
        }
    }
}
