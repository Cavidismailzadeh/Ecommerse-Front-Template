using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Online_Shop___BackEnd.Data;
using Online_Shop___BackEnd.Helpers;
using Online_Shop___BackEnd.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shop___BackEnd.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeCategoryController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public HomeCategoryController(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<HomeCategory> categories = await _context.HomeCategories.Where(m => !m.IsDeleted).ToListAsync();

            return View(categories);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HomeCategory category)
        {
            if (!ModelState.IsValid) return View();

            if (!category.Photo.CheckFileType("image/"))
            {
                ModelState.AddModelError("Photo", "Please, choose correct image type");
                return View();
            }

            if (!category.Photo.CheckFileSize(200))
            {
                ModelState.AddModelError("Photo", "Please, choose correct image size");
                return View();
            }

            string fileName = Guid.NewGuid().ToString() + "_" + category.Photo.FileName;

            string path = Helper.GetFilePath(_environment.WebRootPath, "assets/images/home", fileName);

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await category.Photo.CopyToAsync(stream);
            }

            category.Image = fileName;

            await _context.HomeCategories.AddAsync(category);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();

            HomeCategory category = await _context.HomeCategories.FindAsync(id);

            if (category == null) return NotFound();

            return View(category);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            try
            {
                if (id is null) return BadRequest();

                HomeCategory category = await _context.HomeCategories.FirstOrDefaultAsync(m => m.Id == id);

                if (category is null) return NotFound();

                return View(category);

            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, HomeCategory category)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(category);
                }

                if (!category.Photo.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Photo", "Please choose correct image type");
                    return View();
                }

                string fileName = Guid.NewGuid().ToString() + "_" + category.Photo.FileName;

                HomeCategory dbcategory = await _context.HomeCategories.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);

                if (dbcategory is null) return NotFound();

                if (dbcategory.Photo == category.Photo)
                {
                    return RedirectToAction(nameof(Index));
                }

                string path = Helper.GetFilePath(_environment.WebRootPath, "assets/images/home", fileName);

                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    await category.Photo.CopyToAsync(stream);
                }

                category.Image = fileName;

                _context.HomeCategories.Update(category);

                await _context.SaveChangesAsync();

                string dbPath = Helper.GetFilePath(_environment.WebRootPath, "assets/images/home", dbcategory.Image);

                Helper.DeleteFile(dbPath);

                return RedirectToAction(nameof(Index));

            }
            catch (Exception exception)
            {
                ViewBag.Message = exception.Message;
                return View();
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            HomeCategory category = await GetByIdAsync(id);

            if (category == null) return NotFound();

            string path = Helper.GetFilePath(_environment.WebRootPath, "assets/images/home", category.Image);

            Helper.DeleteFile(path);

            _context.HomeCategories.Remove(category);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<HomeCategory> GetByIdAsync(int id)
        {
            return await _context.HomeCategories.FindAsync(id);
        }
    }
}
