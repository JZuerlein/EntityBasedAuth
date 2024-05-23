using EntityBasedAuth.Domain;
using EntityBasedAuth.MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EntityBasedAuth.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly ILogger<HomeController> _logger;

        private const int creatorId = 1;
        private const int dickId = 2;
        private const int sallyId = 3;
        private const int janeId = 4;
        private const int humanResourceId = 5;

        public HomeController(IAuthorizationService authorizationService,
                              ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var list = new List<EmployeeReview>();
            var dick = new EmployeeReview(creatorId, dickId, "Dick", 2);
            var sally = new EmployeeReview(creatorId, sallyId, "Sally", 5);
            var jane = new EmployeeReview(creatorId, janeId, "Jane", 4);

            list.Add(dick);
            list.Add(sally);
            list.Add(jane);

            return View(list);
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
