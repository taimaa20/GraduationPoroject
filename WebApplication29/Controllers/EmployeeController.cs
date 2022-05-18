using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using WebApplication29.Models;

namespace WebApplication29.Controllers
{
    public class EmployeeController : Controller
    {

        private readonly HomeServicesNewContext _context;
        private IWebHostEnvironment _hostEnvironment;
        private readonly IToastNotification _toastNotification;
        public EmployeeController(HomeServicesNewContext context, IWebHostEnvironment hostEnvironment, IToastNotification toastNotification)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            _toastNotification = toastNotification;

        }
        HomeServicesNewContext db = new HomeServicesNewContext();
       
        public IActionResult Index()
        {
            int? employeeid = HttpContext.Session.GetInt32(id);
            ViewBag.id = HttpContext.Session.GetInt32(id);
           
            ViewBag.CountOfServices = _context.Services.Where(x => x.UserId == employeeid).Count();
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
            ViewBag.CountOfServicesToday = multable1.Where(x => x.services.UserId == employeeid).Count();
            ViewBag.countOFAllUsers = _context.Users.Count();


            ViewBag.countOFCustomer = _context.Logins.Where(x => x.RoleId == 5).Count();

            ViewBag.countOFMessages = _context.Messages.Where(x => x.UserId == employeeid && x.MessageDate==DateTime.Today).Count();
            EmployeeOwnServices();
            return View();
        }
        string id = "id";
        public IActionResult Eid()
        {
            int? employeeid = HttpContext.Session.GetInt32(id);
            ViewBag.id = HttpContext.Session.GetInt32(id);
            return View(employeeid);
                }
        /*      public async Task<IActionResult> Profile( )
          {
              string id = "id";
              int? empolyeeid = HttpContext.Session.GetInt32(id);
              if (empolyeeid == null)
              {
                  return NotFound();
              }

              var user = await _context.Users.FindAsync(empolyeeid);
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
          public async Task<IActionResult> Profile([Bind("UserId,FirstName,LastName,Gender,Email,PhoneNumber,Adress,City,BirthDate,LoginId,UserImage,ImageFile")] User user)
          {
              string id = "id";
              int? empolyeeid = HttpContext.Session.GetInt32(id);
              if (empolyeeid != user.UserId)
              {
                  return NotFound();
              }

              if (user.LoginId != null || ModelState.IsValid)
              {
                  try
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
                          _context.Update(user);
                          await _context.SaveChangesAsync();
                           // return to Index
                      }
                      else
                      {
                      }

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
        */
    
        public IActionResult SalarySlip(DateTime date) 
        {
          //  GymContext gym = new GymContext();
           int? employeeid = HttpContext.Session.GetInt32(id);
         ViewBag.id = HttpContext.Session.GetInt32(id);

            List<User> users = db.Users.ToList();
        List<Salary> salaries = db.Salaries.ToList();

            
            var multable = from usr in users
                       join s in salaries on usr.UserId equals s.UserId into table1
                       from s in table1.ToList()
                  


                       select new joins { salaries = s, users = usr };
            DateTime date1 = new DateTime(01 / 01 / 0001);
            if (date.ToString()==date1.ToString())
            {
            date = DateTime.Today;
            }
        var multable1 = multable.Where(x => x.salaries.UserId == employeeid && x.salaries.MonthOfSalary.Month ==date.Month);
            ViewBag.FirstName = multable.Where(x => x.salaries.UserId == employeeid && x.salaries.MonthOfSalary.Month == date.Month).Select(x=>x.users.FirstName).FirstOrDefault();
            ViewBag.LastName = multable.Where(x => x.salaries.UserId == employeeid && x.salaries.MonthOfSalary.Month == date.Month).Select(x => x.users.LastName).FirstOrDefault();
            ViewBag.UserId = multable.Where(x => x.salaries.UserId == employeeid && x.salaries.MonthOfSalary.Month == date.Month).Select(x => x.users.UserId).FirstOrDefault();
            ViewBag.Email = multable.Where(x => x.salaries.UserId == employeeid && x.salaries.MonthOfSalary.Month == date.Month).Select(x => x.users.Email).FirstOrDefault();
            ViewBag.Adress = multable.Where(x => x.salaries.UserId == employeeid && x.salaries.MonthOfSalary.Month == date.Month).Select(x => x.users.Adress).FirstOrDefault();
            ViewBag.MonthOfSalary = multable.Where(x => x.salaries.UserId == employeeid && x.salaries.MonthOfSalary.Month == date.Month).Select(x => x.salaries.MonthOfSalary).FirstOrDefault();
            ViewBag.Inventives = multable.Where(x => x.salaries.UserId == employeeid && x.salaries.MonthOfSalary.Month == date.Month).Select(x => x.salaries.Inventives).FirstOrDefault();
            ViewBag.Tracks = multable.Where(x => x.salaries.UserId == employeeid && x.salaries.MonthOfSalary.Month == date.Month).Select(x => x.salaries.Tracks).FirstOrDefault();
            ViewBag.Salary1 = multable.Where(x => x.salaries.UserId == employeeid && x.salaries.MonthOfSalary.Month == date.Month).Select(x => x.salaries.Salary1).FirstOrDefault();
            ViewBag.TotalSalary = multable.Where(x => x.salaries.UserId == employeeid && x.salaries.MonthOfSalary.Month == date.Month).Select(x => x.salaries.TotalSalary).FirstOrDefault();
            ViewBag.PhoneNumber = multable.Where(x => x.salaries.UserId == employeeid && x.salaries.MonthOfSalary.Month == date.Month).Select(x => x.users.PhoneNumber).FirstOrDefault();
            return View(multable1.ToList());
        }
        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
        DateTime startDate =  DateTime.Today;
        public IActionResult EmployeeServices(DateTime  startDate)
        {
            int? employeeid = HttpContext.Session.GetInt32(id);
            ViewBag.id = HttpContext.Session.GetInt32(id);

            List<Service> services = db.Services.ToList();

            List<User> user = db.Users.ToList();

            List<UserService> userServices = db.UserServices.ToList();
         

                               var AllServices = from usSer in userServices
                                                 join us in user on usSer.UserId equals us.UserId into table1
                                                 from us in table1.ToList()
                                                 join ser in services on usSer.ServiceId equals ser.ServiceId into table2
                                                 from ser in table2.ToList()

                                                 select new joins { users = us, services = ser, userService = usSer };



            DateTime date1 = new DateTime(01 / 01 / 0001);
            if (startDate.ToString() == date1.ToString())
            {
                startDate = DateTime.Today;
            }

            
                AllServices = AllServices.Where(x => x.services.UserId == employeeid && x.userService.Date == startDate);
           
            return View(AllServices.ToList());
        }
       public IActionResult UserServicesName(string UserName)
        {
            int? employeeid = HttpContext.Session.GetInt32(id);
            ViewBag.id = HttpContext.Session.GetInt32(id);

            List<Service> services = db.Services.ToList();

            List<User> user = db.Users.ToList();

            List<UserService> userServices = db.UserServices.ToList();


            var AllServices = from usSer in userServices
                              join us in user on usSer.UserId equals us.UserId into table1
                              from us in table1.ToList()
                              join ser in services on usSer.ServiceId equals ser.ServiceId into table2
                              from ser in table2.ToList()

                              select new joins { users = us, services = ser, userService = usSer };

            AllServices = AllServices.Where(x => x.services.UserId== employeeid).OrderByDescending(x=>x.userService.Date);
            if (!String.IsNullOrEmpty(UserName))
            {
                AllServices = AllServices.Where(x => x.users.FirstName!.Contains(UserName) || x.users.LastName!.Contains(UserName)).OrderByDescending(x => x.userService.Date);
            }

            return View(AllServices.ToList());
        }
        public IActionResult EmployeeOwnServices()
        {
            int? employeeid = HttpContext.Session.GetInt32(id);
            ViewBag.id = HttpContext.Session.GetInt32(id);
            List<Service> services = db.Services.ToList();
            services=services.Where(x=>x.UserId== employeeid).ToList();
            return View(services.ToList());
        }
        public IActionResult MounthelyEmployeeServices(DateTime startDate )
        {

            List<Service> services = db.Services.ToList();

            List<User> user = db.Users.ToList();

            List<UserService> userServices = db.UserServices.ToList();
          

            var AllServices = from usSer in userServices
                              join us in user on usSer.UserId equals us.UserId into table1
                              from us in table1.ToList()
                              join ser in services on usSer.ServiceId equals ser.ServiceId into table2
                              from ser in table2.ToList()

                              select new joins { users = us, services = ser, userService = usSer };
            int? employeeid = HttpContext.Session.GetInt32(id);
            ViewBag.id = HttpContext.Session.GetInt32(id);
            DateTime date1 = new DateTime(01 / 01 / 0001);
            if (startDate.ToString() == date1.ToString())
            {
                startDate = DateTime.Today;
            }
            var multable1 = AllServices.Where(x => x.services.UserId == employeeid && x.userService.Date.Month == startDate.Month).OrderBy(x=>x.userService.Date);
            return View(multable1.ToList());
        }
      
        public IActionResult EmployeeMessage(DateTime startDate )
        {
            int? employeeid = HttpContext.Session.GetInt32(id);
            ViewBag.id = HttpContext.Session.GetInt32(id);
            List<Message> messages = db.Messages.ToList();
           


            DateTime date1 = new DateTime(01 / 01 / 0001);
            if (startDate.ToString() == date1.ToString())
            {
                messages = messages.Where(x => x.UserId == employeeid).OrderByDescending(x => x.MessageDate).ToList();
            }
            else{
                messages = messages.Where(x => x.MessageDate == startDate && x.UserId == employeeid).OrderBy(x => x.MessageDate).ToList();

            }


            return View(messages.ToList());
        }
        public IActionResult ChangePassword()
        {
            string id = "id";
            int? employeeid = HttpContext.Session.GetInt32(id);
            ViewBag.id = HttpContext.Session.GetInt32(id);

            List<Login> logins = db.Logins.ToList();
            List<User> users = db.Users.ToList();
            var multable = from usr in users
                           join log in logins on usr.LoginId equals log.LoginId into table1
                           from log in table1.ToList()
                           select new joins { users = usr, logins = log };
            multable = multable.Where(x => x.users.UserId == employeeid).ToList();

            var pas = multable.Select(x => x.logins.Password).ToList();
            ViewBag.password = pas.FirstOrDefault();

            ViewBag.FirstName = users.Where(x => x.UserId == employeeid).Select(x => x.FirstName).FirstOrDefault();
            ViewBag.LastName = users.Where(x => x.UserId == employeeid).Select(x => x.LastName).FirstOrDefault();
            ViewBag.Gender = users.Where(x => x.UserId == employeeid).Select(x => x.Gender).FirstOrDefault();
            ViewBag.Email = users.Where(x => x.UserId == employeeid).Select(x => x.Email).FirstOrDefault();
            ViewBag.PhoneNumber = users.Where(x => x.UserId == employeeid).Select(x => x.PhoneNumber).FirstOrDefault();
            ViewBag.Adress = users.Where(x => x.UserId == employeeid).Select(x => x.Adress).FirstOrDefault();
            ViewBag.City = users.Where(x => x.UserId == employeeid).Select(x => x.City).FirstOrDefault();
            ViewBag.BirthDate = users.Where(x => x.UserId == employeeid).Select(x => x.BirthDate).FirstOrDefault();
            ViewBag.UserImage = users.Where(x => x.UserId == employeeid).Select(x => x.UserImage).FirstOrDefault();

            return View();
        }
        public IActionResult Profile(string FirstName, string LastName, string Gender, string Email, string PhoneNumber, string Adress, string City, DateTime BirthDate, string UserImage, IFormFile? ImageFile, User user)
        {
            List<User> users = db.Users.ToList();
            string id = "id";
            int employeeid = (int)HttpContext.Session.GetInt32(id);
            user.LoginId = users.Where(x => x.UserId == employeeid).Select(x => x.LoginId).FirstOrDefault();
            user.FirstName = FirstName;
            user.LastName = LastName;
            user.Gender = Gender;
            user.Email = Email;
            user.PhoneNumber = PhoneNumber;
            user.Adress = Adress;
            user.City = City;
            user.BirthDate = BirthDate;
            user.UserImage = UserImage;
       
            if (user.ImageFile != null)
            {
                string wRootPath = _hostEnvironment.WebRootPath;
                string fileName = Guid.NewGuid().ToString() + "_" + user.ImageFile.FileName;
                string path = Path.Combine(wRootPath + "/Image/", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {

                    user.ImageFile.CopyToAsync(fileStream);
                }
                user.UserImage = fileName;
            }
            user.UserId = employeeid;
            _context.Update(user);

            _context.SaveChangesAsync();
            return RedirectToAction("ChangePassword");
        }
        public IActionResult ChangePasswordBasic(string CurrentPassword, string NewPassword, string ConfirmPssword, Login login)
        {
            string id = "id";
            int? employeeid = HttpContext.Session.GetInt32(id);
            ViewBag.id = HttpContext.Session.GetInt32(id);
            List<Login> logins = db.Logins.ToList();
            List<User> users = db.Users.ToList();
            var multable = from usr in users
                           join log in logins on usr.LoginId equals log.LoginId into table1
                           from log in table1.ToList()
                           select new joins { users = usr, logins = log };
            multable = multable.Where(x => x.users.UserId == employeeid).ToList();

            var pas = multable.Select(x => x.logins.Password).FirstOrDefault();


            if (CurrentPassword == pas)
            {
                var multable11 = multable.Select(x => x.logins.LoginId).FirstOrDefault();
                var UserNAME = multable.Select(x => x.logins.UserName).FirstOrDefault();
                var Role0ID = multable.Select(x => x.logins.RoleId).FirstOrDefault();
                login.LoginId = multable11;
                login.Password = NewPassword;
                login.ConfimPassword = ConfirmPssword;
                login.UserName = UserNAME;
                login.RoleId = Role0ID;
                _context.Update(login);

                _context.SaveChangesAsync();
            }
            else
            {
                _toastNotification.AddErrorToastMessage("Your current password is not true");
            }

            return RedirectToAction("ChangePassword");
        }
        public IActionResult UserNameUpdate(string CurrentUserName, string NewUserName, Login login)
        {
            string id = "id";
            int? employeeid = HttpContext.Session.GetInt32(id);
            ViewBag.id = HttpContext.Session.GetInt32(id);
            List<Login> logins = db.Logins.ToList();
            List<User> users = db.Users.ToList();
            var multable = from usr in users
                           join log in logins on usr.LoginId equals log.LoginId into table1
                           from log in table1.ToList()
                           select new joins { users = usr, logins = log };
            multable = multable.Where(x => x.users.UserId == employeeid).ToList();

            var usrName = multable.Select(x => x.logins.UserName).FirstOrDefault();


            if (CurrentUserName == usrName)
            {
                var multable11 = multable.Select(x => x.logins.LoginId).FirstOrDefault();
                var Password = multable.Select(x => x.logins.Password).FirstOrDefault();
                var Role0ID = multable.Select(x => x.logins.RoleId).FirstOrDefault();
                var ConfimPassword = multable.Select(x => x.logins.ConfimPassword).FirstOrDefault();
                login.LoginId = multable11;
                login.Password = Password;
                login.ConfimPassword = ConfimPassword;
                login.UserName = NewUserName;
                login.RoleId = Role0ID;
                _context.Update(login);

                _context.SaveChangesAsync();
            }
            else
            {
                _toastNotification.AddErrorToastMessage("The username already taken");
             
            }



            return RedirectToAction("ChangePassword");
        }
    }
}
