using Microsoft.AspNetCore.Mvc;

namespace Organic_Food_MVC_Project.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult NotFoundException()
        {
            return View();
        }
    }
}
