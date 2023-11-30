using FluentValidation;
using PolicyExample.Dtos;

namespace PolicyExample.ValidationRules
{
    public class UserLoginDtoValidator : AbstractValidator<UserLoginDto>
    {
        public UserLoginDtoValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("kullanıcı adını boş bırakmazsın");
            RuleFor(x => x.UserName).Length(3, 100).WithMessage("kullanıcı adını 3-100 karakter arasında olmalı");
            RuleFor(x => x.Password).NotEmpty().WithMessage("şifre boş bırakılamaz!!!");
        }
    }
}
