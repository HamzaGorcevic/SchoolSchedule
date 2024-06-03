using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using SchoolSchedule.Services;

namespace SchoolSchedule.Views
{
    /// <summary>
    /// Interaction logic for AttendenceInfo.xaml
    /// </summary>
    public partial class AttendenceInfo : UserControl
    {
        private school_scheduleEntities context = new school_scheduleEntities();

        public ObservableCollection<Student> Students { get; set; }

        private int teacherID;

        public int TeacherID
        {
            get { return teacherID; }
            set { teacherID = value; }
        }


        public AttendenceInfo()
        {
            InitializeComponent();
            LoadStudents();
            DataContext = this;
        }

        private void LoadStudents()
        {
            TeacherID = AuthService.Instance.TeacherId;
            var studentsFromDb = context.Students.ToList();
            var attendences = context.Attendences.ToList();

            int nOfLessons = context.Lessons.Where((el)=>el.Subject.TeacherID == teacherID).Count();
            studentsFromDb.ForEach(student => {



                int count = attendences.Where((el)=>el.StudentId == student.ID && el.Lesson.Subject.TeacherID == TeacherID).Count();
                student.Absences =nOfLessons - count;

            });

          
            Students = new ObservableCollection<Student>(studentsFromDb);

            context.SaveChanges();

        }
    }
}