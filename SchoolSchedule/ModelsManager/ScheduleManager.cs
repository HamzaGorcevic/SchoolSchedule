using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SchoolSchedule.ModelsManager
{
    public class ScheduleManager
    {
        private school_scheduleEntities context;
        public Schedule Schedule { get; private set; }
        public Dictionary<string, ObservableCollection<Lesson>> ScheduleDayOfWeeks { get; private set; }

        public ScheduleManager(school_scheduleEntities dbContext)
        {
            context = dbContext;
            ScheduleDayOfWeeks = new Dictionary<string, ObservableCollection<Lesson>>
        {
            { "Monday", new ObservableCollection<Lesson>() },
            { "Tuesday", new ObservableCollection<Lesson>() },
            { "Wednesday", new ObservableCollection<Lesson>() },
            { "Thursday", new ObservableCollection<Lesson>() },
            { "Friday", new ObservableCollection<Lesson>() }
        };
        }

        public void LoadScheduleFromDatabase()
        {
            try
            {
                Schedule = context.Schedules.FirstOrDefault();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void PopulateSchedule(int teacherId)
        {
            foreach (var key in ScheduleDayOfWeeks.Keys.ToList())
            {
                ScheduleDayOfWeeks[key].Clear();
            }

            foreach (Lesson lesson in Schedule.GetWeek(teacherId))
            {
                if (ScheduleDayOfWeeks.ContainsKey(lesson.DayOfWeek))
                {
                    ScheduleDayOfWeeks[lesson.DayOfWeek].Add(lesson);
                }
            }
        }

        public bool CheckIfAvailable(Lesson lessonToCheck)
        {
            var lessonsInDay = ScheduleDayOfWeeks[lessonToCheck.DayOfWeek];

            foreach (var existingLesson in lessonsInDay)
            {
                if (lessonToCheck.ID == existingLesson.ID && lessonToCheck.ID != 0)
                {
                    continue;
                }

                if (lessonToCheck.StartTimeHour * 60 + lessonToCheck.StartTimeMinute <= existingLesson.StartTimeHour * 60 + existingLesson.StartTimeMinute &&
                    lessonToCheck.EndTimeHour * 60 + lessonToCheck.EndTimeMinute >= existingLesson.StartTimeHour * 60 + existingLesson.StartTimeMinute &&
                    lessonToCheck.EndTimeHour * 60 + lessonToCheck.EndTimeMinute <= existingLesson.EndTimeHour * 60 + existingLesson.EndTimeMinute)
                {
                    return false;
                }

                if (lessonToCheck.StartTimeHour * 60 + lessonToCheck.StartTimeMinute >= existingLesson.StartTimeHour * 60 + existingLesson.StartTimeMinute &&
                    lessonToCheck.StartTimeHour * 60 + lessonToCheck.StartTimeMinute <= existingLesson.EndTimeHour * 60 + existingLesson.EndTimeMinute)
                {
                    return false;
                }

                if (lessonToCheck.StartTimeHour * 60 + lessonToCheck.StartTimeMinute >= lessonToCheck.EndTimeHour * 60 + lessonToCheck.EndTimeMinute &&
                    lessonToCheck.EndTimeHour * 60 + lessonToCheck.EndTimeMinute <= existingLesson.EndTimeHour * 60 + existingLesson.EndTimeMinute)
                {
                    return false;
                }

                if (lessonToCheck.StartTimeHour * 60 + lessonToCheck.StartTimeMinute > 12 * 60 &&
                    existingLesson.EndTimeHour * 60 + existingLesson.EndTimeMinute < 12 * 60 &&
                    lessonToCheck.EndTimeHour * 60 + lessonToCheck.EndTimeMinute <= existingLesson.StartTimeHour * 60 + existingLesson.StartTimeMinute &&
                    lessonToCheck.EndTimeHour * 60 + lessonToCheck.EndTimeMinute <= existingLesson.EndTimeHour * 60 + existingLesson.EndTimeMinute)
                {
                    return false;
                }

                if (lessonToCheck.StartTimeHour * 60 + lessonToCheck.StartTimeMinute > 12 * 60 &&
                    lessonToCheck.EndTimeHour * 60 + lessonToCheck.EndTimeMinute >= existingLesson.EndTimeHour * 60 + existingLesson.EndTimeMinute &&
                    lessonToCheck.EndTimeHour * 60 + lessonToCheck.EndTimeMinute < 12 * 60)
                {
                    return false;
                }

                if (existingLesson.EndTimeHour * 60 + existingLesson.EndTimeMinute <= lessonToCheck.EndTimeHour * 60 + lessonToCheck.EndTimeMinute &&
                    lessonToCheck.StartTimeHour * 60 + lessonToCheck.StartTimeMinute <= existingLesson.EndTimeHour * 60 + existingLesson.EndTimeMinute)
                {
                    return false;
                }
            }

            return true;
        }


    }

}
