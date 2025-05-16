using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using TimeLineAgregator.Model;
using TimeLineAgregator.Services.Abstractions;

namespace TimeLineAgregator.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TimelineController(ITimelineService timelineService) : BaseController
    {
        [HttpGet(Name = "Get")]
        public async Task<ActionResult<TweetHeaderFilterResponse?>> Get(TweetFilterRequest tweetFilter)
        {
            try
            {
                var user = this.GetUser();
                if (!user.HasValue)
                    return Unauthorized("Usuario no registrado");

                return await timelineService.GetTimeline(user.Value, tweetFilter);
            }
            catch (RpcException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = ex.Status.Detail });
            }
        }
    }
}
