using System.Linq;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Data.Entity;
using FamPhotos.Models;
using System.IO;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.AspNet.Http;
using System;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Collections.Generic;
using Microsoft.AspNet.Hosting;
using Microsoft.Net.Http.Headers;
using FamPhotos.ViewModels.Photos;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Authorization;

namespace FamPhotos.Controllers
{
    [Authorize(Roles = "Member")]
    public class PhotosController : Controller
    {
        private ApplicationDbContext _context;
        private IHostingEnvironment _hostingEnvironment;
        private IApplicationEnvironment _applicationEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;


        public PhotosController(ApplicationDbContext context, 
            IHostingEnvironment hostingEnvironment, 
            IApplicationEnvironment applicationEnvironment,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _applicationEnvironment = applicationEnvironment;
            _userManager = userManager;
        } 

        // GET: Photos
        
        public IActionResult Index()
        {
            string userid = User.GetUserId();
            IEnumerable<UserFolder> folders = new List<UserFolder>();

            if (!string.IsNullOrEmpty(userid))
            {
                folders = _context.UserFolder.Where(x => x.ApplicationUserId == userid);
            }
            return View(_context.UserFolder.AsEnumerable());
        }

        // GET: Photos/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Photo photo = _context.Photo.Single(m => m.PhotoID == id);
            if (photo == null)
            {
                return HttpNotFound();
            }

            return View(photo);
        }

        // GET: Photos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Photos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        //Need Binding.
        public async Task<IActionResult> Create(Photo photo, IFormFile files, int id)
        {
            if (files == null)
            {
                ModelState.AddModelError(string.Empty, "Please select a file to upload.");
            }
            else if (ModelState.IsValid)
            {
                photo.UploadDate = DateTime.Now;
                photo.UserFolderId = id;
                
                var folderName = _context.UserFolder.Where(q => q.UserFolderID == id).First().Name;
               
                //TODO:  Check for image types
                var fileName = Guid.NewGuid() + ContentDispositionHeaderValue.Parse(files.ContentDisposition).FileName.Trim('"');
                var filePath = Path.Combine("Photos", User.GetUserName(), folderName, fileName);
                await files.SaveAsAsync(filePath);

                photo.Url = "~/" + filePath;

                _context.Add(photo);
                _context.SaveChanges();


                return ViewFolderContents(id);
            }
            return View(photo);
        }

        // GET: Photos/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Photo photo = _context.Photo.Single(m => m.PhotoID == id);
            if (photo == null)
            {
                return HttpNotFound();
            }

            EditDescriptionViewModel model = new EditDescriptionViewModel() { Description = photo.Description };
            return View(model);
        }

        // POST: Photos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EditDescriptionViewModel model, int? id)
        {
            Photo photo = _context.Photo.Single(m => m.PhotoID == id);
            if (ModelState.IsValid)
            {
                photo.Description = model.Description;
                _context.Update(photo);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // GET: Photos/Delete/5
        [ActionName("Delete")]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Photo photo = _context.Photo.Single(m => m.PhotoID == id);
            if (photo == null)
            {
                return HttpNotFound();
            }

            return View(photo);
        }

        // POST: Photos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            Photo photo = _context.Photo.Single(m => m.PhotoID == id);
            var filePath = photo.Url;
            FileInfo fileInfo = new FileInfo(filePath);
            fileInfo.Delete();
            _context.Photo.Remove(photo);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        //GET:  Photos/CreateNewFolder
        public IActionResult CreateNewFolder()
        {
            return View();
        }

        //Post: Photos/CreateNewFolder
        [HttpPost, ActionName("CreateNewFolder")]
        [ValidateAntiForgeryToken]
        public IActionResult CreateNewFolder(UserFolder folder)
        {
            folder.ApplicationUserId = User.GetUserId();
            _context.UserFolder.Add(folder);
            _context.SaveChanges();

            var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "Photos", User.GetUserName(), folder.Name);
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            else
            {
                return View("FolderCreationFailure");
            }
            return RedirectToAction("Index");
        }

        //Get Photos/ViewFolderContents
        public IActionResult ViewFolderContents(int? id)
        {
            IEnumerable<Photo> photos = new List<Photo>();
            if (id != null)
            {
                photos = _context.Photo.Where(x => x.UserFolderId == id);
                if (photos.Count() == 0)
                {
                    NoPhotoInListViewModel folder = new NoPhotoInListViewModel();
                    folder.foldernum = (int)id;
                    folder.foldername = _context.UserFolder.Where(q => q.UserFolderID == id).Single().Name;
                    return View("ViewEmptyFolder", folder);
                }
            }
            
            return View("ViewNonEmptyFolder", photos);
        }

    }
}
