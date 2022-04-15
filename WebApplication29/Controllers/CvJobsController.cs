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
    public class CvJobsController : Controller
    {
        private readonly HomeServicesNewContext _context;

        public CvJobsController(HomeServicesNewContext context)
        {
            _context = context;
        }

        // GET: CvJobs
        public async Task<IActionResult> Index()
        {
            var homeServicesNewContext = _context.CvJobs.Include(c => c.Category).Include(c => c.User);
            return View(await homeServicesNewContext.ToListAsync());
        }

        // GET: CvJobs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cvJob = await _context.CvJobs
                .Include(c => c.Category)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.CvId == id);
            if (cvJob == null)
            {
                return NotFound();
            }

            return View(cvJob);
        }

        // GET: CvJobs/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId");
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId");
            return View();
        }

        // POST: CvJobs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CvId,UserId,YearsOfExperiance,LastWorkPlace,JobDescription,CategoryId")] CvJob cvJob)
        {
            if ((cvJob.UserId != null && cvJob.CategoryId != null )|| ModelState.IsValid)
            {
                _context.Add(cvJob);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", cvJob.CategoryId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", cvJob.UserId);
            return View(cvJob);
        }

        // GET: CvJobs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cvJob = await _context.CvJobs.FindAsync(id);
            if (cvJob == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", cvJob.CategoryId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", cvJob.UserId);
            return View(cvJob);
        }

        // POST: CvJobs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CvId,UserId,YearsOfExperiance,LastWorkPlace,JobDescription,CategoryId")] CvJob cvJob)
        {
            if (id != cvJob.CvId)
            {
                return NotFound();
            }

            if ((cvJob.UserId != null && cvJob.CategoryId != null) || ModelState.IsValid)
            {
                try
                {
                    _context.Update(cvJob);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CvJobExists(cvJob.CvId))
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", cvJob.CategoryId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", cvJob.UserId);
            return View(cvJob);
        }

        // GET: CvJobs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cvJob = await _context.CvJobs
                .Include(c => c.Category)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.CvId == id);
            if (cvJob == null)
            {
                return NotFound();
            }

            return View(cvJob);
        }

        // POST: CvJobs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cvJob = await _context.CvJobs.FindAsync(id);
            _context.CvJobs.Remove(cvJob);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CvJobExists(int id)
        {
            return _context.CvJobs.Any(e => e.CvId == id);
        }
    }
}
