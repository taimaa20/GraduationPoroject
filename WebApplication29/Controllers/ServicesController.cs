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
    public class ServicesController : Controller
    {


        private readonly HomeServicesNewContext _context;
        private IWebHostEnvironment _hostEnvironment;
        private readonly IToastNotification _toastNotification;
        public ServicesController(HomeServicesNewContext context, IWebHostEnvironment hostEnvironment, IToastNotification toastNotification)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            _toastNotification = toastNotification;

        }
        HomeServicesNewContext db = new HomeServicesNewContext();
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

            List<User> users = db.Users.ToList();


            List<Login> logins = db.Logins.ToList();
            List<Role> roles = db.Roles.ToList();




            var lob = from us in users
                      join log in logins on us.LoginId equals log.LoginId into table1
                      from log in table1.ToList()
                      join rol in roles on log.RoleId equals rol.RoleId into table2
                      from rol in table2.ToList()
                      where rol.RoleId == 3
                      select new
                      {
                          LobName = us.FirstName,
                          LoblName = us.LastName,
                          LobID = us.UserId
                      };


            var list = lob.Select(x => new SelectListItem
            {
                Value = x.LobID.ToString(),
                Text = (x.LobName + " " + x.LoblName).ToString(),

            }).ToList();

            ViewBag.Organisations = list;

            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
           
            return View();
        }

        // POST: Services/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ServiceId,ServiceName,Price,CategoryId,UserId,Description,ImageFile")] Service service)
        {
            List<User> users = db.Users.ToList();


            List<Login> logins = db.Logins.ToList();
            List<Role> roles = db.Roles.ToList();




            var lob = from us in users
                      join log in logins on us.LoginId equals log.LoginId into table1
                      from log in table1.ToList()
                      join rol in roles on log.RoleId equals rol.RoleId into table2
                      from rol in table2.ToList()
                      where rol.RoleId == 3
                      select new
                      {
                          LobName = us.FirstName,
                          LoblName = us.LastName,
                          LobID = us.UserId
                      };


            var list = lob.Select(x => new SelectListItem
            {
                Value = x.LobID.ToString(),
                Text = (x.LobName +" "+ x.LoblName).ToString(),
               
            }).ToList();

            ViewBag.Organisations = list;
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
                    _toastNotification.AddSuccessToastMessage("Service add sucssfully");
                    return RedirectToAction("Create");
                }
            }
            
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", service.CategoryId);
         
            return View(service);
            
        }
        // GET: Services/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            List<User> users = db.Users.ToList();


            List<Login> logins = db.Logins.ToList();
            List<Role> roles = db.Roles.ToList();
            List<Service> services = db.Services.ToList();



            var lob = from us in users
                      join log in logins on us.LoginId equals log.LoginId into table1
                      from log in table1.ToList()
                      join rol in roles on log.RoleId equals rol.RoleId into table2
                      from rol in table2.ToList()
                      where rol.RoleId == 3
                      select new
                      {
                          LobName = us.FirstName,
                          LoblName = us.LastName,
                          LobID = us.UserId
                      };


            var list = lob.Select(x => new SelectListItem
            {
                Value = x.LobID.ToString(),
                Text = (x.LobName + " " + x.LoblName).ToString(),

            }).ToList();

            ViewBag.Organisations = list; 
            ViewBag.img = services.Where(x=>x.ServiceId==id).Select(x=>x.UserImage).FirstOrDefault();
            var service = await _context.Services.FindAsync(id);
            if (service == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", service.CategoryId);
            
            return View(service);
        }

        // POST: Services/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,string img, [Bind("ServiceId,ServiceName,Price,CategoryId,UserId,Description,UserImage,ImageFile")] Service service)
        {

            List<User> users = db.Users.ToList();
            List<Service> services = db.Services.ToList();


            List<Login> logins = db.Logins.ToList();
            List<Role> roles = db.Roles.ToList();




            var lob = from us in users
                      join log in logins on us.LoginId equals log.LoginId into table1
                      from log in table1.ToList()
                      join rol in roles on log.RoleId equals rol.RoleId into table2
                      from rol in table2.ToList()
                      where rol.RoleId == 3
                      select new
                      {
                          LobName = us.FirstName,
                          LoblName = us.LastName,
                          LobID = us.UserId
                      };


            var list = lob.Select(x => new SelectListItem
            {
                Value = x.LobID.ToString(),
                Text = (x.LobName + " " + x.LoblName).ToString(),

            }).ToList();
         
            ViewBag.Organisations = list;


            if (id != service.ServiceId)
            {
                return NotFound();
            }
         
            if ((service.CategoryId != null && service.UserId != null) || ModelState.IsValid)
            {
                try { 
                   service.UserImage = img;

                /* formFiles=services.Where(x=>x.ServiceId==id).Select(x=>x.UserImage).FirstOrDefault().ToList();
                 IFormFile file = img;
                 service.ImageFile = file;*/
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
                    }

                 
                    _context.Update(service);
                        await _context.SaveChangesAsync();
                  
                    
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
                _toastNotification.AddSuccessToastMessage("Service edited sucssfully");

                return RedirectToAction("servicesNameSearch","Admin");
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", service.CategoryId);
          
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

        public IActionResult EditView()
        {

            List<User> users = db.Users.ToList();


            List<Login> logins = db.Logins.ToList();
            List<Role> roles = db.Roles.ToList();

            
           

            var lob = from us in users
                      join log in logins on us.LoginId equals log.LoginId into table1
                      from log in table1.ToList()
                      join rol in roles on log.RoleId equals rol.RoleId into table2
                      from rol in table2.ToList()
                   where rol.RoleId==3
                      select new
                      {
                          LobName = us.FirstName,
                          LobID = us.UserId
                      };


            var list = lob.Select(x => new SelectListItem
            {
                Value = x.LobID.ToString(),
                Text = x.LobName
            }).ToList();

            ViewBag.Organisations = list;

            return View();
        }
        public IActionResult Editsub(int ServiceId,string ServiceName,double Price,string CategoryId,string UserName ,string Description, IFormFile ImageFile)
        {
            return View();
        }
    }
}
