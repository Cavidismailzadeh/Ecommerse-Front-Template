using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Online_Shop___BackEnd.Data;
using Online_Shop___BackEnd.Helpers;
using Online_Shop___BackEnd.Models;
using Online_Shop___BackEnd.ViewModels.CategoryViewModels;
using Online_Shop___BackEnd.ViewModels.SubCategoryViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shop___BackEnd.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SubCategoryController : Controller
    {
        private readonly AppDbContext _context;

        public SubCategoryController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int page = 1, int take = 3)
        {
            List<SubCategory> categories = await _context.SubCategories
                .Where(m => !m.IsDeleted)
                .Include(m => m.Category)
                .OrderByDescending(m => m.Id)
                .Skip((page * take) - take)
                .Take(take)
                .ToListAsync();

            List<SubCategoryListVM> mapDatas = GetMapDatas(categories);

            int count = await GetPageCount(take);

            Paginate<SubCategoryListVM> result = new Paginate<SubCategoryListVM>(mapDatas, page, count);

            return View(result);
        }

        private async Task<int> GetPageCount(int take)
        {
            int categoryCount = await _context.SubCategories.Where(m => !m.IsDeleted).CountAsync();

            return (int)Math.Ceiling((decimal)categoryCount / take);
        }

        private List<SubCategoryListVM> GetMapDatas(List<SubCategory> categories)
        {
            List<SubCategoryListVM> categoryList = new List<SubCategoryListVM>();

            foreach (var category in categories)
            {
                SubCategoryListVM newCategory = new SubCategoryListVM
                {
                    Id = category.Id,
                    Category = category.Category.Name,
                    SubCategory = category.Name
                };

                categoryList.Add(newCategory);
            }

            return categoryList;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.categories = await GetCategoriesAsync();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SubCategoryCreateVM category)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(category);
                }

                bool isExist = await _context.SubCategories.AnyAsync(m => m.Name.Trim() == category.SubCategory.Trim());

                if (isExist)
                {
                    ModelState.AddModelError("SubCategory", "Sub category already exist!");
                    return View();
                }

                SubCategory newCategory = new SubCategory
                {
                    Name = category.SubCategory,
                    CategoryId = category.CategoryId
                };

                await _context.SubCategories.AddAsync(newCategory);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception exception)
            {
                ViewBag.Message = exception.Message;
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();

            SubCategory category = await _context.SubCategories
                .Where(m => !m.IsDeleted && m.Id == id)
                .Include(m => m.Category)
                .FirstOrDefaultAsync();

            if (category == null) return NotFound();

            SubCategoryListVM categoryDetail = new SubCategoryListVM
            {
                Id = category.Id,
                SubCategory = category.Name,
                Category = category.Category.Name
            };

            return View(categoryDetail);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            if (id is null) return BadRequest();

            ViewBag.categories = await GetCategoriesAsync();

            SubCategory dbSubCategory = await GetByIdAsync((int)id);

            SubCategoryUpdateVM updateSubCategory = new SubCategoryUpdateVM
            {
                Id = dbSubCategory.Id,
                SubCategory = dbSubCategory.Name,
                CategoryId = dbSubCategory.CategoryId
            };

            return View(updateSubCategory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, SubCategoryUpdateVM updateSubCategory)
        {
            ViewBag.categories = await GetCategoriesAsync();

            if (!ModelState.IsValid) return View(updateSubCategory);

            SubCategory dbSubCategory = await GetByIdAsync(id);

            dbSubCategory.Name = updateSubCategory.SubCategory;
            dbSubCategory.CategoryId = updateSubCategory.CategoryId;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            SubCategory category = await _context.SubCategories
                .Where(m => !m.IsDeleted && m.Id == id)
                .FirstOrDefaultAsync();

            if (category == null)
            {
                return NotFound();
            }

            category.IsDeleted = true;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private async Task<SelectList> GetCategoriesAsync()
        {
            IEnumerable<Category> categories = await _context.Categories.Where(m => !m.IsDeleted).ToListAsync();

            return new SelectList(categories, "Id", "Name");
        }

        private async Task<SubCategory> GetByIdAsync(int id)
        {
            return await _context.SubCategories
                .Where(m => !m.IsDeleted && m.Id == id)
                .Include(m => m.Category)
                .FirstOrDefaultAsync();
        }
    }
}
