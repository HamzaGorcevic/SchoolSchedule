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





                if (lessonToCheck.StartTime <= existingLesson.StartTime && lessonToCheck.EndTime >= existingLesson.StartTime && lessonToCheck.EndTime <= existingLesson.EndTime)
                {
                    return false;
                }
                if (lessonToCheck.StartTime >= existingLesson.StartTime && lessonToCheck.StartTime <= existingLesson.EndTime)
                {

                    return false;
                }
                if (lessonToCheck.StartTime >= lessonToCheck.EndTime && lessonToCheck.EndTime <= existingLesson.EndTime)
                {

                    return false;
                }
                if (lessonToCheck.StartTime > TimeSpan.FromHours(12) && existingLesson.EndTime < TimeSpan.FromHours(12) && lessonToCheck.EndTime <= existingLesson.StartTime && lessonToCheck.EndTime <= existingLesson.EndTime)
                {

                    return false;
                }
                if (lessonToCheck.StartTime > TimeSpan.FromHours(12) && lessonToCheck.EndTime >= existingLesson.EndTime && lessonToCheck.EndTime < TimeSpan.FromHours(12))
                {

                    return false;
                }
                if (existingLesson.EndTime <= lessonToCheck.EndTime && lessonToCheck.StartTime <= existingLesson.EndTime)
                {

                    return false;
                }

            }

            return true;
        }

    }

}
