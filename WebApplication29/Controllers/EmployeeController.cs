using Microsoft.AspNetCore.Mvc;

namespace WebApplication29.Controllers
{
    public class EmployeeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
