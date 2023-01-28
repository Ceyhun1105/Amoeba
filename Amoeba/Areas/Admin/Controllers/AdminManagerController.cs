using Amoeba.Areas.Admin.ViewModels;
using Amoeba.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Amoeba.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class AdminManagerController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AdminManagerController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(AdminLoginViewModel adminVM)
        {
            if (!ModelState.IsValid) return View(adminVM);

            AppUser admin = await _userManager.FindByNameAsync(adminVM.UserName);
            if(admin is null)
            {
                ModelState.AddModelError("", "Username or Password is invalid");
                return View(adminVM);
            }
            var result = await _signInManager.PasswordSignInAsync(admin, adminVM.Password, false, false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Username or Password is invalid");
                return View(adminVM);
            }

            return RedirectToAction("index", "dashboard");

        }

        public async Task<IActionResult> Logout()
        {
            if (User.Identity.IsAuthenticated)
            {
                await _signInManager.SignOutAsync();
            }
            return RedirectToAction("Index", "home", new { area = "null" });
        }






      /*  public async Task<IActionResult> CreateAdmin()
        {
            AppUser admin = new AppUser
            {
                UserName = "Admin2023",
                FullName = "Admin"
            };
            var result = await _userManager.CreateAsync(admin, "Admin2023");

            return Ok(result);
        }

        public async Task<IActionResult> CreateRole()
        {
            IdentityRole role1 = new IdentityRole("Admin");
            IdentityRole role2 = new IdentityRole("Member");

            await _roleManager.CreateAsync(role1);
            await _roleManager.CreateAsync(role2);

            return Ok("created roles");
        }

        public async Task<IActionResult> SetRole()
        {
            AppUser admin = await _userManager.FindByNameAsync("Admin2023");

            var result = await _userManager.AddToRoleAsync(admin, "Admin");
            return Ok(result);
        }*/
    }
}
