using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using WebApplication29.Models;

namespace WebApplication29.Controllers
{
    public class CostumerController : Controller
    {
        private readonly HomeServicesNewContext _context;
        private IWebHostEnvironment _hostEnvironment;
        private readonly IToastNotification _toastNotification;
        public CostumerController(HomeServicesNewContext context, IWebHostEnvironment hostEnvironment, IToastNotification toastNotification)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            _toastNotification = toastNotification;

        }
        HomeServicesNewContext db = new HomeServicesNewContext();
        public IActionResult Index()
        {
            string id = "id";

            int? employeeid = HttpContext.Session.GetInt32(id);
            ViewBag.id = HttpContext.Session.GetInt32(id);
            if (employeeid==null)
            {
                _toastNotification.AddErrorToastMessage("Login First please");

                return RedirectToAction("Login", "LoginAndSession");
            }
           
            List<UserService> userServices = db.UserServices.ToList();
            List<User> users = db.Users.ToList();
            List<Service> services = db.Services.ToList();
            List<Payment> payments = db.Payments.ToList();
            ViewBag.CountOfUserServices = _context.UserServices.Where(x => x.UserId == employeeid).Count();
            var AllServices = from us in users
                              join usSer in userServices on us.UserId equals usSer.UserId into table1
                              from usSer in table1.ToList()
                              join pay in payments on usSer.UserServiceId equals pay.UserServiceId into table2
                              from pay in table2.ToList()


                              select new joins { users = us, payment = pay, userService = usSer };


            var multable1 = AllServices.Where(x => x.users.UserId== employeeid).Select(x=>x.payment.PaymentAmount).Sum();
            ViewBag.CountOfPayment=multable1;


            var dServices = from usSer in userServices
                              join us in users on usSer.UserId equals us.UserId into table1
                              from us in table1.ToList()
                              join ser in services on usSer.ServiceId equals ser.ServiceId into table2
                              from ser in table2.ToList()

                              select new joins { users = us, services = ser, userService = usSer };




            dServices = dServices.Where(x => x.userService.UserId == employeeid);
            var DoneServices = dServices.Where(x => x.userService.Status == "Done" || x.userService.Status == "done").ToList();

            ViewBag.CountOfDoneServices = DoneServices.Count();
            ViewBag.CountOfTodayServices = _context.UserServices.Where(x => x.UserId == employeeid && x.Date==DateTime.Today).Count();

            var AllUserServices = from ser in services
                              join us in users on ser.UserId equals us.UserId into table1
                              from us in table1.ToList()
                              join usSer in userServices on ser.ServiceId equals usSer.ServiceId into table2
                              from usSer in table2.ToList()


                              select new joins { users = us, services = ser, userService = usSer };
            AllUserServices = AllUserServices.Where(x => x.userService.UserId == employeeid).ToList().OrderBy(x=>x.userService.Date);

            return View(AllUserServices);
        }
        public IActionResult AddReview()
        {
            string id = "id";
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


            

            AllServices = AllServices.Where(x => x.userService.UserId == employeeid );
           var DoneServices=AllServices.Where(x => x.userService.Status == "Done"||x.userService.Status=="done");

            return View(DoneServices.ToList());
        }

        public IActionResult Checkpayments()
        {
            return View();
        }
        public IActionResult ServicesDetails(int UserServiceId)
        {
           
            List<UserService> userServices = db.UserServices.ToList();
            List<Service> services = db.Services.ToList();
            var AllServices = from usSer in userServices
                              join ser in services on usSer.ServiceId equals ser.ServiceId into table1
                              from ser in table1.ToList()

                              select new joins {  services = ser, userService = usSer };


            ViewBag.UserServiceId = UserServiceId;
            AllServices = AllServices.Where(x=>x.userService.UserServiceId==UserServiceId).ToList();

            return View(AllServices);
        }
        public IActionResult CreateReview(int UserServiceId,double ReviewRange,string ReviewText,Review review)
        {
          review.UserServiceId = UserServiceId;
          review.ReviewRange = ReviewRange;
          review.ReviewText = ReviewText;
            _context.Add(review);

            _context.SaveChangesAsync();



            return RedirectToAction("ServicesDetails", new {UserServiceId=UserServiceId});
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

            ViewBag.FirstName = users.Where(x=>x.UserId==employeeid).Select(x=>x.FirstName).FirstOrDefault();
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
        public IActionResult Profile(string FirstName,string LastName,string Gender,string Email ,string PhoneNumber,string Adress,string City,DateTime BirthDate,string UserImage, IFormFile? ImageFile,User user)
        {
            List<User> users = db.Users.ToList();
            string id = "id";
            int employeeid = (int)HttpContext.Session.GetInt32(id);
            user.LoginId=users.Where(x => x.UserId == employeeid).Select(x => x.LoginId).FirstOrDefault();
            user.FirstName = FirstName;
            user.LastName = LastName;   
            user.Gender = Gender;
            user.Email = Email;
            user.PhoneNumber = PhoneNumber;
            user.Adress = Adress;
            user.City = City;
            user.BirthDate = BirthDate;
           
            user.ImageFile = ImageFile;
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
        public IActionResult ServicesWithPayment(DateTime? startdate=null)
        {


            string id = "id";
            int? employeeid = HttpContext.Session.GetInt32(id);
            ViewBag.id = HttpContext.Session.GetInt32(id);
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




            if (startdate==null)
            {
                AllServices = AllServices.Where(x => x.userService.UserId == employeeid);
            }
            else
            {
                AllServices = AllServices.Where(x => x.userService.UserId == employeeid && x.userService.Date == startdate);
            }
         
            return View(AllServices);
        }

    }
}
