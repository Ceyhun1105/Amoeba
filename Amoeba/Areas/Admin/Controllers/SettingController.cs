using Amoeba.DBContextFIles;
using Amoeba.Helpers;
using Amoeba.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Amoeba.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class SettingController : Controller
    {
        private readonly AppDbContext _context;

        public SettingController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int page =1)
        {
            var query = _context.Settings.AsQueryable();
            List<Setting> settings = PaginatedList<Setting>.Create(query, page, 2);
            return View(settings);
        }
        public IActionResult Update(int id)
        {
            Setting setting = _context.Settings.FirstOrDefault(x=>x.Id == id);
            if (setting == null) return NotFound();
            return View(setting);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(Setting setting)
        {
            Setting existsetting = _context.Settings.FirstOrDefault(x=>x.Id == setting.Id);
            if (!ModelState.IsValid) return View(setting);
            existsetting.Value = setting.Value;

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
