
using SAEA.MVC;
using SAEA.WebRedisManager.Services;

namespace SAEA.WebRedisManager.Controllers
{
    public class UpdateController : Controller
    {
        public ActionResult GetLatest()
        {
            return Json(new UpdateService().GetLatest());
        }
    }
}
