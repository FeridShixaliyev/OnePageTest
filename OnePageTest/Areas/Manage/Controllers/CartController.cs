using Microsoft.AspNetCore.Mvc;
using OnePageTest.DAL;
using OnePageTest.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnePageTest.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class CartController : Controller
    {
        private readonly AppDbContext _sql;

        public CartController(AppDbContext sql)
        {
            _sql = sql;
        }
        public IActionResult Index()
        {
            List<Carts> carts = _sql.Carts.ToList();
            return View(carts);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Carts carts)
        {
            if (!ModelState.IsValid) return View();
            if (carts == null) return NotFound();
            await _sql.Carts.AddAsync(carts);
            await _sql.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public IActionResult Edit(int? id)
        {
            Carts carts = _sql.Carts.Find(id);
            return View(carts);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Carts carts)
        {
            if (!ModelState.IsValid) return View();
            Carts cartsExist = _sql.Carts.FirstOrDefault(c=>c.Id==carts.Id);
            if (cartsExist == null) return NotFound();
            cartsExist.Text = carts.Text;
            cartsExist.Title = carts.Title;
            cartsExist.Logo = carts.Logo;
            _sql.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();
            Carts carts = _sql.Carts.Find(id);
            if (carts == null) return NotFound();
            _sql.Carts.Remove(carts);
            _sql.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
