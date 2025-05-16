using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace TweetAgregator.Controllers
{
    public class BaseController : Controller
    {
        protected int? GetUser()
        {
            int idUser;
            Request.Headers.TryGetValue("User", out StringValues headerValues);
            return int.TryParse(headerValues.FirstOrDefault(), out idUser) ? idUser : null;
        }
    }
}
