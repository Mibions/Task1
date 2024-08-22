using Microsoft.AspNetCore.Mvc;

namespace Category_Task1.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
