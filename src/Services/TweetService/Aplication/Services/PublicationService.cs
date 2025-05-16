using FluentValidation;
using Grpc.Core;
using TweetService.Aplication.Abstractions;

namespace TweetService.Aplication.Services
{
    public class PublicationService(IMessagePublisher messagePublisher, IValidator<MessageRequest> validator) : Publication.PublicationBase    
    {
        public override async Task<MessageResponse> SaveMessage(MessageRequest request, ServerCallContext context)
        {
            await ValidateModel(request);
            await messagePublisher.PublishAsync(request);
            
            //TODO: Se agrega un objecto de respuesta para facilitar si en un futuro cercano es necesario devolver mas informacion
            return new MessageResponse
            {
                Success = true,
            };
        }

        private async Task ValidateModel(MessageRequest request)
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
                throw new RpcException(new Status(StatusCode.InvalidArgument, string.Join(".", validationResult.Errors.Select(x => x.ErrorMessage))));
        }
    }
}
