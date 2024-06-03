using SchoolSchedule.Services;
using System.Windows;

namespace SchoolSchedule.Views
{
    public partial class LoginForm : Window
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private string username;

        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        private string password;

        public string Password
        {
            get { return password; }

            set { password = value; }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            username = UsernameTextBox.Text;
            password = PasswordBox.Password;

            bool loggedIn = AuthService.Instance.Login(username, password);
            if (loggedIn)
            {
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid username or password.");
            }
        }
    }
}
