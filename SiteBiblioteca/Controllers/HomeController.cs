﻿using Microsoft.AspNetCore.Mvc;

namespace SiteBiblioteca.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult EditarPerfil()
        {
            return View();
        }
    }
}
