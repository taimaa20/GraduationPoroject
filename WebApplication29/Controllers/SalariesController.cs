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
    public class SalariesController : Controller
    {
        private readonly HomeServicesNewContext _context;

        public SalariesController(HomeServicesNewContext context)
        {
            _context = context;
        }

        HomeServicesNewContext db = new HomeServicesNewContext();
        // GET: Salaries
        public async Task<IActionResult> Index()
        {
            List<User> users = db.Users.ToList();


            List<Login> logins = db.Logins.ToList();
            List<Role> roles = db.Roles.ToList();
            List<Salary> salaries = db.Salaries.ToList();

            var employeeSalaryRecord = from us in users
                                       join log in logins on us.LoginId equals log.LoginId into table1
                                       from log in table1.ToList()
                                       join rol in roles on log.RoleId equals rol.RoleId into table2
                                       from rol in table2.ToList()
                                       join sal in salaries on us.UserId equals sal.UserId into table3
                                       from sal in table3.ToList()
                                       select new joins { salaries = sal, roles = rol, users = us, logins = log };

         
            var homeServicesNewContext= employeeSalaryRecord.Where(x => x.roles.RoleId ==3 || x.roles.RoleId == 4);
            return View( homeServicesNewContext.ToList());
        }

        // GET: Salaries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salary = await _context.Salaries
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.SalaryId == id);
            if (salary == null)
            {
                return NotFound();
            }

            return View(salary);
        }

        // GET: Salaries/Create
        public IActionResult Create()
        {
    

            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "FirstName");
            return View();
        }

        // POST: Salaries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SalaryId,Salary1,Tracks,Inventives,MonthOfSalary,TotalSalary,UserId")] Salary salary)
        {
            if (salary.UserId!=null|| ModelState.IsValid)
            {
                _context.Add(salary);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "FirstName", salary.UserId);
            return View(salary);
        }

        // GET: Salaries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salary = await _context.Salaries.FindAsync(id);
            if (salary == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "FirstName", salary.UserId);
            return View(salary);
        }

        // POST: Salaries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SalaryId,Salary1,Tracks,Inventives,MonthOfSalary,TotalSalary,UserId")] Salary salary)
        {
            if (id != salary.SalaryId)
            {
                return NotFound();
            }

            if (salary.UserId != null || ModelState.IsValid)
            {

                try
                {
                    _context.Update(salary);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SalaryExists(salary.SalaryId))
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
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "FirstName", salary.UserId);
            return View(salary);
        }

        // GET: Salaries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salary = await _context.Salaries
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.SalaryId == id);
            if (salary == null)
            {
                return NotFound();
            }

            return View(salary);
        }

        // POST: Salaries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var salary = await _context.Salaries.FindAsync(id);
            _context.Salaries.Remove(salary);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SalaryExists(int id)
        {
            return _context.Salaries.Any(e => e.SalaryId == id);
        }
    }
}
