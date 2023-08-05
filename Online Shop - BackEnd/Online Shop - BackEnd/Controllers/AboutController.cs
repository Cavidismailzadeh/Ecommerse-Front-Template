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
    public class AboutController : Controller
    {
        private readonly AppDbContext _context;

        public AboutController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            PageHeader pageHeader = await _context.PageHeaders.Where(m => !m.IsDeleted).FirstOrDefaultAsync();
            IEnumerable<About> abouts = await _context.Abouts.Where(m => !m.IsDeleted).ToListAsync();
            IEnumerable<Brand> brands = await _context.Brands.Where(m => !m.IsDeleted).ToListAsync();

            AboutVM model = new AboutVM
            {
                PageHeader= pageHeader,
                Abouts= abouts,
                Brands= brands
            };

            return View(model);
        }
    }
}
