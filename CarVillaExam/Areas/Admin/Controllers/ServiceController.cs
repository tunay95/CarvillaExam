using Microsoft.AspNetCore.Mvc;

namespace CarVillaExam.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ServiceController:Controller
    {
        public IActionResult Index()
        {
            return View();
        }


    }
}
