using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SOF03.Data;
using SOF03.Models;
using System.Diagnostics;

namespace SOF03.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<SiteUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManger;
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;

        public HomeController(UserManager<SiteUser> userManager, RoleManager<IdentityRole> roleManger, ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _userManager = userManager;
            _roleManger = roleManger;
            _logger = logger;
            _db = db;
        }

        public async Task<IActionResult> DelegateAdmin()
        {
            var principal = this.User;
            var user = await _userManager.GetUserAsync(principal);
            var role = new IdentityRole()
            {
                Name = "Admin"
            };
            if (!await _roleManger.RoleExistsAsync("Admin"))
            {
                await _roleManger.CreateAsync(role);
            }
            await _userManager.AddToRoleAsync(user, "Admin");
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Admin()
        {
            return View(_db.Jobs);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Users()
        {
            return View(_userManager.Users);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveAdmin(string uid)
        {
            var user = _userManager.Users.FirstOrDefault(t => t.Id == uid);
            await _userManager.RemoveFromRoleAsync(user, "Admin");
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GrantAdmin(string uid)
        {
            var user = _userManager.Users.FirstOrDefault(t => t.Id == uid);
            await _userManager.AddToRoleAsync(user, "Admin");
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Index()
        {
            return View(_db.Jobs);
        }

        [Authorize]
        public IActionResult Add()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Add(Job job)
        {
            job.OwnerId = _userManager.GetUserId(this.User);
            ;
            var old = _db.Jobs.FirstOrDefault(t => t.Name == job.Name && t.OwnerId == job.OwnerId);
            if (old == null)
            {
            _db.Jobs.Add(job);
            _db.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(string uid)
        {
            var item = _db.Jobs.FirstOrDefault(t => t.Uid == uid);
            if (item != null && item.OwnerId == _userManager.GetUserId(this.User))
            {
                _db.Jobs.Remove(item);
                _db.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AdminDelete(string uid)
        {
            var item = _db.Jobs.FirstOrDefault(t => t.Uid == uid);
            if (item != null)
            {
                _db.Jobs.Remove(item);
                _db.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }











        [Authorize]
        public async Task<IActionResult> Privacy()
        {
            var principal = this.User;
            var user = await _userManager.GetUserAsync(principal);
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}