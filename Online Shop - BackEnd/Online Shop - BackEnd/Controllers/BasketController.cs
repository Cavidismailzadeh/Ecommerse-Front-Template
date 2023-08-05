using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Online_Shop___BackEnd.Data;
using Online_Shop___BackEnd.Models;
using Online_Shop___BackEnd.ViewModels.BasketViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shop___BackEnd.Controllers
{
    public class BasketController : Controller
    {
        private readonly AppDbContext _context;

        public BasketController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<BasketVM> basketItems = JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies["basket"]);
            List<BasketDetailVM> basketDetail = new List<BasketDetailVM>();
            ViewBag.sizes = await GetSizeAsync();

            foreach (var item in basketItems)
            {
                Product product = await _context.Products
                    .Where(m => m.Id == item.Id && m.IsDeleted == false)
                    .Include(m => m.ProductImages)
                    .Include(m => m.ProductSubCategories)
                    .ThenInclude(m => m.SubCategory)
                    .FirstOrDefaultAsync();

                ProductSubCategory brand = await _context.ProductSubCategories
                .Where(m => m.ProductId == item.Id && m.SubCategory.CategoryId == 2)
                .Include(m => m.SubCategory)
                .FirstOrDefaultAsync();

                ProductSubCategory category = await _context.ProductSubCategories
                    .Where(m => m.ProductId == item.Id && m.SubCategory.CategoryId == 5)
                    .Include(m => m.SubCategory)
                    .FirstOrDefaultAsync();

                BasketDetailVM newBasket = new BasketDetailVM
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Image = product.ProductImages.Where(m => m.IsMain).FirstOrDefault()?.Name,
                    Price = product.Price,
                    Brand = brand.SubCategory.Name,
                    Category = category.SubCategory.Name,
                    Count = item.Count,
                    Total = product.Price * item.Count
                };

                basketDetail.Add(newBasket);

            }

            return View(basketDetail);
        }

        private async Task<List<SubCategory>> GetSizeAsync()
        {
            List<SubCategory> sizes = await _context.SubCategories.Where(m => !m.IsDeleted && m.CategoryId == 1).ToListAsync();

            return sizes;
        }
    }
}
