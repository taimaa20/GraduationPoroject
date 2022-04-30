using Microsoft.AspNetCore.Mvc;
using WebApplication29.Models;

namespace WebApplication29.Controllers
{
    public class LoginAndSessionController : Controller
    {
        private readonly HomeServicesNewContext _context;
        public LoginAndSessionController(HomeServicesNewContext context)
        {
            _context = context;
        }
        HomeServicesNewContext db = new HomeServicesNewContext();
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]


        public IActionResult Login(string UserName, string Password)
        {
            
            List<Login> logins = db.Logins.ToList();
            List<User> user = db.Users.ToList();

           var AllServices = from usr in user
                              join log in logins on usr.LoginId equals log.LoginId into table1
                              from log in table1.ToList()
                           

                              select new joins { logins=log,users=usr};


            var auth = AllServices.Where(x => x.logins.UserName == UserName && x.logins.Password == Password);
        
           var id=auth.SingleOrDefault();
            var auth1 = _context.Logins.Where(x => x.UserName == UserName && x.Password == Password).SingleOrDefault();

            if (auth1 != null)
            {


                switch (auth1.RoleId)
                {
                    

                    case 3:
                        {

                            HttpContext.Session.SetInt32("id", id.users.UserId);
                            return RedirectToAction("Index", "Admin");
                        }
                    case 4:
                        {

                            HttpContext.Session.SetInt32("id", id.users.UserId);
                            return RedirectToAction("Index", "employee");
                        }

                    case 5:
                        {

                            HttpContext.Session.SetInt32("id", id.users.UserId);
                            return RedirectToAction("Index", "Home");
                        }

                }




            }
            return View();
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Login", "LoginAndSessionController");

        }

    }
}
