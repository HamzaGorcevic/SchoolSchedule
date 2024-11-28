using SchoolSchedule.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace SchoolSchedule.Views { 

    public partial class LessonForm : Window, INotifyPropertyChanged
    {
        private Lesson lesson;
        private school_scheduleEntities context = new school_scheduleEntities();
        private bool isValidTimeInput = true;
        public ObservableCollection<int> HourOptions { get; set; }
        public ObservableCollection<int> MinuteOptions { get; set; }


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
                Subject = new Subject(),
                StartTimeHour = 0, // Default to "00"
                StartTimeMinute = 0, // Default to "00"
                EndTimeHour = 0, // Default to "00"
                EndTimeMinute = 0 // Default to "00"
            };
            HourOptions = new ObservableCollection<int>(Enumerable.Range(0, 24)); // 0-23 hours
            MinuteOptions = new ObservableCollection<int>(Enumerable.Range(0, 60).Where(x => x % 5 == 0));

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

        private void OpenBothDropDowns()
        {
  
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

        // This method will update the Lesson StartTime and EndTime based on ComboBox selection
        private void Submit_Click(object sender, RoutedEventArgs e)
        {


            // Ensure StartTime and EndTime are set correctly from the ComboBox values
            if (Lesson.StartTimeHour.HasValue && Lesson.StartTimeMinute.HasValue &&
                Lesson.EndTimeHour.HasValue && Lesson.EndTimeMinute.HasValue &&
                !string.IsNullOrEmpty(Lesson.Subject.SubjectName))
            {
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Please fill in all details.");
            }
        }

        private void TimeComboBox_SelectionChanged(object sender, RoutedEventArgs e)
        {

            try
            {
                // Get the selected hour and minute values
                string startHourString = StartHoursComboBox.SelectedItem as string;
                string startMinuteString = StartMinuteComboBox.SelectedItem as string;
                string endHourString = EndHourComboBox.SelectedItem as string;
                string endMinuteString = EndMinuteComboBox.SelectedItem as string;

                // Ensure no field is empty
                if (!string.IsNullOrEmpty(startHourString) && !string.IsNullOrEmpty(startMinuteString) &&
                    !string.IsNullOrEmpty(endHourString) && !string.IsNullOrEmpty(endMinuteString))
                {
                    // Convert them to integers and then to TimeSpan
                    int startHour = int.Parse(startHourString);
                    int startMinute = int.Parse(startMinuteString);
                    int endHour = int.Parse(endHourString);
                    int endMinute = int.Parse(endMinuteString);

                    // Assign the values to the lesson object
                    Lesson.StartTimeHour = startHour;
                    Lesson.StartTimeMinute = startMinute;
                    Lesson.EndTimeHour = endHour;
                    Lesson.EndTimeMinute = endMinute;
                    Lesson.StartTime = new TimeSpan(startHour, startMinute, 0);
                    Lesson.EndTime = new TimeSpan(endHour, endMinute, 0);
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Please enter valid hours and minutes.");
            }
        }



        private void TimeTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            // This is now unnecessary, as we are using ComboBox for time selection

        }

        private void EndMinuteComboBox_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
        
            EndHourComboBox.IsDropDownOpen = true;
            EndMinuteComboBox.IsDropDownOpen = true;

        }

        private void EndHourComboBox_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            EndHourComboBox.IsDropDownOpen = true;
            EndMinuteComboBox.IsDropDownOpen = true;
        }

        private void StartMinuteComboBox_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            StartHoursComboBox.IsDropDownOpen = true;
            StartMinuteComboBox.IsDropDownOpen = true;
        }

        private void StartHoursComboBox_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            StartHoursComboBox.IsDropDownOpen = true;
            StartMinuteComboBox.IsDropDownOpen = true;

        }
    }
}
