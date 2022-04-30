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
    public class UsersController : Controller
    {
        private readonly HomeServicesNewContext _context;
        private IWebHostEnvironment _hostEnvironment;


        public UsersController(HomeServicesNewContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }


        // GET: Users
        public async Task<IActionResult> Index()
        {
            var homeServicesNewContext = _context.Users.Include(u => u.Login);
            return View(await homeServicesNewContext.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.Login)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            ViewData["LoginId"] = new SelectList(_context.Logins, "LoginId", "LoginId");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,FirstName,LastName,Gender,Email,PhoneNumber,Adress,City,BirthDate,LoginId,UserImage,ImageFile")] User user)
        {
            if (user.LoginId != null || ModelState.IsValid)
            {
                if (user.ImageFile != null)
                {
                    string wRootPath = _hostEnvironment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + "_" + user.ImageFile.FileName;
                    string path = Path.Combine(wRootPath + "/Image/", fileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {

                        await user.ImageFile.CopyToAsync(fileStream);
                    }
                    user.UserImage = fileName;
                    _context.Add(user); //add object to table
                  await _context.SaveChangesAsync(); //saveoepration
return RedirectToAction(nameof(Index)); // return to Index
}
                else
                {
                }
           
            }
            ViewData["LoginId"] = new SelectList(_context.Logins, "LoginId", "LoginId", user.LoginId);
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ViewData["LoginId"] = new SelectList(_context.Logins, "LoginId", "LoginId", user.LoginId);
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,FirstName,LastName,Gender,Email,PhoneNumber,Adress,City,BirthDate,LoginId,UserImage")] User user)
        {
            if (id != user.UserId)
            {
                return NotFound();
            }

            if (user.LoginId != null || ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserId))
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
            ViewData["LoginId"] = new SelectList(_context.Logins, "UserName", "UsreName", user.LoginId);
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.Login)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
    }
}
