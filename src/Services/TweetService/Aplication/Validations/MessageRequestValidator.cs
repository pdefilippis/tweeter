using FluentValidation;

namespace TweetService.Aplication.Validations
{
    public class MessageRequestValidator : AbstractValidator<MessageRequest>
    {
        public MessageRequestValidator() 
        {
            RuleFor(message => message.Message)
                .NotEmpty().WithMessage("No es posible guardar un mensaje vacio")
                .NotNull().WithMessage("No es posible guardar un mensaje vacio")
                .Length(2, 280).WithMessage("El mensaje debe ser mayor a 2 y menor a 280 caracteres");


        }
    }
}
