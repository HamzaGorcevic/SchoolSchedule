using System;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using BCrypt.Net;
namespace SchoolSchedule.Services
{
    public class AuthService
    {
        private static AuthService instance;
        private bool isLoggedIn = false;
        private string currentUser;
        public int TeacherId { get; set; }         

        public event Action OnLoginStatsChanged;
        private AuthService() { }

        public static AuthService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AuthService();
                }
                return instance;
            }
        }

        public bool IsLoggedIn
        {
            get { return isLoggedIn; }
            private set { isLoggedIn = value; OnLoginStatsChanged?.Invoke(); }
        }

        public bool Login(string username, string password)
        {
            using (var context = new school_scheduleEntities())
            {
                var teacher = context.Teachers.FirstOrDefault(user => user.FirstName == username);

                if (teacher != null && BCrypt.Net.BCrypt.Verify(password, teacher.password))
                {
                    TeacherId = teacher.ID;
                    IsLoggedIn = true;
                    currentUser = username;
                }
                else
                {
                    IsLoggedIn = false;
                }
            }

            return IsLoggedIn;
        }

        public bool Register(string firstName,string lastName, string password) {
            
            using (var context = new school_scheduleEntities())
            {
                var teacher = context.Teachers.FirstOrDefault((t) => t.FirstName == firstName && t.LastName == lastName);
                if (teacher != null)
                {
                    return false;
                }
                var newTeacher = new Teacher
                {
                    FirstName = firstName,
                    LastName = lastName,
                    password = HashPassword(password)
                };
                context.Teachers.Add(newTeacher);
                context.SaveChanges();
            }

            return true;
        }
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public void Logout()
        {
            IsLoggedIn = false;
            currentUser = null;
        }

        public string GetCurrentLoggedInUser()
        {
            return IsLoggedIn ? currentUser : null;
        }
    }
}
