using SchoolSchedule.Services;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace SchoolSchedule.Views
{
    public partial class LessonForm : Window, INotifyPropertyChanged
    {
        private Lesson lesson;
        private school_scheduleEntities context = new school_scheduleEntities();
        private bool isValidTimeInput = true;
        public Lesson Lesson
        {
            get { return lesson; }
            set
            {
                lesson = value;
                OnPropertyChanged();
            }
        }

        public LessonForm()
        {
            InitializeComponent();
            Lesson = new Lesson
            {
                Subject = new Subject()
            };
            this.DataContext = this;
        }

        public void SetValues(Lesson argLesson)
        {
            Lesson = argLesson;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(Lesson.Subject.SubjectName) &&
                Lesson.StartTime.HasValue &&
                Lesson.EndTime.HasValue)
            {
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Please fill in all details.");
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (Lesson.ID == 0)
            {
                MessageBox.Show("This lesson cannot be deleted because it is not saved yet.");
                return;
            }

            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this lesson?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                var lessonToDelete = context.Lessons.Find(Lesson.ID);
                if (lessonToDelete != null)
                {
                    context.Lessons.Remove(lessonToDelete);
                    context.SaveChanges();
                    MessageBox.Show("Lesson deleted successfully.");
                    this.DialogResult = true;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Lesson not found in the database.");
                }
            }
        }

        private TimeSpan ConvertToTimeSpan(string input)
        {
            if (int.TryParse(input, out int hour))
            {
                return new TimeSpan(hour, 0, 0);
            }

            if (TimeSpan.TryParse(input, out TimeSpan time))
            {
                return time;
            }

            throw new FormatException("Invalid time format");
        }

        private void TimeTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!isValidTimeInput)
            {
                isValidTimeInput = true; // Reset the flag
                return; // Skip further processing if the previous input was invalid
            }

            if (sender is TextBox textBox)
            {
                try
                {
                    TimeSpan time = ConvertToTimeSpan(textBox.Text);

                    if (time < TimeSpan.FromHours(0) || time >= TimeSpan.FromHours(24))
                    {
                        throw new ArgumentOutOfRangeException("The time must be between 00:00 and 23:59.");
                    }

                    textBox.Text = time.ToString(@"hh\:mm");

                    if (textBox.Name == "StartTimeTextBox")
                    {
                        Lesson.StartTime = time;
                    }
                    else if (textBox.Name == "EndTimeTextBox")
                    {
                        Lesson.EndTime = time;
                    }
                }
                catch (FormatException)
                {
                    MessageBox.Show("Please enter a valid time in HH:mm format.");
                    isValidTimeInput = false;
                    textBox.Focus();
                }
                catch (ArgumentOutOfRangeException)
                {
                    MessageBox.Show("Please enter a valid time between 00:00 and 23:59.");
                    isValidTimeInput = false;
                    textBox.Focus();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An unexpected error occurred: {ex.Message}");
                    isValidTimeInput = false;
                    textBox.Focus();
                }
            }
        }

    }
}
