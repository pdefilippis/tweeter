using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using TweetAgregator.Services.Abstractions;

namespace TweetAgregator.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PublicationController(IPublicationService _publicationService) : BaseController
    {
        [HttpPost(Name = "Post")]
        public async Task<ActionResult<Model.PublicationResponse?>> Post(Model.PublicationRequest publication)
        {
            try
            {
                var user = this.GetUser();
                if (!user.HasValue)
                    return Unauthorized("Usuario no registrado");

                return await _publicationService.SaveMessageAsync(user.Value, publication);

            } catch(RpcException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = ex.Status.Detail });
            }
           
        }
    }
}
