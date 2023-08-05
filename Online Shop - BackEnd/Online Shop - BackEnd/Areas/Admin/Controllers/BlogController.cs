using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Online_Shop___BackEnd.Data;
using Online_Shop___BackEnd.Helpers;
using Online_Shop___BackEnd.Models;
using Online_Shop___BackEnd.ViewModels.ProductViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;
using Online_Shop___BackEnd.ViewModels.BlogViewModels;

namespace Online_Shop___BackEnd.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BlogController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public BlogController(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<IActionResult> Index(int page = 1, int take = 3)
        {
            List<Blog> blogs = await _context.Blogs
                .Where(m => !m.IsDeleted)
                .Include(m => m.BlogImages)
                .OrderByDescending(m => m.Id)
                .Skip((page * take) - take)
                .Take(take)
                .ToListAsync();

            List<BlogListVM> mapDatas = GetMapDatas(blogs);

            int count = await GetPageCount(take);

            Paginate<BlogListVM> result = new Paginate<BlogListVM>(mapDatas, page, count);

            return View(result);
        }

        private async Task<int> GetPageCount(int take)
        {
            int blogCount = await _context.Blogs.Where(m => !m.IsDeleted).CountAsync();

            return (int)Math.Ceiling((decimal)blogCount / take);
        }

        private List<BlogListVM> GetMapDatas(List<Blog> blogs)
        {
            List<BlogListVM> blogList = new List<BlogListVM>();

            foreach (var blog in blogs)
            {
                BlogListVM newBlog = new BlogListVM
                {
                    Id = blog.Id,
                    Title = blog.Title,
                    Image = blog.BlogImages.FirstOrDefault()?.Name
                };

                blogList.Add(newBlog);
            }

            return blogList;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.categories = await GetCategoriesAsync();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlogCreateVM blog)
        {
            if (!ModelState.IsValid) return View(blog);

            foreach (var photo in blog.Photos)
            {
                if (!photo.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Photo", "Please, choose correct image type");
                    return View(blog);
                }

                if (!photo.CheckFileSize(1000))
                {
                    ModelState.AddModelError("Photo", "Please, choose correct image size");

                    ViewBag.categories = await GetCategoriesAsync();

                    return View(blog);
                }
            }

            List<BlogImage> images = new List<BlogImage>();

            foreach (var photo in blog.Photos)
            {
                string fileName = Guid.NewGuid().ToString() + "_" + photo.FileName;

                string path = Helper.GetFilePath(_environment.WebRootPath, "assets/images/blog", fileName);

                await Helper.SaveFile(path, photo);

                BlogImage image = new BlogImage
                {
                    Name = fileName
                };

                images.Add(image);
            }

            Blog newBlog = new Blog
            {
                BlogImages = images,
                Title = blog.Title,
                Description = blog.Description,
            };

            await _context.BlogImages.AddRangeAsync(images);
            await _context.Blogs.AddAsync(newBlog);
            await _context.SaveChangesAsync();

            BlogSubCategory blogCategory = new BlogSubCategory()
            {
                BlogId = newBlog.Id,
                SubCategoryId = blog.CategoryId
            };

            await _context.BlogSubCategories.AddAsync(blogCategory);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();

            Blog blog = await _context.Blogs
                .Where(m => !m.IsDeleted && m.Id == id)
                .Include(m => m.BlogImages)
                .Include(m => m.BlogSubCategories)
                .ThenInclude(m => m.SubCategory)
                .FirstOrDefaultAsync();

            BlogSubCategory category = await _context.BlogSubCategories
                .Where(m => m.BlogId == id && m.SubCategory.CategoryId == 6)
                .Include(m => m.SubCategory)
                .FirstOrDefaultAsync();

            if (blog == null) return NotFound();

            BlogDetailVM blogDetail = new BlogDetailVM
            {
                Id = blog.Id,
                Images = blog.BlogImages,
                Title = blog.Title,
                Description = blog.Description,
                Category = category.SubCategory.Name,
            };

            return View(blogDetail);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();

            Blog dbBlog = await GetByIdAsync((int)id);

            BlogSubCategory category = await _context.BlogSubCategories
                .Where(m => m.BlogId == id && m.SubCategory.CategoryId == 6)
                .FirstOrDefaultAsync();

            ViewBag.categories = await GetCategoriesAsync();

            BlogUpdateVM updatedBlog = new BlogUpdateVM()
            {
                Id = dbBlog.Id,
                Images = dbBlog.BlogImages,
                Title = dbBlog.Title,
                Description = dbBlog.Description,
                CategoryName = category.SubCategory.Name,
            };

            BlogSubCategory blogCategory = new BlogSubCategory()
            {
                BlogId = dbBlog.Id,
                SubCategoryId = updatedBlog.CategoryId
            };

            await _context.BlogSubCategories.AddAsync(blogCategory);

            return View(updatedBlog);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, BlogUpdateVM updatedBlog)
        {
            ViewBag.categories = await GetCategoriesAsync();

            if (!ModelState.IsValid) return View(updatedBlog);

            Blog dbBlog = await GetByIdAsync(id);

            if (updatedBlog.Photos != null)
            {
                foreach (var photo in updatedBlog.Photos)
                {
                    if (!photo.CheckFileType("image/"))
                    {
                        ModelState.AddModelError("Photo", "Please, choose correct image type");
                        return View(updatedBlog);
                    }

                    if (!photo.CheckFileSize(1000))
                    {
                        ModelState.AddModelError("Photo", "Please, choose correct image size");

                        ViewBag.categories = await GetCategoriesAsync();

                        return View(updatedBlog);
                    }
                }

                foreach (var image in dbBlog.BlogImages)
                {
                    string path = Helper.GetFilePath(_environment.WebRootPath, "assets/images/blog", image.Name);
                    Helper.DeleteFile(path);
                }

                List<BlogImage> images = new List<BlogImage>();

                foreach (var photo in updatedBlog.Photos)
                {
                    string fileName = Guid.NewGuid().ToString() + "_" + photo.FileName;

                    string path = Helper.GetFilePath(_environment.WebRootPath, "assets/images/blog", fileName);

                    await Helper.SaveFile(path, photo);

                    BlogImage image = new BlogImage
                    {
                        Name = fileName
                    };

                    images.Add(image);
                }

                ViewBag.categories = await GetCategoriesAsync();

                dbBlog.BlogImages = images;
            }

            dbBlog.Title = updatedBlog.Title;
            dbBlog.Description = updatedBlog.Description;

            BlogSubCategory blogCategory = new BlogSubCategory()
            {
                BlogId = updatedBlog.Id,
                SubCategoryId = updatedBlog.CategoryId
            };

            await _context.BlogSubCategories.AddAsync(blogCategory);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            Blog blog = await _context.Blogs
                .Where(m => !m.IsDeleted && m.Id == id)
                .Include(m => m.BlogImages)
                .FirstOrDefaultAsync();

            if (blog == null) return NotFound();

            foreach (var image in blog.BlogImages)
            {
                string path = Helper.GetFilePath(_environment.WebRootPath, "assets/images/blog", image.Name);
                Helper.DeleteFile(path);
            }

            blog.IsDeleted = true;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private async Task<Blog> GetByIdAsync(int id)
        {
            return await _context.Blogs
                .Where(m => !m.IsDeleted && m.Id == id)
                .Include(m => m.BlogImages)
                .Include(m => m.BlogSubCategories)
                .ThenInclude(m => m.SubCategory)
                .FirstOrDefaultAsync();
        }

        private async Task<List<SubCategory>> GetCategoriesAsync()
        {
            List<SubCategory> categories = await _context.SubCategories.Where(m => !m.IsDeleted && m.CategoryId == 6).ToListAsync();

            return categories;
        }
    }
}
