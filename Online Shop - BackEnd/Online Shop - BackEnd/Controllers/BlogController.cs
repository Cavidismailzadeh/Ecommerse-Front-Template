using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Online_Shop___BackEnd.Data;
using Online_Shop___BackEnd.Models;
using Online_Shop___BackEnd.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shop___BackEnd.Controllers
{
    public class BlogController : Controller
    {
        private readonly AppDbContext _context;

        public BlogController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {

            IEnumerable<PageHeader> pageHeader = await _context.PageHeaders
                .Where(m => !m.IsDeleted)
                .ToListAsync();
            IEnumerable<Blog> blogs = await _context.Blogs
                .Where(m => !m.IsDeleted)
                .Include(m => m.BlogImages)
                .Include(m => m.BlogSubCategories)
                .ThenInclude(m => m.SubCategory)
                .ToListAsync();
            IEnumerable<SubCategory> subCategories = await _context.SubCategories
                .Where(m => !m.IsDeleted)
                .ToListAsync();
            IEnumerable<BlogSubCategory> blogSubCategories = await _context.BlogSubCategories
                .Where(m => !m.IsDeleted)
                .Include(m => m.SubCategory)
                .Include(m => m.Blog)
                .ThenInclude(m => m.BlogImages)
                .ToListAsync();

            BlogVM model = new BlogVM
            {
                Blogs = blogs,
                SubCategories = subCategories,
                BlogSubCategories = blogSubCategories,
                PageHeader = pageHeader
            };

            return View(model);
        }
        public IActionResult Search(string search)
        {
            List<Blog> searchBlog = _context.Blogs.Include(m => m.BlogImages).Include(m => m.BlogSubCategories).ThenInclude(m => m.SubCategory).Where(m => m.Title.ToLower().Contains(search.ToLower()) && !m.IsDeleted).ToList();
            BlogVM model = new BlogVM
            {
                Blogs = searchBlog,
            };
            return PartialView("_SearchBlogPartial", model);
        }

    }
}
