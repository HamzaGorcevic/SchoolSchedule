using System.Linq;

namespace SchoolSchedule.Services
{
    public class AuthService
    {
        private static AuthService instance;
        private bool isLoggedIn = false;
        private string currentUser;
        public int TeacherId { get; set; }         

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
            private set { isLoggedIn = value; }
        }

        public bool Login(string username, string password)
        {
            using (var context = new school_scheduleEntities())
            {

                var teacher = context.Teachers.FirstOrDefault((user) => user.FirstName == username && user.password == password);
                if(teacher != null)
                {
                    TeacherId = teacher.ID;

                    IsLoggedIn = true;
                }
                if (IsLoggedIn)
                {
                    IsLoggedIn = true;
                    currentUser = username;

                }
                return IsLoggedIn;
            }
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
