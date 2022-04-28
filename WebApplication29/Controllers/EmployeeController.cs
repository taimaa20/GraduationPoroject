using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication29.Models;

namespace WebApplication29.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly HomeServicesNewContext _context;
        public EmployeeController(HomeServicesNewContext context)
        {
            _context = context;
        }
        HomeServicesNewContext db = new HomeServicesNewContext();
        // string uid = "UserId";
        public IActionResult Index()
        {
            //int? userId = HttpContext.Session.GetInt32(uid);
            // ViewBag.id = HttpContext.Session.GetInt32(uid);
            ViewBag.CountOfServices = _context.Services.Where(x => x.UserId == 4).Count();
            List<Service> services = db.Services.ToList();
            List<Category> categories = db.Categories.ToList();
            List<User> user = db.Users.ToList();

            List<UserService> userServices = db.UserServices.ToList();
            var AllServices = from ser in services
                              join us in user on ser.UserId equals us.UserId into table1
                              from us in table1.ToList()
                              join usSer in userServices on ser.ServiceId equals usSer.ServiceId into table2
                              from usSer in table2.ToList()


                              select new joins { users = us, services = ser, userService = usSer };


            var multable1 = AllServices.Where(x => x.userService.Date >= DateTime.Today);
            ViewBag.CountOfServicesToday = multable1.Where(x => x.services.UserId == 3).Count();
            ViewBag.countOFAllUsers = _context.Users.Count();


            ViewBag.countOFCustomer = _context.Logins.Where(x => x.RoleId == 3).Count();

            ViewBag.countOFMessages = _context.Messages.Where(x => x.UserId == 3).Count();
            EmployeeOwnServices();
            return View();
        }
       
        public async Task<IActionResult> Profile(int? id)
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
        public async Task<IActionResult> Profile(int id, [Bind("UserId,FirstName,LastName,Gender,Email,PhoneNumber,Adress,City,BirthDate,LoginId,UserImage")] User user)
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
      
        public IActionResult SalarySlip()
        {
          //  GymContext gym = new GymContext();
         //  int? employeeid = HttpContext.Session.GetInt32(id);
        // ViewBag.id = HttpContext.Session.GetInt32(id);

            List<User> users = db.Users.ToList();
        List<Salary> salaries = db.Salaries.ToList();
        

        var multable = from usr in users
                       join s in salaries on usr.UserId equals s.UserId into table1
                       from s in table1.ToList()
                  


                       select new joins { salaries = s, users = usr };
            
           

        var multable1 = multable.Where(x => x.salaries.UserId == 3 && x.salaries.MonthOfSalary.Month==DateTime.Today.Month);
            
        
            return View(multable1.ToList());
        }
        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
        DateTime startDate =  DateTime.Today;
        public IActionResult EmployeeServices( DateTime  startDate)
        {

            List<Service> services = db.Services.ToList();

            List<User> user = db.Users.ToList();

            List<UserService> userServices = db.UserServices.ToList();
         /*   var AllServices = from ser in services
                              join us in user on ser.UserId equals us.UserId into table1
                              from us in table1.ToList()
                              join usSer in userServices on ser.ServiceId equals usSer.ServiceId into table2
                              from usSer in table2.ToList()*/

                               var AllServices = from usSer in userServices
                                                 join us in user on usSer.UserId equals us.UserId into table1
                                                 from us in table1.ToList()
                                                 join ser in services on usSer.ServiceId equals ser.ServiceId into table2
                                                 from ser in table2.ToList()

                                                 select new joins { users = us, services = ser, userService = usSer };
       

            var multable1 = AllServices.Where(x => x.services.UserId == 3 && x.userService.Date>=startDate);
            return View(multable1.ToList());
        }
        public IActionResult UserServicesName(string UserName)
        {

            List<Service> services = db.Services.ToList();

            List<User> user = db.Users.ToList();

            List<UserService> userServices = db.UserServices.ToList();
            /*   var AllServices = from ser in services
                                 join us in user on ser.UserId equals us.UserId into table1
                                 from us in table1.ToList()
                                 join usSer in userServices on ser.ServiceId equals usSer.ServiceId into table2
                                 from usSer in table2.ToList()*/

            var AllServices = from usSer in userServices
                              join us in user on usSer.UserId equals us.UserId into table1
                              from us in table1.ToList()
                              join ser in services on usSer.ServiceId equals ser.ServiceId into table2
                              from ser in table2.ToList()

                              select new joins { users = us, services = ser, userService = usSer };

       
            if (!String.IsNullOrEmpty(UserName))
            {
                AllServices = AllServices.Where(x => x.users.FirstName!.Contains(UserName) || x.users.LastName!.Contains(UserName));
            }

            return View(AllServices.ToList());
        }
        public IActionResult EmployeeOwnServices()
        {
            List<Service> services = db.Services.ToList();
            services=services.Where(x=>x.UserId==3).ToList();
            return View(services.ToList());
        }
        public IActionResult MounthelyEmployeeServices(DateTime startDate ,DateTime endDate)
        {

            List<Service> services = db.Services.ToList();

            List<User> user = db.Users.ToList();

            List<UserService> userServices = db.UserServices.ToList();
            /*   var AllServices = from ser in services
                                 join us in user on ser.UserId equals us.UserId into table1
                                 from us in table1.ToList()
                                 join usSer in userServices on ser.ServiceId equals usSer.ServiceId into table2
                                 from usSer in table2.ToList()*/

            var AllServices = from usSer in userServices
                              join us in user on usSer.UserId equals us.UserId into table1
                              from us in table1.ToList()
                              join ser in services on usSer.ServiceId equals ser.ServiceId into table2
                              from ser in table2.ToList()

                              select new joins { users = us, services = ser, userService = usSer };


            var multable1 = AllServices.Where(x => x.services.UserId == 3 && x.userService.Date >= startDate&&x.userService.Date<=endDate);
            return View(multable1.ToList());
        }
        public IActionResult EmployeeMessage()
        {
            List<Message> messages = db.Messages.ToList();
            messages=messages.Where(x=>x.UserId == 3).ToList();
            return View(messages.ToList());
        }
    }
}
