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
    public class UserServicesController : Controller
    {
        private readonly HomeServicesNewContext _context;

        public UserServicesController(HomeServicesNewContext context)
        {
            _context = context;
        }

        // GET: UserServices
        public async Task<IActionResult> Index()
        {
            var homeServicesNewContext = _context.UserServices.Include(u => u.Service).Include(u => u.User);
            return View(await homeServicesNewContext.ToListAsync());
        }

        // GET: UserServices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userService = await _context.UserServices
                .Include(u => u.Service)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.UserServiceId == id);
            if (userService == null)
            {
                return NotFound();
            }

            return View(userService);
        }

        // GET: UserServices/Create
        public IActionResult Create()
        {
            ViewData["ServiceId"] = new SelectList(_context.Services, "ServiceId", "ServiceId");
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId");
            return View();
        }

        // POST: UserServices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserServiceId,ServiceId,Date,Status,UserId")] UserService userService)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userService);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ServiceId"] = new SelectList(_context.Services, "ServiceId", "ServiceId", userService.ServiceId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", userService.UserId);
            return View(userService);
        }

        // GET: UserServices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userService = await _context.UserServices.FindAsync(id);
            if (userService == null)
            {
                return NotFound();
            }
            ViewData["ServiceId"] = new SelectList(_context.Services, "ServiceId", "ServiceId", userService.ServiceId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", userService.UserId);
            return View(userService);
        }

        // POST: UserServices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserServiceId,ServiceId,Date,Status,UserId")] UserService userService)
        {
            if (id != userService.UserServiceId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userService);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserServiceExists(userService.UserServiceId))
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
            ViewData["ServiceId"] = new SelectList(_context.Services, "ServiceId", "ServiceId", userService.ServiceId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", userService.UserId);
            return View(userService);
        }

        // GET: UserServices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userService = await _context.UserServices
                .Include(u => u.Service)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.UserServiceId == id);
            if (userService == null)
            {
                return NotFound();
            }

            return View(userService);
        }

        // POST: UserServices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userService = await _context.UserServices.FindAsync(id);
            _context.UserServices.Remove(userService);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserServiceExists(int id)
        {
            return _context.UserServices.Any(e => e.UserServiceId == id);
        }
    }
}
