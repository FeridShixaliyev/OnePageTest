using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnePageTest.DAL;

namespace OnePageTest.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles="Admin")]
    public class DashboardController : Controller
    {
        private readonly AppDbContext _sql;

        public DashboardController(AppDbContext sql)
        {
            _sql = sql;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
