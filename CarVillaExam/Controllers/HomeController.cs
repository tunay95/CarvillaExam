using CarVillaExam.DAL;
using CarVillaExam.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CarVillaExam.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _dbContext;

        public HomeController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task <IActionResult> Index()
        {
            List<Service> services = await _dbContext.Services.ToListAsync();
            return View(services);
        }

    }
}