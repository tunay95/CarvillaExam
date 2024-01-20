using CarVillaExam.Areas.Admin.ViewModels;
using CarVillaExam.DAL;
using CarVillaExam.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarVillaExam.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ServiceController:Controller
    {
        private readonly AppDbContext _dbContext;

        public ServiceController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            List<Service> services = await _dbContext.Services.ToListAsync();
            return View(services);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {

            return View();
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateServiceVM createServiceVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            Service service = new Service()
            {
                Title = createServiceVM.Title,
                Description = createServiceVM.Description,
                Icon = createServiceVM.Icon,
            };

            await _dbContext.AddAsync(service);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index),"Service");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id)
        {
            Service service = await _dbContext.Services.Where(x=>x.Id==id).FirstOrDefaultAsync();

            UpdateServiceVM updateServiceVM = new UpdateServiceVM()
            {
                Title = service.Title,
                Description = service.Description,
                Icon = service.Icon,
            };
            return View(updateServiceVM);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(UpdateServiceVM updateServiceVM)
        {
            if (!ModelState.IsValid) return View();

            Service service = await _dbContext.Services.Where(x=>x.Id == updateServiceVM.Id).FirstOrDefaultAsync();
            if (service == null) throw new Exception("Service shpuldn't be null!");

            service.Title = updateServiceVM.Title;
            service.Description = updateServiceVM.Description;
            service.Icon = updateServiceVM.Icon;

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            Service service =  _dbContext.Services.Where(x => x.Id == id).FirstOrDefault();

            _dbContext.Remove(service);
            _dbContext.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

    }
}
