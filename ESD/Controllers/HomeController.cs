using ESD.Data;
using ESD.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Drawing.Printing;
using X.PagedList;

namespace ESD.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _context = context;
            _logger = logger;
        }

        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {


            ViewBag.TextSortParm = String.IsNullOrEmpty(sortOrder) ? "Text_desc" : "";
            ViewBag.ViewSSortParm = sortOrder == "ViewS" ? "ViewS_desc" : "ViewS";
            ViewBag.LikeSSortParm = sortOrder == "LikeS" ? "LikeS_desc" : "LikeS";
            List<Idea> students = _context.Ideas.ToList();


            switch (sortOrder)
            {
                case "Text_desc":
                    students = (List<Idea>)students.OrderByDescending(s => s.Text).ToList();
                    break;
                case "ViewS":
                    students = (List<Idea>)students.OrderBy(s => s.ViewS).ToList();
                    break;
                case "ViewS_desc":
                    students = (List<Idea>)students.OrderByDescending(s => s.ViewS).ToList();
                    break;
                case "LikeS":
                    students = (List<Idea>)students.OrderBy(s => s.LikeS).ToList();
                    break;
                case "LikeS_desc":
                    students = (List<Idea>)students.OrderByDescending(s => s.LikeS).ToList();
                    break;
                default:
                    //students = (List<Idea>)students.OrderBy(s => s.Text);
                    break;
            }
            ViewData["Ideas"] = students;
            int pageSize = 1;
            int pageNumber = (page ?? 1);
            return View(students.ToPagedList(pageNumber, pageSize));           
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