using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication29.Models;

namespace WebApplication29.Controllers
{

    public class AdminController : Controller
    {
     
        private readonly HomeServicesNewContext _context;
        private IWebHostEnvironment _hostEnvironment;
        public AdminController(HomeServicesNewContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;

        }
      
        HomeServicesNewContext db = new HomeServicesNewContext();
        public IActionResult AllUsers(string name)
        {
            //FetchData();


            List<User> user = db.Users.ToList();
            var getuser = from m in _context.Users
                          select m;
            if (!String.IsNullOrEmpty(name))
            {
                getuser = getuser.Where(x => x.FirstName.Contains(name) || x.LastName.Contains(name));

            }




            return View(getuser.ToList());


        }
        public IActionResult Users()
        {
            //FetchData();


            List<User> user = db.Users.ToList();



            return View(user.ToList());


        }
        public IActionResult services()
        {
            //FetchData();


            List<Service> services = db.Services.ToList();

            List<User> user = db.Users.ToList();

            var AllServices = from ser in services
                              join us in user on ser.UserId equals us.UserId into table1
                              from us in table1.ToList()

                              select new joins { users = us, services = ser };


            return View(AllServices);
        }
        public IActionResult servicesNameSearch(string sName)
        {
            //FetchData();


            List<Service> services = db.Services.ToList();

            List<User> user = db.Users.ToList();

            var AllServices = from ser in services
                              join us in user on ser.UserId equals us.UserId into table1
                              from us in table1.ToList()

                              select new joins { users = us, services = ser };
            if (!String.IsNullOrEmpty(sName))
            {
                AllServices = AllServices.Where(x => x.services.ServiceName!.Contains(sName));

            }


            return View(AllServices.ToList());
        }
        public IActionResult CategoryNameSearch(string CatName)
        {
            //FetchData();


            List<Category> categories = db.Categories.ToList();

            var getuser = from m in _context.Categories
                          select m;


            if (!String.IsNullOrEmpty(CatName))
            {
                getuser = getuser.Where(x => x.CategoryName.Contains(CatName));

            }


            return View(getuser.ToList());
        }
        public IActionResult DisplayEmployee(string EmName)
        {
            //FetchData();

            List<User> users = db.Users.ToList();


            List<Login> logins = db.Logins.ToList();
            List<Role> roles = db.Roles.ToList();

            var employeeRecord = from us in users
                                 join log in logins on us.LoginId equals log.LoginId into table1
                                 from log in table1.ToList()
                                 join rol in roles on log.RoleId equals rol.RoleId into table2
                                 from rol in table2.ToList()

                                 select new joins { roles = rol, users = us, logins = log };
            var mal = employeeRecord.Where(x => x.roles.RoleId == 3);
            if (!String.IsNullOrEmpty(EmName))
            {
                mal = mal.Where(x => x.users.FirstName!.Contains(EmName) || x.users.LastName!.Contains(EmName));
            }

            return View(mal.ToList());

        }
        public IActionResult DisplaySalaryEmployee()
        {
            //FetchData();





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

            var multable1 = employeeSalaryRecord.Where(x => x.roles.RoleId == 3 );
            return View(employeeSalaryRecord.ToList());

        }
        public IActionResult DisplaySalaryEmployeeSearch(DateTime startDate, DateTime endDate)
        {
            //FetchData();





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

            var multable1 = employeeSalaryRecord.Where(x => x.salaries.MonthOfSalary >= startDate && x.salaries.MonthOfSalary <= endDate);

            return View(multable1.ToList());

        }
        public IActionResult AllUserServices(string ServName)
        {
            //FetchData();


            List<Service> services = db.Services.ToList();

            List<User> user = db.Users.ToList();

            List<UserService> userServices = db.UserServices.ToList();
            var AllServices = from ser in services
                              join us in user on ser.UserId equals us.UserId into table1
                              from us in table1.ToList()
                              join usSer in userServices on ser.ServiceId equals usSer.ServiceId into table2
                              from usSer in table2.ToList()


                              select new joins { users = us, services = ser, userService = usSer };

            if (!String.IsNullOrEmpty(ServName))
            {
                AllServices = AllServices.Where(x => x.services.ServiceName.Contains(ServName));
            }


            return View(AllServices);
        }
        public IActionResult DailyUserServices(DateTime startDate, DateTime endDate)
        {
            //FetchData();


            List<Service> services = db.Services.ToList();

            List<User> user = db.Users.ToList();

            List<UserService> userServices = db.UserServices.ToList();
            var AllServices = from ser in services
                              join us in user on ser.UserId equals us.UserId into table1
                              from us in table1.ToList()
                              join usSer in userServices on ser.ServiceId equals usSer.ServiceId into table2
                              from usSer in table2.ToList()


                              select new joins { users = us, services = ser, userService = usSer };


            var multable1 = AllServices.Where(x => x.userService.Date >= startDate && x.userService.Date <= endDate);
            return View(multable1);
        }
        public IActionResult MonthelyUserServices(DateTime startDate, DateTime endDate)
        {
            //FetchData();


            List<Service> services = db.Services.ToList();

            List<User> user = db.Users.ToList();

            List<UserService> userServices = db.UserServices.ToList();
            var AllServices = from ser in services
                              join us in user on ser.UserId equals us.UserId into table1
                              from us in table1.ToList()
                              join usSer in userServices on ser.ServiceId equals usSer.ServiceId into table2
                              from usSer in table2.ToList()


                              select new joins { users = us, services = ser, userService = usSer };


            var multable1 = AllServices.Where(x => x.userService.Date >= startDate && x.userService.Date <= endDate);
          
            return View(multable1);
        }
        public IActionResult UserServicesStatus(string ServStatus)
        {
            //FetchData();


            List<Service> services = db.Services.ToList();

            List<User> user = db.Users.ToList();

            List<UserService> userServices = db.UserServices.ToList();
            var AllServices = from ser in services
                              join us in user on ser.UserId equals us.UserId into table1
                              from us in table1.ToList()
                              join usSer in userServices on ser.ServiceId equals usSer.ServiceId into table2
                              from usSer in table2.ToList()


                              select new joins { users = us, services = ser, userService = usSer };

            if (!String.IsNullOrEmpty(ServStatus))
            {
                AllServices = AllServices.Where(x => x.userService.Status.Contains(ServStatus));
            }

            return View(AllServices);
        }
        public IActionResult UserReview()
        {
            //FetchData();


            List<Review> reviews = db.Reviews.ToList();

            List<User> user = db.Users.ToList();

            List<UserService> userServices = db.UserServices.ToList();
            var AllServices = from rev in reviews
                              join usSer in userServices on rev.UserServiceId equals usSer.UserServiceId into table1
                              from usSer in table1.ToList()
                              join us in user on usSer.UserId equals us.UserId into table2
                              from us in table2.ToList()


                              select new joins { users = us, review = rev, userService = usSer };


            return View(AllServices);
        }
        public IActionResult DailyUserReview(DateTime startDate, DateTime endDate)
        {
            //FetchData();

            List<Review> reviews = db.Reviews.ToList();

            List<User> user = db.Users.ToList();

            List<UserService> userServices = db.UserServices.ToList();
            var AllServices = from rev in reviews
                              join usSer in userServices on rev.UserServiceId equals usSer.UserServiceId into table1
                              from usSer in table1.ToList()
                              join us in user on usSer.UserId equals us.UserId into table2
                              from us in table2.ToList()


                              select new joins { users = us, review = rev, userService = usSer };

            var multable1 = AllServices.Where(x => x.userService.Date >= startDate && x.userService.Date <= endDate);

            return View(multable1);
        }
        public IActionResult MonthelyUserReview(DateTime startDate, DateTime endDate)
        {
            //FetchData();


            List<Review> reviews = db.Reviews.ToList();

            List<User> user = db.Users.ToList();

            List<UserService> userServices = db.UserServices.ToList();
            var AllServices = from rev in reviews
                              join usSer in userServices on rev.UserServiceId equals usSer.UserServiceId into table1
                              from usSer in table1.ToList()
                              join us in user on usSer.UserId equals us.UserId into table2
                              from us in table2.ToList()


                              select new joins { users = us, review = rev, userService = usSer };

            var multable1 = AllServices.Where(x => x.userService.Date >= startDate && x.userService.Date <= endDate);
            return View(multable1);
        }
        public IActionResult Payments(string UName)
        {
            //FetchData();


            List<Payment> payment = db.Payments.ToList();

            List<User> user = db.Users.ToList();

            var AllServices = from pay in payment
                              join us in user on pay.UserId equals us.UserId into table1
                              from us in table1.ToList()



                              select new joins { users = us, payment = pay };

            if (!String.IsNullOrEmpty(UName))
            {
                AllServices = AllServices.Where(x => x.users.FirstName!.Contains(UName) || x.users.LastName!.Contains(UName));
            }


            return View(AllServices);
        }
        public IActionResult MonthlyPayments(DateTime startDate, DateTime endDate)
        {
            //FetchData();


            List<Payment> payment = db.Payments.ToList();

            List<User> user = db.Users.ToList();

            var AllServices = from pay in payment
                              join us in user on pay.UserId equals us.UserId into table1
                              from us in table1.ToList()



                              select new joins { users = us, payment = pay };
            var multable1 = AllServices.Where(x => x.payment.PaymentDate >= startDate && x.payment.PaymentDate <= endDate);


            return View(multable1);
        }



        public IActionResult Adds()
        {
            ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleId");
            return View();
        }

        // POST: Logins/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Adds([Bind("LoginId,UserName,Password,ConfimPassword,RoleId")] Login login, string FirstName, string LastName, string Gender, string Email, string PhoneNumber, string Adress, string City, DateTime BirthDate, string UserImage, IFormFile ImageFile)
        {
            if (login.RoleId != null || ModelState.IsValid)
            {
                User user = new User();

                login.RoleId = 4;

                _context.Add(login);

                var LastId = _context.Logins.OrderByDescending(x => x.LoginId).FirstOrDefault().LoginId;
                user.FirstName = FirstName;
                user.LastName = LastName;

                user.Gender = Gender;
                user.PhoneNumber = PhoneNumber;
                user.Email = Email;
                user.Adress = Adress;
                user.City = City;
                user.BirthDate = BirthDate;
                user.ImageFile = ImageFile;
                user.LoginId = LastId;
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
                    _context.Add(user);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
                ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleId", login.RoleId);
                return View(login);
            
        }
        public IActionResult ContactUsTable()

        {
            List<ContactU> contactUs = db.ContactUs.ToList();



            return View(contactUs.ToList());

          
        }
        public IActionResult Index()

        {


            ViewBag.countOFEmployees = _context.Logins.Where(x => x.RoleId == 3).Count();
            ViewBag.countOFAllUsers = _context.Users.Count();
            ViewBag.countOFServices = _context.Services.Count();

            ViewBag.countOFCustomer = _context.Logins.Where(x => x.RoleId == 5).Count();
            Users();
            return View();
        }
    }
}
