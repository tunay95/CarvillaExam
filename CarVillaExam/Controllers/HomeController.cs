using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CarVillaExam.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

    }
}