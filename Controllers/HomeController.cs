﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ReportManagement.Data;
using ReportManagement.Models;
using ReportManagement.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ReportManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            HomeVM homeVM = new HomeVM()
            {
                Complaints = _db.Complaint.Include(u => u.Category),
                Categories = _db.Category

            };
            return View(homeVM);
        }


        public IActionResult Details(int id)
        {

            // var product = _db.Product.Find(id);
            DetailsVM DetailsVM = new DetailsVM()
            {
                Complaint = _db.Complaint.Include(u => u.Category)
                .Where(u => u.Id == id).FirstOrDefault(),
                ExistsInCart = false
            };

            
            return View(DetailsVM);
        }

        public IActionResult DataTracker()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
