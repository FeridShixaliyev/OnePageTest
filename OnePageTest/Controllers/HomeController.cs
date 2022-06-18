using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnePageTest.DAL;
using OnePageTest.ViewModels;
using System.Threading.Tasks;

namespace OnePageTest.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _sql;

        public HomeController(AppDbContext sql)
        {
            _sql = sql;
        }

        public async Task<IActionResult> Index()
        {
            SliderCartVM sliderCartVM = new SliderCartVM
            {
                Sliders = await _sql.Sliders.ToListAsync(),
                Carts =await _sql.Carts.ToListAsync()
            };
            return View(sliderCartVM);
        }
    }
}
