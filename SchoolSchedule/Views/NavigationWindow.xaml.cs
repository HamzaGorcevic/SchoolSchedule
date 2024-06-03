using System.Windows;

namespace SchoolSchedule.Views
{
    public partial class NavigationWindow : Window
    {


        int lessonId = 0;
        public NavigationWindow(int lessonId)
        {
            this.lessonId = lessonId;
            InitializeComponent();
            ShowAttendances();
        }

        private void AttendancesButton_Click(object sender, RoutedEventArgs e)
        {
            ShowAttendances();
        }

        private void AttendanceInfoButton_Click(object sender, RoutedEventArgs e)
        {
            ShowAttendanceInfo();
        }

        private void ShowAttendances()
        {
            ContentArea.Content = new AttendanceForm(lessonId);
        }

        private void ShowAttendanceInfo()
        {
            ContentArea.Content = new AttendenceInfo();
        }
    }
}