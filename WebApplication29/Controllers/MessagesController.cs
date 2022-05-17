#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using WebApplication29.Models;

namespace WebApplication29.Controllers
{
    public class MessagesController : Controller
    {
        private readonly HomeServicesNewContext _context;
     
        private readonly IToastNotification _toastNotification;
        public MessagesController(HomeServicesNewContext context, IToastNotification toastNotification)
        {
            _context = context;
         
            _toastNotification = toastNotification;

        }
        HomeServicesNewContext db = new HomeServicesNewContext();

        // GET: Messages
        public async Task<IActionResult> Index()
        {
            var homeServicesNewContext = _context.Messages.Include(m => m.User);
            return View(await homeServicesNewContext.ToListAsync());
        }

        // GET: Messages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var message = await _context.Messages
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.MessageId == id);
            if (message == null)
            {
                return NotFound();
            }

            return View(message);
        }

        // GET: Messages/Create
        public IActionResult Create(int id)
        {
            List<User> users = db.Users.ToList();
            int employeeid = id;
            ViewBag.EmployeeId = employeeid;
            ViewBag.Employeename = users.Where(x => x.UserId == id).Select(x => x.FirstName).FirstOrDefault();
            return View();
        }



        public IActionResult Create1(string MessageText, string MessageTitle, DateTime MessageDate, Message message, int id)
        {
            message.UserId = id;
            message.SenderName = "Admin";
            message.MessageText = MessageText;
            message.MessageTitle = MessageTitle;
            message.MessageDate = MessageDate;
      
            _context.Add(message);
            _context.SaveChangesAsync();
            _toastNotification.AddSuccessToastMessage("Message sent sucssfully");

            return RedirectToAction("DisplayEmployee", "Admin");
        }
       
       
        // GET: Messages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var message = await _context.Messages.FindAsync(id);
            if (message == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", message.UserId);
            return View(message);
        }

        // POST: Messages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MessageId,MessageText,SenderName,MessageTitle,MessageDate,UserId")] Message message)
        {
            if (id != message.MessageId)
            {
                return NotFound();
            }

            if (message.UserId != null || ModelState.IsValid)
            {
                try
                {
                    _context.Update(message);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MessageExists(message.MessageId))
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
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", message.UserId);
            return View(message);
        }

        // GET: Messages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var message = await _context.Messages
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.MessageId == id);
            if (message == null)
            {
                return NotFound();
            }

            return View(message);
        }

        // POST: Messages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var message = await _context.Messages.FindAsync(id);
            _context.Messages.Remove(message);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MessageExists(int id)
        {
            return _context.Messages.Any(e => e.MessageId == id);
        }
    }
}
