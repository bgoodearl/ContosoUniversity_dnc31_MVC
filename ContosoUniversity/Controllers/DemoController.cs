using Microsoft.AspNetCore.Mvc;

namespace ContosoUniversity.Controllers
{
    [Route("[Controller]/[Action]")]
    public class DemoController : Controller
    {
        [Route("~/[Controller]")]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Counter()
        {
            return View();
        }

        public IActionResult TabDemo()
        {
            return View();
        }
    }
}
