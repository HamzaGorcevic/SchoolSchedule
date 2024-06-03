using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace SchoolSchedule.Views
{
    public partial class AttendanceForm : UserControl
    {
        private int lessonId;

        public class StudentWrapper : Student, INotifyPropertyChanged
        {
            private bool isPresent;
            public bool IsPresent
            {
                get { return isPresent; }
                set
                {
                    if (isPresent != value)
                    {
                        isPresent = value;
                        OnPropertyChanged();
                    }
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private school_scheduleEntities context = new school_scheduleEntities();

        public ObservableCollection<StudentWrapper> Students { get; set; }

        public AttendanceForm(int lessonId)
        {
            InitializeComponent();
            this.lessonId = lessonId;
            LoadStudents();
            DataContext = this;
        }

        private void LoadStudents()
        {
            var studentsFromDb = context.Students.ToList();
            Students = new ObservableCollection<StudentWrapper>(
                studentsFromDb.Select(s => new StudentWrapper
                {
                    ID = s.ID,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    Grade = s.Grade,
                    IsPresent = context.Attendences.Any(att => att.StudentId == s.ID && att.lessonId == this.lessonId)
                })
            );

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            foreach (var student in Students)
            {
                var attendance = context.Attendences.FirstOrDefault(a => a.StudentId == student.ID && a.lessonId == this.lessonId);
                if (student.IsPresent)
                {
                    if (attendance == null)
                    {
                        Attendence newAttendance = new Attendence
                        {
                            lessonId = this.lessonId,
                            StudentId = student.ID
                        };
                        context.Attendences.Add(newAttendance);
                    }
                }
                else
                {
                    if (attendance != null)
                    {
                        context.Attendences.Remove(attendance);
                    }
                }
            }

            context.SaveChanges();
            MessageBox.Show("Attendance updated successfully.");
        }
    }
}
