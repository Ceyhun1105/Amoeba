using Amoeba.DBContextFIles;
using Amoeba.Helpers;
using Amoeba.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Amoeba.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TeamController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public TeamController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int page = 1)
        {
            var query = _context.Teams.Include(x=>x.Position).AsQueryable();
            List<Team> teams = PaginatedList<Team>.Create(query, page, 2);
            /*List<Team> teams = _context.Teams.Include(x=>x.Position).ToList();*/
            return View(teams);
        }

        public IActionResult Create()
        {
            ViewBag.Positions = _context.Positions.ToList();

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Team team)
        {


            ViewBag.Positions = _context.Positions.ToList();
            if(!ModelState.IsValid) return View(team);

            if(team.ImageFile is null)
            {
                ModelState.AddModelError("ImageFile", "Required");
                return View(team);
            }

            if (!team.ImageFile.CheckFileLength(1048676 * 3))
            {
                ModelState.AddModelError("ImageFile", "Please,Upload less than 3 mb");
                return View(team);
            }   
            if (!team.ImageFile.CheckFileType())
            {
                ModelState.AddModelError("ImageFile", "Please,Upload only jpeg , jpg and png files");
                return View(team);
            }


            team.ImageUrl = team.ImageFile.SaveFile(_env.WebRootPath,"uploads/teams");

            _context.Teams.Add(team);
            _context.SaveChanges();


            return RedirectToAction(nameof(Index));
        }


        public IActionResult Update(int id)
        {
            ViewBag.Positions = _context.Positions.ToList();
            Team existteam = _context.Teams.FirstOrDefault(t => t.Id == id);
            if(existteam == null) return NotFound();

            ViewBag.Image = existteam.ImageUrl;

            return View(existteam);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Update(Team team)
        {
            ViewBag.Positions = _context.Positions.ToList();
            
            Team existteam = _context.Teams.FirstOrDefault(t => t.Id == team.Id);
            if(existteam is null) return NotFound();
            ViewBag.Image = existteam.ImageUrl;

            if (!ModelState.IsValid) return View(team);

            if(team.ImageFile is not null)
            {

                if (!team.ImageFile.CheckFileLength(1048676 * 3))
                {
                    ModelState.AddModelError("ImageFile", "Please,Upload less than 3 mb");
                    return View(team);
                }
                if (!team.ImageFile.CheckFileType())
                {
                    ModelState.AddModelError("ImageFile", "Please,Upload only jpeg , jpg and png files");
                    return View(team);
                }

                team.ImageUrl = team.ImageFile.SaveFile(_env.WebRootPath, "uploads/teams");

                string path = Path.Combine(_env.WebRootPath, "uploads/teams", existteam.ImageUrl);

                if(System.IO.File.Exists(path)) System.IO.File.Delete(path);

                existteam.ImageUrl = team.ImageUrl;

            }

            existteam.FullName = team.FullName;
            existteam.Description = team.Description;
            existteam.PositionId = team.PositionId;
            existteam.InstaUrl = team.InstaUrl;
            existteam.TwitterUrl = team.TwitterUrl;
            existteam.LnUrl = team.LnUrl;
            existteam.FbUrl = team.FbUrl;   
            
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));

        }


        public IActionResult Delete(int id)
        {
            Team existteam = _context.Teams.FirstOrDefault(t => t.Id == id);
            if (existteam == null) return NotFound();

            string path = Path.Combine(_env.WebRootPath, "uploads/teams", existteam.ImageUrl);

            if (System.IO.File.Exists(path)) System.IO.File.Delete(path);

            _context.Teams.Remove(existteam);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }





    }
}
