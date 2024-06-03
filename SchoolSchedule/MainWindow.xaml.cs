using SchoolSchedule.ModelsManager;
using SchoolSchedule.Services;
using SchoolSchedule.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Contexts;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SchoolSchedule
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public Schedule schedule;
        public bool isLoggedIn = false;

        private int teacherId = AuthService.Instance.TeacherId;

        private TaskCompletionSource<bool> gridLoaded = new TaskCompletionSource<bool>();

        int counter = 0;


        public int TeacherId
        {
            get { return teacherId; }
            set { teacherId = value; OnPropertyChanged(); }
        }
        public bool IsLoggedIn
        {
            get { return isLoggedIn; }
            set { isLoggedIn = value; OnPropertyChanged(); }
        }

        private ScheduleManager scheduleManager;
        private UiManager uiManager;
   

        private school_scheduleEntities context = new school_scheduleEntities();

        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindow()
        {
            InitializeComponent();
            scheduleManager = new ScheduleManager(context);
            scheduleManager.LoadScheduleFromDatabase();
            schedule = scheduleManager.Schedule;
            uiManager = new UiManager(this);


        }

        public void ShowDay(string choosenDay)
        {
            counter = 2;
            gridSchedule.Children.Clear();

            uiManager.GenerateUi(choosenDay,scheduleManager.ScheduleDayOfWeeks,isLoggedIn,teacherId,counter);

        }
        public void ShowWeek()
        {

            counter = 0;

            try
            {
                gridSchedule.Children.Clear();

                foreach (var key in scheduleManager.ScheduleDayOfWeeks.Keys.ToList())
                {
                    scheduleManager.ScheduleDayOfWeeks[key].Clear();
                }

                foreach (Lesson lesson in schedule.GetWeek(teacherId))
                {
                    if (scheduleManager.ScheduleDayOfWeeks.ContainsKey(lesson.DayOfWeek))
                    {
                        scheduleManager.ScheduleDayOfWeeks[lesson.DayOfWeek].Add(lesson);
                    }
                }


                foreach (var day in scheduleManager.ScheduleDayOfWeeks.Keys)
                {
                    uiManager.GenerateUi(day, scheduleManager.ScheduleDayOfWeeks,  isLoggedIn, teacherId,counter);
                    counter++;

                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"Message : {e.Message}");
            }

        }


      

        public void AttendenceSingleLesson_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button clickedButton)
            {
                if (clickedButton.Tag is int lessonId)
                {
                    NavigateToAttendancePage(lessonId);
                }
            }

        }

        public void DeleteLesson_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button clickedButton)
            {
                if (clickedButton.Tag is int lessonId)
                {
                    DeleteLesson(lessonId);
                }
            }
        }

        public void ShowForm_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button clickedButton)
            {
                if (clickedButton.Tag is string dayOfWeek)
                {
                    PostLesson(dayOfWeek);
                }
                else if (clickedButton.DataContext is int lessonId)
                {
                    UpdateLesson(lessonId);
                }

            }
        }

        private void NavigateToAttendancePage(int lessonid)
        {
            NavigationWindow attencedForm = new NavigationWindow(lessonid);
            attencedForm.Show();
        }

        private void PostLesson(string dayOfWeek)
        {

            LessonForm lessonForm = new LessonForm
            {
                Owner = this
            };
            if (lessonForm.ShowDialog() == true)
            {
                Attendence newAttendence = new Attendence();
                Lesson newLesson = lessonForm.Lesson;
                newLesson.Subject.TeacherID = teacherId;
                newAttendence.lessonId = newLesson.ID;
                newLesson.ScheduleId = schedule.ID;
                newLesson.DayOfWeek = dayOfWeek;

                if (scheduleManager.CheckIfAvailable(newLesson))
                {
                    context.Attendences.Add(newAttendence);
                    context.Lessons.Add(newLesson);
                    context.SaveChanges();

                    scheduleManager.ScheduleDayOfWeeks[dayOfWeek].Add(newLesson);
                    ShowWeek();
                }
                else
                {
                    MessageBox.Show("Ne mozete uneti to vreme!");
                }
            }
        }

       



        private void UpdateLesson(int lessonId)
        {
            var existingLesson = context.Lessons.FirstOrDefault(l => l.ID == lessonId);
            if (existingLesson == null) return;

            LessonForm lessonForm = new LessonForm
            {
                Owner = this
            };

            lessonForm.SetValues(existingLesson);



            if (lessonForm.ShowDialog() == true)
            {
                Lesson updatedLesson = lessonForm.Lesson;
                updatedLesson.Subject.TeacherID = teacherId;


                if (existingLesson != null && scheduleManager.CheckIfAvailable(existingLesson))
                {
                    existingLesson.Subject = updatedLesson.Subject;
                    existingLesson.StartTime = updatedLesson.StartTime;
                    existingLesson.EndTime = updatedLesson.EndTime;

                    context.SaveChanges();

                    var updatedLesson1 = scheduleManager.ScheduleDayOfWeeks[existingLesson.DayOfWeek].FirstOrDefault(l => l.ID == lessonId);

                    if (updatedLesson1 != null && scheduleManager.CheckIfAvailable(existingLesson))
                    {
                        updatedLesson1.Subject = updatedLesson.Subject;
                        updatedLesson1.StartTime = updatedLesson.StartTime;
                        updatedLesson1.EndTime = updatedLesson.EndTime;
                        MessageBox.Show(updatedLesson.ToString());

                        ShowWeek();
                    }
                }
                else
                {
                    MessageBox.Show("Ne mozete uneti to vreme!");
                }
            }
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {

            if (AuthService.Instance.IsLoggedIn)
            {
                IsLoggedIn = false;
                AuthService.Instance.Logout();
                TeacherId = 0;
                btnLogin.Content = "Login";
            }
            else
            {
                Image logoutIcon = new Image
                {
                    Source = new BitmapImage(new Uri("C:\\Users\\gorce\\source\\repos\\SchoolSchedule\\SchoolSchedule\\Icons\\logout.ico")),
                    Width = 20,
                    Height = 20
                };


                LoginForm loginForm = new LoginForm();

                if (loginForm.ShowDialog() == true)
                {
                    bool loginSuccess = AuthService.Instance.Login(loginForm.Username, loginForm.Password);
                    if (loginSuccess)
                    {
                        TeacherId = AuthService.Instance.TeacherId;

                        IsLoggedIn = true;
                        btnLogin.Content = logoutIcon;

                    }
                    else
                    {
                        MessageBox.Show("Login failed. Invalid credentials.");
                    }
                }
            }
        }



        private void DeleteLesson(int lessonId)
        {

            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this lesson?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                var attendances = context.Attendences.Where(a => a.lessonId == lessonId).ToList();

                foreach (var attendance in attendances)
                {
                    context.Attendences.Remove(attendance);
                }


                var lessonToDelete = context.Lessons.Find(lessonId);
                var subjectToDelete = context.Subjects.FirstOrDefault((el) => el.ID == lessonToDelete.SubjectID);
                if (lessonToDelete != null)
                {

                    context.Lessons.Remove(lessonToDelete);
                    context.Subjects.Remove(subjectToDelete);
                    context.SaveChanges();
                    OnPropertyChanged();
                    MessageBox.Show("Lesson delet" +
                        "ed successfully.");

                }
                else
                {
                    MessageBox.Show("Lesson not found in the database.");
                }
            }

        }


        public void RemoveLessonFromUI(Lesson lesson)
        {
            if (scheduleManager.ScheduleDayOfWeeks.ContainsKey(lesson.DayOfWeek))
            {
                var lessonToRemove = scheduleManager.ScheduleDayOfWeeks[lesson.DayOfWeek].FirstOrDefault(l => l.ID == lesson.ID);
                if (lessonToRemove != null)
                {
                    scheduleManager.ScheduleDayOfWeeks[lesson.DayOfWeek].Remove(lessonToRemove);
                }
            }
        }

        private async void cbDays_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbDays.SelectedItem is ComboBoxItem selectedItem)
            {

                if ((string)selectedItem.Content == "Week")
                {

                    await gridLoaded.Task;
                    if (gridSchedule != null)
                    {
                        ShowWeek();
                    }


                }
                else
                {
                    string selectedDay = selectedItem.Content.ToString();

                    ShowDay(selectedDay);
                }

            }
        }



        private void gridSchedule_Loaded(object sender, RoutedEventArgs e)
        {
            gridLoaded.TrySetResult(true);

        }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            cbDays.SelectedIndex = -1;

            cbDays.SelectedIndex = 0;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

