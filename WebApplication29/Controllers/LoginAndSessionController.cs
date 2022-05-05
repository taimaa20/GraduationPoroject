using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NToastNotify;
using WebApplication29.Models;

namespace WebApplication29.Controllers
{
    public class LoginAndSessionController : Controller
    {
        private readonly HomeServicesNewContext _context;
        private IWebHostEnvironment _hostEnvironment;
        private readonly IToastNotification _toastNotification;
        public LoginAndSessionController(HomeServicesNewContext context,IWebHostEnvironment hostEnvironment, IToastNotification toastNotification)
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
        
           var id=auth.FirstOrDefault();
            var auth1 = _context.Logins.Where(x => x.UserName == UserName && x.Password == Password).SingleOrDefault();

            if (auth1 != null)
            {


                switch (auth1.RoleId)
                {
                    

                    case 4:
                        {

                            HttpContext.Session.SetInt32("id", id.users.UserId);
                            return RedirectToAction("Index", "Admin");
                        }
                    case 3:
                        {

                            HttpContext.Session.SetInt32("id", id.users.UserId);
                            return RedirectToAction("Index", "employee");
                        }

                    case 5:
                        {

                            HttpContext.Session.SetInt32("id", id.users.UserId);
                            return RedirectToAction("Index", "HomeIndex");
                        }

                }

                


            }
            _toastNotification.AddErrorToastMessage("the username or password is incorrect");
            return View();
        }
        
        public IActionResult Regstration()
        {
            ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleId");
            return View();
        }

        // POST: Logins/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Regstration([Bind("LoginId,UserName,Password,ConfimPassword,RoleId")] Login login, string FirstName, string LastName, string Gender, string Email, string PhoneNumber, string Adress, string City, DateTime BirthDate, string UserImage, IFormFile ImageFile)
        {
            if (login.RoleId != null || ModelState.IsValid)
            {
                User user = new User();
              
                    login.RoleId = 5;

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
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Login", "LoginAndSession");

        }

    }
}
