#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication29.Models;

namespace WebApplication29.Controllers
{
    public class ServicesController : Controller
    {
    
        private readonly HomeServicesNewContext _context;
        private IWebHostEnvironment _hostEnvironment;


        public ServicesController(HomeServicesNewContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }
        // GET: Services
        public async Task<IActionResult> Index()
        {
            var homeServicesNewContext = _context.Services.Include(s => s.Category).Include(s => s.User);
            return View(await homeServicesNewContext.ToListAsync());
        }

        // GET: Services/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _context.Services
                .Include(s => s.Category)
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.ServiceId == id);
            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }

        // GET: Services/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId");
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId");
            return View();
        }

        // POST: Services/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ServiceId,ServiceName,Price,CategoryId,UserId,Description,ImageFile")] Service service)
        {
            if ((service.CategoryId != null && service.UserId != null) || ModelState.IsValid)
            {
                if (service.ImageFile != null)
                {
                    string wRootPath = _hostEnvironment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + "_" + service.ImageFile.FileName;
                    string path = Path.Combine(wRootPath + "/Image/", fileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {

                        await service.ImageFile.CopyToAsync(fileStream);
                    }
                    service.UserImage = fileName;
                    _context.Add(service);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", service.CategoryId);
                ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", service.UserId);
                return View(service);
            
        }
        // GET: Services/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _context.Services.FindAsync(id);
            if (service == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", service.CategoryId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", service.UserId);
            return View(service);
        }

        // POST: Services/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ServiceId,ServiceName,Price,CategoryId,UserId,Description,ImageFile")] Service service)
        {
            if (id != service.ServiceId)
            {
                return NotFound();
            }

            if ((service.CategoryId != null && service.UserId != null) || ModelState.IsValid)
            {
                try
                {
                    if (service.ImageFile != null)
                    {
                        string wRootPath = _hostEnvironment.WebRootPath;
                        string fileName = Guid.NewGuid().ToString() + "_" + service.ImageFile.FileName;
                        string path = Path.Combine(wRootPath + "/Image/", fileName);
                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {

                            await service.ImageFile.CopyToAsync(fileStream);
                        }
                        service.UserImage = fileName;

                        _context.Update(service);
                        await _context.SaveChangesAsync();
                    }
                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServiceExists(service.ServiceId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", service.CategoryId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", service.UserId);
            return View(service);
        }

        // GET: Services/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _context.Services
                .Include(s => s.Category)
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.ServiceId == id);
            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }

        // POST: Services/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var service = await _context.Services.FindAsync(id);
            _context.Services.Remove(service);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ServiceExists(int id)
        {
            return _context.Services.Any(e => e.ServiceId == id);
        }
    }
}
