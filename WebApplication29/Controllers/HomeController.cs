using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NToastNotify;
using System.Diagnostics;
using WebApplication29.Models;

namespace WebApplication29.Controllers
{
    public class HomeController : Controller
    {

        private readonly HomeServicesNewContext _context;
        private IWebHostEnvironment _hostEnvironment;
        private readonly IToastNotification _toastNotification;
        public HomeController(HomeServicesNewContext context, IWebHostEnvironment hostEnvironment, IToastNotification toastNotification)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            _toastNotification = toastNotification;

        }

        HomeServicesNewContext db = new HomeServicesNewContext();
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult HomeIndex()
        {
            return View();
        }
        public IActionResult AboutUs()
        {
            return View();
        }
        public IActionResult ContactUs()
        {
            return View();
        }
        public IActionResult Porifilo()
        {
            return View();
        }

        public IActionResult Pricing()
        {
            string id = "id";
            int? employeeid = HttpContext.Session.GetInt32(id);
            if (employeeid == null)
            {
                return RedirectToAction("Login", "LoginAndSession");
            }
            return RedirectToAction("create", "Services");
        }
        public IActionResult Privacy()
        {
            return View();
        }


        public IActionResult Services()
        {
            List<Service> services = db.Services.ToList();
           
           
            return View(services.ToList());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult _UserServicePartial(int ServiceId, string Description, string ServiceName, double Price, string UserImage)
        {
          
            string id = "id";
            int? employeeid = HttpContext.Session.GetInt32(id);
            if (employeeid == null)
            {
                _toastNotification.AddErrorToastMessage("Login First please");

                return RedirectToAction("Login", "LoginAndSession");
            }
            ViewBag.ServiceId = ServiceId;
            ViewBag.Description = Description;
            ViewBag.ServiceName = ServiceName;
            ViewBag.Price = Price;
            ViewBag.UserImage = UserImage;

            return View();
        }
        public IActionResult UserServiceAdd(int ServiceId, DateTime Date, UserService userService)
        {
            string id = "id";
            int? employeeid = HttpContext.Session.GetInt32(id);
            ViewBag.ServiceId = ServiceId;



            userService.ServiceId = ViewBag.ServiceId;
            userService.Date = Date;
            userService.UserId = employeeid;
            userService.Status = "under process";
            _context.Add(userService);

            _context.SaveChangesAsync();


            int lastuserServiceId = db.UserServices.Max(item => item.UserServiceId);
            return RedirectToAction("_paymentPartial", new { UserServiceId = lastuserServiceId });
        }
        public IActionResult _paymentPartial(int UserServiceId)
        {
            string id = "id";
            int? employeeid = HttpContext.Session.GetInt32(id);
            ViewBag.UserServiceId = UserServiceId;

            List<Service> services = db.Services.ToList();

            List<UserService> userServices = db.UserServices.ToList();
            var AllServices = from ser in services
                              join usrser in userServices on ser.ServiceId equals usrser.ServiceId into table1
                              from usrser in table1.ToList()

                              select new joins { services = ser, userService = usrser };


            AllServices = AllServices.Where(x => x.userService.UserServiceId == UserServiceId);

            double am = AllServices.Select(x => x.services.Price).SingleOrDefault();

            ViewBag.PaymentAmount = am;
            
            return View();
        }
        public IActionResult AddPayment(int UserServiceId,string CardNumber,string PaymenType, Payment payment)
        {
            string id = "id";
            int? employeeid = HttpContext.Session.GetInt32(id);


            List<Service> services = db.Services.ToList();

            List<UserService> userServices = db.UserServices.ToList();
         

            var AllServices = from ser in services
                              join usrser in userServices on ser.ServiceId equals usrser.ServiceId into table1
                              from usrser in table1.ToList()
                              
                              select new joins {services=ser,userService=usrser};
            

            AllServices = AllServices.Where(x => x.userService.UserServiceId ==UserServiceId);

            double am = AllServices.Select(x => x.services.Price).SingleOrDefault();

            ViewBag.PaymentAmount = am;
            payment.UserServiceId = UserServiceId;
            payment.PaymentAmount = am;
            payment.CardNumber = CardNumber;
            payment.PaymentType = PaymenType;
            payment.PaymentDate = DateTime.Now;
          
      
            if(string.IsNullOrEmpty(CardNumber))
            {
                _toastNotification.AddErrorToastMessage("the payment card is invalid");

                return RedirectToAction("_paymentPartial", new {UserServiceId=UserServiceId});
            }

            else
            {
                _context.Add(payment);

                _context.SaveChangesAsync();
                _toastNotification.AddSuccessToastMessage("Paymet done successfully");
                _toastNotification.AddAlertToastMessage("get your next service");
                return RedirectToAction("Services");
               
            }
        }

    }
}