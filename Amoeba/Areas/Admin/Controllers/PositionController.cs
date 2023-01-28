using Amoeba.DBContextFIles;
using Amoeba.Helpers;
using Amoeba.Models;
using Microsoft.AspNetCore.Mvc;

namespace Amoeba.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PositionController : Controller
    {
        private readonly AppDbContext _context;
        

        public PositionController(AppDbContext context)
        {
            _context = context;
           
        }
        public IActionResult Index(int page = 1)
        {
            var query = _context.Positions.AsQueryable();
            List<Position> positions = PaginatedList<Position>.Create(query,page,2);
           /* List<Position> positions = _context.Positions.ToList();*/
            return View(positions);
        }

        public IActionResult Create()
        {
            ViewBag.Positions = _context.Positions.ToList();

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Position position)
        {


            if (!ModelState.IsValid) return View(position);

            _context.Positions.Add(position);
            _context.SaveChanges();


            return RedirectToAction(nameof(Index));
        }


        public IActionResult Update(int id)
        {
            Position existposition = _context.Positions.FirstOrDefault(t => t.Id == id);
            if (existposition == null) return NotFound();
            return View(existposition);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Update(Position position)
        {

            Position existposition = _context.Positions.FirstOrDefault(t => t.Id == position.Id);
            if (existposition is null) return NotFound();

            if (!ModelState.IsValid) return View(position);
          
            existposition.Name = position.Name;

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));

        }


        public IActionResult Delete(int id)
        {
            Position existposition = _context.Positions.FirstOrDefault(t => t.Id == id);
            if (existposition == null) return NotFound();

            _context.Positions.Remove(existposition);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }





    }
}
