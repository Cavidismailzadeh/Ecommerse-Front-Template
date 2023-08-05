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
    public class HomeBannerController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;
         
        public HomeBannerController(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Banner> banners = await _context.Banners.Where(m => !m.IsDeleted).ToListAsync();

            return View(banners);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Banner banner)
        {
            if (!ModelState.IsValid) return View();

            if (!banner.Photo.CheckFileType("image/"))
            {
                ModelState.AddModelError("Photo", "Please, choose correct image type");
                return View();
            }

            if (!banner.Photo.CheckFileSize(200))
            {
                ModelState.AddModelError("Photo", "Please, choose correct image size");
                return View();
            }

            string fileName = Guid.NewGuid().ToString() + "_" + banner.Photo.FileName;

            string path = Helper.GetFilePath(_environment.WebRootPath, "assets/images/home", fileName);

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await banner.Photo.CopyToAsync(stream);
            }

            banner.Image = fileName;

            await _context.Banners.AddAsync(banner);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();

            Banner banner = await _context.Banners.FindAsync(id);

            if (banner == null) return NotFound();

            return View(banner);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            try
            {
                if (id is null) return BadRequest();

                Banner banner = await _context.Banners.FirstOrDefaultAsync(m => m.Id == id);

                if (banner is null) return NotFound();

                return View(banner);

            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, Banner banner)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(banner);
                }

                if (!banner.Photo.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Photo", "Please choose correct image type");
                    return View();
                }

                string fileName = Guid.NewGuid().ToString() + "_" + banner.Photo.FileName;

                Banner dbBanner = await _context.Banners.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);

                if (dbBanner is null) return NotFound();

                if (dbBanner.Photo == banner.Photo)
                {
                    return RedirectToAction(nameof(Index));
                }

                string path = Helper.GetFilePath(_environment.WebRootPath, "assets/images/home", fileName);

                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    await banner.Photo.CopyToAsync(stream);
                }

                banner.Image = fileName;

                _context.Banners.Update(banner);

                await _context.SaveChangesAsync();

                string dbPath = Helper.GetFilePath(_environment.WebRootPath, "assets/images/home", dbBanner.Image);

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
            Banner banner = await GetByIdAsync(id);

            if (banner == null) return NotFound();

            string path = Helper.GetFilePath(_environment.WebRootPath, "assets/images/home", banner.Image);

            Helper.DeleteFile(path);

            _context.Banners.Remove(banner);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<Banner> GetByIdAsync(int id)
        {
            return await _context.Banners.FindAsync(id);
        }
    }
}
