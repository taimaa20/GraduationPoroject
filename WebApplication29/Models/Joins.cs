using WebApplication29.Models;

namespace WebApplication29.Models
{
    public class joins
    {
        public Role? roles { get; set; }
        public User? users { get; set; }
        public Login? logins { get; set; }
        public Salary? salaries { get; set; }
        public Service? services { get; set; }
        public UserService? userService { get; set; }
        public Review ?review { get; set; }

        public Message ?message { get; set; }
        public Payment ? payment { get; set; }

        public Category? categories { get; set; }

    }
}
