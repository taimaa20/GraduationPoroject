using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NToastNotify;
using WebApplication29.Models;

namespace WebApplication29.Controllers
{

    public class AdminController : Controller
    {

        private readonly HomeServicesNewContext _context;
        private IWebHostEnvironment _hostEnvironment;
        private readonly IToastNotification _toastNotification;
        public AdminController(HomeServicesNewContext context, IWebHostEnvironment hostEnvironment, IToastNotification toastNotification)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            _toastNotification = toastNotification;

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

            List<Category> categories = db.Categories.ToList();

            var AllServices = from ser in services
                              join us in user on ser.UserId equals us.UserId into table1
                              from us in table1.ToList()
                              join cat in categories on ser.CategoryId equals cat.CategoryId into table2
                              from cat in table2.ToList()

                              select new joins { users = us, services = ser,categories=cat };


            return View(AllServices.ToList());
        }
        public IActionResult servicesNameSearch(string sName)
        {
            //FetchData();


            List<Service> services = db.Services.ToList();

            List<User> user = db.Users.ToList();

            List<Category> categories = db.Categories.ToList();

            var AllServices = from ser in services
                              join us in user on ser.UserId equals us.UserId into table1
                              from us in table1.ToList()
                              join cat in categories on ser.CategoryId equals cat.CategoryId into table2
                              from cat in table2.ToList()

                              select new joins { users = us, services = ser, categories = cat };


            return View(AllServices.ToList());
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
        public IActionResult DisplaySalaryEmployee(DateTime date)
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

           
            DateTime date1 = new DateTime(01 / 01 / 0001);
            if (date.ToString() == date1.ToString())
            {
                date = DateTime.Today;
            }
            var multable1 = employeeSalaryRecord.Where(x => x.roles.RoleId == 3 && x.salaries.MonthOfSalary.Month == date.Month);

            return View(multable1.ToList());

        }
      /*  public IActionResult DisplaySalaryEmployeeSearch(DateTime startDate, DateTime endDate)
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

        }*/
        public IActionResult AllUserServices(string ServName)
        {
            //FetchData();


            List<Service> services = db.Services.ToList();

            List<User> user = db.Users.ToList();

            List<UserService> userServices = db.UserServices.ToList();
            var AllServices = from usSer in userServices
                            join us in user on usSer.UserId equals us.UserId into table1
                            from us in table1.ToList()
                            join ser in services on usSer.ServiceId equals ser.ServiceId into table2
                            from ser in table2.ToList()

                            select new joins { users = us, services = ser, userService = usSer };

         

            if (!String.IsNullOrEmpty(ServName))
            {
                AllServices = AllServices.Where(x => x.services.ServiceName.Contains(ServName));
            }


            return View(AllServices.ToList());
        }
        public IActionResult DailyUserServices(DateTime startDate)
        {
            //FetchData();


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

            var multable1 = AllServices.Where(x => x.userService.Date == startDate);
            return View(multable1.ToList());
        }
        public IActionResult MonthelyUserServices(DateTime startDate)
        {
            //FetchData();


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
 


            var multable1 = AllServices.Where(x => x.userService.Date.Month == startDate.Month );
          
            return View(multable1.ToList());
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

            return View(AllServices.ToList());
        }
        public IActionResult UserReview()
        {
            //FetchData();


            List<Review> reviews = db.Reviews.ToList();

            List<User> user = db.Users.ToList();

            List<Service> services = db.Services.ToList();
            List<UserService> userServices = db.UserServices.ToList();
            var AllServices = from rev in reviews
                              join usSer in userServices on rev.UserServiceId equals usSer.UserServiceId into table1
                              from usSer in table1.ToList()
                              join us in user on usSer.UserId equals us.UserId into table2
                              from us in table2.ToList()
                              join ser in services on usSer.ServiceId equals ser.ServiceId into table3
                              from ser in table3.ToList()


                              select new joins { users = us, review = rev, userService = usSer,services= ser };


            return View(AllServices.ToList());
        }
        public IActionResult DailyUserReview(DateTime startDate)
        {
            //FetchData();

            List<Review> reviews = db.Reviews.ToList();

            List<User> user = db.Users.ToList();
            List<Service> services = db.Services.ToList();
            List<UserService> userServices = db.UserServices.ToList();
            var AllServices = from rev in reviews
                              join usSer in userServices on rev.UserServiceId equals usSer.UserServiceId into table1
                              from usSer in table1.ToList()
                              join us in user on usSer.UserId equals us.UserId into table2
                              from us in table2.ToList()
                              join ser in services on usSer.ServiceId equals ser.ServiceId into table3
                              from ser in table3.ToList()


                              select new joins { users = us, review = rev, userService = usSer, services = ser };


           
            DateTime date1 = new DateTime(01 / 01 / 0001);
            if (startDate.ToString() == date1.ToString())
            {
                startDate = DateTime.Today;
            }

            var multable1 = AllServices.Where(x => x.userService.Date.Day== startDate.Day);

            return View(multable1.ToList());
        }
        public IActionResult MonthelyUserReview(DateTime startDate)
        {
            //FetchData();


            List<Review> reviews = db.Reviews.ToList();
            List<Service> services = db.Services.ToList();
            List<User> user = db.Users.ToList();

            List<UserService> userServices = db.UserServices.ToList();
            var AllServices = from rev in reviews
                              join usSer in userServices on rev.UserServiceId equals usSer.UserServiceId into table1
                              from usSer in table1.ToList()
                              join us in user on usSer.UserId equals us.UserId into table2
                              from us in table2.ToList()
                              join ser in services on usSer.ServiceId equals ser.ServiceId into table3
                              from ser in table3.ToList()


                              select new joins { users = us, review = rev, userService = usSer, services = ser };

            DateTime date1 = new DateTime(01 / 01 / 0001);
            if (startDate.ToString() == date1.ToString())
            {
                startDate = DateTime.Today;
            }

            var multable1 = AllServices.Where(x => x.userService.Date.Month == startDate.Month);
            return View(multable1.ToList());
        }
        public IActionResult Payments(string UName)
        {
            //FetchData();


            List<Payment> payment = db.Payments.ToList();
            List<Service> services = db.Services.ToList();

            List<UserService> userServices = db.UserServices.ToList();
            List<User> user = db.Users.ToList();

            var AllServices = from pay in payment
                              join usrser in userServices on pay.UserServiceId equals usrser.UserServiceId into table1
                              from usrser in table1.ToList()
                              join usr in user on usrser.UserId equals usr.UserId into table2
                              from usr in table2.ToList()
                              join ser in services on usrser.ServiceId equals ser.ServiceId into table3
                              from ser in table3.ToList()




                              select new joins { users = usr, payment = pay, userService = usrser,services=ser };

            if (!String.IsNullOrEmpty(UName))
            {
                AllServices = AllServices.Where(x => x.users.FirstName!.Contains(UName) || x.users.LastName!.Contains(UName));
            }


            return View(AllServices.ToList());
        }
        public IActionResult MonthlyPayments(DateTime startDate)
        {
            //FetchData();
            List<Payment> payment = db.Payments.ToList();
            List<Service> services = db.Services.ToList();

            List<UserService> userServices = db.UserServices.ToList();
            List<User> user = db.Users.ToList();

            var AllServices = from pay in payment
                              join usrser in userServices on pay.UserServiceId equals usrser.UserServiceId into table1
                              from usrser in table1.ToList()
                              join usr in user on usrser.UserId equals usr.UserId into table2
                              from usr in table2.ToList()
                              join ser in services on usrser.ServiceId equals ser.ServiceId into table3
                              from ser in table3.ToList()




                              select new joins { users = usr, payment = pay, userService = usrser, services = ser };

            DateTime date1 = new DateTime(01 / 01 / 0001);
            if (startDate.ToString() == date1.ToString())
            {
                startDate = DateTime.Today;
            }

            var multable1 = AllServices.Where(x => x.payment.PaymentDate.Month == startDate.Month );


            return View(multable1.ToList());
        }
       


        public IActionResult Adds()
        {
           
            return View();
        }

        
        public IActionResult Adds1(string UserName, string Password, string ConfirmPassword, Login login, string FirstName, string LastName, string Gender, string Email, string PhoneNumber, string Adress, string City, DateTime BirthDate, string UserImage, IFormFile ImageFile, User user)
        {


            List<Login> loginss = db.Logins.ToList();
            var name = loginss.Where(x => x.UserName == UserName).ToList();
            if( Password!= ConfirmPassword)
            {
                _toastNotification.AddErrorToastMessage("The confirm password is wrong");
                return RedirectToAction("Adds", "Admin");
            }
            else
            {
                if (name.Count() == 0)
                {
                    login.RoleId = 3;
                    login.UserName = UserName;
                    login.Password = Password;
                    login.ConfimPassword = ConfirmPassword;
                    _context.Add(login);
                    _context.SaveChangesAsync();
                    List<Login> logins = db.Logins.ToList();
                    var LastId = logins.Where(x => x.UserName == UserName).Select(x => x.LoginId).FirstOrDefault();
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

                            user.ImageFile.CopyToAsync(fileStream);
                        }
                        user.UserImage = fileName;
                        _context.Add(user);
                        _context.SaveChangesAsync();
                        _toastNotification.AddSuccessToastMessage("Employee add sucssfully");
                        return RedirectToAction("Adds", "Admin");
                    }
                }
                else
                {
                    _toastNotification.AddErrorToastMessage("The username already taken");
                    return RedirectToAction("Adds", "Admin");
                }
            }
          

            return RedirectToAction("login");
        }
        public IActionResult ContactUsTable()

        {
            List<ContactU> contactUs = db.ContactUs.ToList();



            return View(contactUs.ToList());

          
        }
        public IActionResult AddSalary(string EmName)
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
