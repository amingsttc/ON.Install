﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ON.Authentication;
using ON.Content.SimpleCMS.Web.Models;
using ON.Content.SimpleCMS.Web.Services;

namespace ON.Content.SimpleCMS.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly ContentService contentService;

        public HomeController(ILogger<HomeController> logger, ContentService contentService)
        {
            this.logger = logger;
            this.contentService = contentService;
        }

        public async Task<IActionResult> Index()
        {
            return View("Home", new HomeViewModel((await contentService.GetAll()).Records.Where(r => r.Public.PublishedOnUTC != null)));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}