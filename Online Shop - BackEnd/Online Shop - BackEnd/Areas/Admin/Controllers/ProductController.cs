using Microsoft.AspNetCore.Mvc;
using Online_Shop___BackEnd.Data;
using Online_Shop___BackEnd.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Online_Shop___BackEnd.ViewModels.ProductViewModels;
using Online_Shop___BackEnd.Helpers;
using Microsoft.AspNetCore.Hosting;

namespace Online_Shop___BackEnd.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public ProductController(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<IActionResult> Index(int page = 1, int take = 3)
        {
            List<Product> products = await _context.Products
                .Where(m => !m.IsDeleted)
                .Include(m => m.ProductImages)
                .OrderByDescending(m => m.Id)
                .Skip((page * take) - take)
                .Take(take)
                .ToListAsync();

            List<ProductListVM> mapDatas = GetMapDatas(products);

            int count = await GetPageCount(take);

            Paginate<ProductListVM> result = new Paginate<ProductListVM>(mapDatas, page, count);

            return View(result);
        }

        private async Task<int> GetPageCount(int take)
        {
            int productCount = await _context.Products.Where(m => !m.IsDeleted).CountAsync();

            return (int)Math.Ceiling((decimal)productCount / take);
        }

        private List<ProductListVM> GetMapDatas(List<Product> products)
        {
            List<ProductListVM> productList = new List<ProductListVM>();

            foreach (var product in products)
            {
                ProductListVM newProduct = new ProductListVM
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    MainImage = product.ProductImages.Where(m => m.IsMain).FirstOrDefault()?.Name
                };

                productList.Add(newProduct);
            }

            return productList;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.categories = await GetCategoriesAsync();
            ViewBag.brands = await GetBrandsAsync();
            var data = await GetSizeAsync();

            ProductCreateVM product = new ProductCreateVM()
            {
                Size = data
            };

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateVM product)
        {
            if (!ModelState.IsValid) return View(product);

            foreach (var photo in product.Photos)
            {
                if (!photo.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Photo", "Please, choose correct image type");
                    return View(product);
                }

                if (!photo.CheckFileSize(1000))
                {
                    ModelState.AddModelError("Photo", "Please, choose correct image size");

                    ViewBag.categories = await GetCategoriesAsync();
                    ViewBag.brands = await GetBrandsAsync();
                    var data = await GetSizeAsync();

                    return View(product);
                }
            }

            List<ProductImage> images = new List<ProductImage>();

            foreach (var photo in product.Photos)
            {
                string fileName = Guid.NewGuid().ToString() + "_" + photo.FileName;

                string path = Helper.GetFilePath(_environment.WebRootPath, "assets/images/home/products", fileName);

                await Helper.SaveFile(path, photo);

                ProductImage image = new ProductImage
                {
                    Name = fileName
                };

                images.Add(image);
            }

            images.FirstOrDefault().IsMain = true;

            Product newProduct = new Product
            {
                ProductImages = images,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price
            };

            await _context.ProductImages.AddRangeAsync(images);
            await _context.Products.AddAsync(newProduct);
            await _context.SaveChangesAsync();

            foreach (var size in product.Size.Where(m => m.IsSelected && m.CategoryId == 1))
            {
                ProductSubCategory sizes = new ProductSubCategory
                {
                    ProductId = newProduct.Id,
                    SubCategoryId = size.Id
                };

                await _context.ProductSubCategories.AddAsync(sizes);
            }

            ProductSubCategory productCategory = new ProductSubCategory()
            {
                ProductId = newProduct.Id,
                SubCategoryId = product.CategoryId
            };
            await _context.ProductSubCategories.AddAsync(productCategory);

            ProductSubCategory productBrand = new ProductSubCategory()
            {
                ProductId = newProduct.Id,
                SubCategoryId = product.BrandId
            };
            await _context.ProductSubCategories.AddAsync(productBrand);

            //ProductSubCategory productSize = new ProductSubCategory()
            //{
            //    ProductId = newProduct.Id,
            //    SubCategoryId = product.SizeId
            //};

            //await _context.ProductSubCategories.AddAsync(productSize);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();

            Product product = await GetByIdAsync((int)id);

            IEnumerable<ProductSubCategory> size = await _context.ProductSubCategories
                .Where(m => m.ProductId == id && m.SubCategory.CategoryId == 1)
                .ToListAsync();

            ProductSubCategory brand = await _context.ProductSubCategories
                .Where(m => m.ProductId == id && m.SubCategory.CategoryId == 2)
                .Include(m => m.SubCategory)
                .FirstOrDefaultAsync();

            ProductSubCategory category = await _context.ProductSubCategories
                .Where(m => m.ProductId == id && m.SubCategory.CategoryId == 5)
                .Include(m => m.SubCategory)
                .FirstOrDefaultAsync();

            if (product == null) return NotFound();

            ProductDetailVM productDetail = new ProductDetailVM
            {
                Id = product.Id,
                Images = product.ProductImages,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Brand = brand.SubCategory.Name,
                Category = category.SubCategory.Name,
                Size = size
            };

            return View(productDetail);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();

            Product dbProduct = await GetByIdAsync((int)id);

            ProductSubCategory category = await _context.ProductSubCategories
                .Where(m => m.ProductId == id && m.SubCategory.CategoryId == 5)
                .FirstOrDefaultAsync();

            ProductSubCategory brand = await _context.ProductSubCategories
                .Where(m => m.ProductId == id && m.SubCategory.CategoryId == 2)
                .FirstOrDefaultAsync();

            IEnumerable<ProductSubCategory> size = await _context.ProductSubCategories
                .Where(m => m.ProductId == id && m.SubCategory.CategoryId == 1)
                .ToListAsync();

            decimal convertedPrice = decimal.Parse(dbProduct.Price.ToString("##0.00"));

            ViewBag.categories = await GetCategoriesAsync();
            ViewBag.brands = await GetBrandsAsync();
            var data = await GetSizeAsync();

            ProductUpdateVM updatedProduct = new ProductUpdateVM()
            {
                Id = dbProduct.Id,
                Images = dbProduct.ProductImages,
                Name = dbProduct.Name,
                Description = dbProduct.Description,
                Price = convertedPrice,
                CategoryName = category.SubCategory.Name,
                BrandName = brand.SubCategory.Name,
                SizeName = size,
                Size = data
            };

            ProductSubCategory productCategory = new ProductSubCategory()
            {
                ProductId = dbProduct.Id,
                SubCategoryId = updatedProduct.CategoryId
            };

            await _context.ProductSubCategories.AddAsync(productCategory);

            ProductSubCategory productBrand = new ProductSubCategory()
            {
                ProductId = dbProduct.Id,
                SubCategoryId = updatedProduct.BrandId
            };

            await _context.ProductSubCategories.AddAsync(productBrand);

            return View(updatedProduct);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, ProductUpdateVM updatedProduct)
        {
            ViewBag.categories = await GetCategoriesAsync();
            ViewBag.brands = await GetBrandsAsync();
            var data = await GetSizeAsync();

            ProductUpdateVM product = new ProductUpdateVM()
            {
                Size = data
            };

            if (!ModelState.IsValid) return View(updatedProduct);

            Product dbProduct = await GetByIdAsync(id);

            if (updatedProduct.Photos != null)
            {
                foreach (var photo in updatedProduct.Photos)
                {
                    if (!photo.CheckFileType("image/"))
                    {
                        ModelState.AddModelError("Photo", "Please, choose correct image type");
                        return View(updatedProduct);
                    }

                    if (!photo.CheckFileSize(1000))
                    {
                        ModelState.AddModelError("Photo", "Please, choose correct image size");

                        ViewBag.categories = await GetCategoriesAsync();
                        ViewBag.brands = await GetBrandsAsync();
                        var data2 = await GetSizeAsync();

                        return View(updatedProduct);
                    }
                }

                foreach (var image in dbProduct.ProductImages)
                {
                    string path = Helper.GetFilePath(_environment.WebRootPath, "assets/images/home/products", image.Name);
                    Helper.DeleteFile(path);
                }

                List<ProductImage> images = new List<ProductImage>();

                foreach (var photo in updatedProduct.Photos)
                {
                    string fileName = Guid.NewGuid().ToString() + "_" + photo.FileName;

                    string path = Helper.GetFilePath(_environment.WebRootPath, "assets/images/home/products", fileName);

                    await Helper.SaveFile(path, photo);

                    ProductImage image = new ProductImage
                    {
                        Name = fileName
                    };

                    images.Add(image);
                }

                ViewBag.categories = await GetCategoriesAsync();
                ViewBag.brands = await GetBrandsAsync();
                var data1 = await GetSizeAsync(); ;

                dbProduct.ProductImages = images;
            }

            dbProduct.Name = updatedProduct.Name;
            dbProduct.Description = updatedProduct.Description;
            dbProduct.Price = updatedProduct.Price;

            ProductSubCategory productCategory = new ProductSubCategory()
            {
                ProductId = updatedProduct.Id,
                SubCategoryId = updatedProduct.CategoryId
            };

            await _context.ProductSubCategories.AddAsync(productCategory);

            ProductSubCategory productBrand = new ProductSubCategory()
            {
                ProductId = updatedProduct.Id,
                SubCategoryId = updatedProduct.BrandId
            };

            await _context.ProductSubCategories.AddAsync(productBrand);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            Product product = await _context.Products
                .Where(m => !m.IsDeleted && m.Id == id)
                .Include(m => m.ProductImages)
                .FirstOrDefaultAsync();

            if (product == null) return NotFound();

            foreach (var image in product.ProductImages)
            {
                string path = Helper.GetFilePath(_environment.WebRootPath, "assets/images/home/products", image.Name);
                Helper.DeleteFile(path);
                image.IsDeleted = true;
            }

            product.IsDeleted = true;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private async Task<Product> GetByIdAsync(int id)
        {
            return await _context.Products
                .Where(m => !m.IsDeleted && m.Id == id)
                .Include(m => m.ProductImages)
                .Include(m => m.ProductSubCategories)
                .ThenInclude(m => m.SubCategory)
                .FirstOrDefaultAsync();
        }

        private async Task<List<SubCategory>> GetCategoriesAsync()
        {
            List<SubCategory> categories = await _context.SubCategories.Where(m => !m.IsDeleted && m.CategoryId == 5).ToListAsync();

            return categories;
        }

        private async Task<List<SubCategory>> GetBrandsAsync()
        {
            List<SubCategory> brands = await _context.SubCategories.Where(m => !m.IsDeleted && m.CategoryId == 2).ToListAsync();

            return brands;
        }

        private async Task<List<SubCategory>> GetSizeAsync()
        {
            List<SubCategory> sizes = await _context.SubCategories.Where(m => !m.IsDeleted && m.CategoryId == 1).ToListAsync();

            return sizes;
        }
    }
}
