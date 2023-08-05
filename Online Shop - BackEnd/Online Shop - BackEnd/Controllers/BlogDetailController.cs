using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Online_Shop___BackEnd.Data;
using Online_Shop___BackEnd.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shop___BackEnd.Controllers
{
    public class BlogDetailController : Controller
    {
        private readonly AppDbContext _context;

        public BlogDetailController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? id)
        {
            Blog blog = await _context.Blogs
                .Where(m => !m.IsDeleted && m.Id == id)
                .Include(m => m.BlogImages)
                .FirstOrDefaultAsync();

            return View(blog);
        }
    }
}
