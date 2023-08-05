using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Online_Shop___BackEnd.Data;
using Online_Shop___BackEnd.Models;
using Online_Shop___BackEnd.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shop___BackEnd.Controllers
{
    public class ContactController : Controller
    {
        private readonly AppDbContext _context;

        public ContactController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            Contact contact = await _context.Contacts.Where(m => !m.IsDeleted).FirstOrDefaultAsync();
            ContactUs contactUs = await _context.ContactUs.Where(m => !m.IsDeleted).FirstOrDefaultAsync();

            ContactVM model = new ContactVM
            {
                Contact = contact,
                ContactUs = new ContactUs()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ContactUs contactUs)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    Contact contact = await _context.Contacts.Where(m => !m.IsDeleted).FirstOrDefaultAsync();

                    ContactVM model = new ContactVM
                    {
                        Contact = contact,
                    };

                    return View(model);
                }

                bool isExist = await _context.ContactUs.AnyAsync(m =>
                m.FullName.Trim() == contactUs.FullName.Trim() &&
                m.Email.Trim() == contactUs.Email.Trim() &&
                m.PhoneNumber.Trim() == contactUs.PhoneNumber.Trim() &&
                m.Message.Trim() == contactUs.Message.Trim());

                if (isExist)
                {
                    ModelState.AddModelError("FullName", "Subject already exist!");
                }

                await _context.ContactUs.AddAsync(contactUs);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                return View();
            }
        }
    }
}
