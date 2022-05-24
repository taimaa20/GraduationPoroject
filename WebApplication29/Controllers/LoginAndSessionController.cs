using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NToastNotify;
using System.Linq;
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
                            var fisrtname = user.Where(x => x.UserId == id.users.UserId).Select(x => x.FirstName).FirstOrDefault();
                            var lastname = user.Where(x => x.UserId == id.users.UserId).Select(x => x.LastName).FirstOrDefault();

                            HttpContext.Session.SetInt32("id", id.users.UserId);
                            _toastNotification.AddSuccessToastMessage("welcome to the system "+fisrtname.ToString()+" "+ lastname.ToString());
                            return RedirectToAction("HomeIndex", "Home");
                        }

                }

                


            }
            _toastNotification.AddErrorToastMessage("the username or password is incorrect");
            return View();
        }
        
      public IActionResult Regstration()
        {
           
            return View();
        }

        // POST: Logins/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        public IActionResult Regstration1(string UserName ,string Password,string ConfirmPassword, Login login, string FirstName, string LastName, string Gender, string Email, string PhoneNumber, string Adress, string City, DateTime BirthDate, string UserImage, IFormFile ImageFile , User user)
        {


            List<Login> loginss = db.Logins.ToList();
          var name = loginss.Where(x => x.UserName == UserName).ToList();
            if (Password != ConfirmPassword)
            {
                _toastNotification.AddErrorToastMessage("The confirm password is wrong");
                return RedirectToAction("Regstration", "LoginAndSession");
            }
            else
            {
                if (name.Count() == 0)
                {
                    login.RoleId = 5;
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
                      
                    }
                    _context.Add(user);
                    _context.SaveChangesAsync();
                    _toastNotification.AddSuccessToastMessage("welcome to home service website");
                    return RedirectToAction("login");
                }
                else
                {
                    _toastNotification.AddErrorToastMessage("The username already taken");
                    return RedirectToAction("Regstration", "LoginAndSession");

                }
            }
            return RedirectToAction("login");
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Login", "LoginAndSession");

        }

    }
}
