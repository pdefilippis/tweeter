using FollowAgregator.Model;
using FollowAgregator.Services.Abstractions;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;

namespace FollowAgregator.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FollowController(IFollowService followService) : BaseController
    {
        [HttpPost(Name = "Post")]
        public async Task<ActionResult<FollowResponse?>> Post(FollowRequest follow)
        {
            try
            {
                var user = this.GetUser();
                if (!user.HasValue)
                    return Unauthorized("Usuario no registrado");

                return await followService.SavFollowAsync(user.Value, follow);
            }
            catch (RpcException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = ex.Status.Detail });
            }
        }
    }
}
