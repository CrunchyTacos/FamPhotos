using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using FamPhotos.Models;

namespace FamPhotos.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "FamPhotos is a website for the friends and family of Alicia and David Deering to share, organize and document pictures";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "If you have any ideas or suggestions, call and let me know!";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
