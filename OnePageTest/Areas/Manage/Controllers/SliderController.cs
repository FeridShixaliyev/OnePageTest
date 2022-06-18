using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnePageTest.DAL;
using OnePageTest.Extention;
using OnePageTest.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnePageTest.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class SliderController : Controller
    {
        private readonly AppDbContext _sql;
        private readonly IWebHostEnvironment _env;

        public SliderController(AppDbContext sql,IWebHostEnvironment env)
        {
            _sql = sql;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<Slider> sliders = await _sql.Sliders.ToListAsync();
            return View(sliders);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Slider slider)
        {
            if (!ModelState.IsValid) return View();
            if (slider == null) return NotFound();
            if (slider.ImageFile != null)
            {
                if (!slider.ImageFile.IsImage())
                {
                    ModelState.AddModelError("ImageFile","Sekilin formati duzgun deyil!");
                    return View();
                }
                if (!slider.ImageFile.IsSizeOk(5))
                {
                    ModelState.AddModelError("ImageFile","Sekilin olcusu 5 mb-dan cox ola bilmez!");
                    return View();
                }
                slider.Image = slider.ImageFile.SaveImage(_env.WebRootPath,"assets/css/image");
            }
            else
            {
                ModelState.AddModelError("ImageFile","Sekil yukleyin");
                return View();
            }
            await _sql.Sliders.AddAsync(slider);
            await _sql.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public IActionResult Edit(int? id)
        {
            Slider slider = _sql.Sliders.Find(id);
            if (slider == null) return NotFound();
            return View(slider);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Slider slider)
        {
            if (!ModelState.IsValid) return View();
            Slider sliderExist = await _sql.Sliders.FirstOrDefaultAsync(s=>s.Id==slider.Id);
            if (sliderExist == null) return NotFound();
            if (slider.ImageFile != null)
            {
                if (!slider.ImageFile.IsImage())
                {
                    ModelState.AddModelError("Photo", "Sekilin formati duzgun deyil!");
                    return View();
                }
                if (!slider.ImageFile.IsSizeOk(5))
                {
                    ModelState.AddModelError("Photo", "Sekilin olcusu 5 mb-dan cox ola bilmez!");
                    return View();
                }
                Helpers.Helper.DeleteImg(_env.WebRootPath,"assets/css/image", sliderExist.Image);
                sliderExist.Image = slider.ImageFile.SaveImage(_env.WebRootPath,"assets/css/image");
            }
            else
            {
                ModelState.AddModelError("Photo", "Sekil.yukleyin!");
                return View();
            }
            sliderExist.Title = slider.Title;
            await _sql.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(int? id)
        {
            Slider slider = await _sql.Sliders.FindAsync(id);
            if (slider == null) return NotFound();
            Helpers.Helper.DeleteImg(_env.WebRootPath,"assets/css/image",slider.Image);
            _sql.Sliders.Remove(slider);
            await _sql.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
