using System.Linq;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Data.Entity;
using FamPhotos.Models;
using Microsoft.AspNet.Authorization;

namespace FamPhotos.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Admin
        public IActionResult Index()
        {
            return View(_context.ApplicationUser.ToList());
        }

        // GET: Admin/Details/5
        public IActionResult Details(string id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            ApplicationUser applicationUser = _context.ApplicationUser.Single(m => m.Id == id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }

            return View(applicationUser);
        }

        // GET: Admin/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ApplicationUser applicationUser)
        {
            if (ModelState.IsValid)
            {
                _context.ApplicationUser.Add(applicationUser);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(applicationUser);
        }

        // GET: Admin/Edit/5
        public IActionResult Edit(string id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            ApplicationUser applicationUser = _context.ApplicationUser.Single(m => m.Id == id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        // POST: Admin/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ApplicationUser applicationUser)
        {
            if (ModelState.IsValid)
            {
                _context.Update(applicationUser);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(applicationUser);
        }

        // GET: Admin/Delete/5
        [ActionName("Delete")]
        public IActionResult Delete(string id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            ApplicationUser applicationUser = _context.ApplicationUser.Single(m => m.Id == id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }

            return View(applicationUser);
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(string id)
        {
            ApplicationUser applicationUser = _context.ApplicationUser.Single(m => m.Id == id);
            _context.ApplicationUser.Remove(applicationUser);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
