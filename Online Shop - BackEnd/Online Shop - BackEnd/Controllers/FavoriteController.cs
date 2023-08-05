using Microsoft.AspNetCore.Mvc;

namespace Online_Shop___BackEnd.Controllers
{
    public class FavoriteController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
