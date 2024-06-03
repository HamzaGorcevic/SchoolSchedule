using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using System.Collections.ObjectModel;

namespace SchoolSchedule.ModelsManager
{
    public class UiManager
    {

        private MainWindow mainWindow;

        private int counter;
        public UiManager(MainWindow window, int counter=0)
        {
            mainWindow = window;
            this.counter = counter;
        }



        public void GenerateUi(string day, Dictionary<string, ObservableCollection<Lesson>> scheduleDayOfWeeks, bool isLoggedIn,int teacherId, int counter=0)
        {



            this.counter = counter;
            Border borderContainer = new Border
            {
                Padding = new Thickness(10),
                CornerRadius = new CornerRadius(20),
                Margin = new Thickness(4),
                Height = 435,

            };
            Grid.SetColumn(borderContainer, this.counter);
            Grid.SetRow(borderContainer, 1);


            StackPanel spDayOfWeek = new StackPanel();
            Border borderDayOfWeek = new Border
            {
                Margin = new Thickness(0, 0, 0, 10),
                Background = Brushes.LightBlue,
                CornerRadius = new CornerRadius(10),
                Height = 52
            };

            TextBlock tbDayOfWeek = new TextBlock
            {
                FontWeight = FontWeights.Bold,
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = 20,
                HorizontalAlignment = HorizontalAlignment.Center,
                Text = day.ToString(),
                Foreground = Brushes.DarkSlateGray,

            };

            borderDayOfWeek.Child = tbDayOfWeek;
            spDayOfWeek.Children.Add(borderDayOfWeek);

            Border borderLessons = new Border
            {
                VerticalAlignment = VerticalAlignment.Stretch,
                Height = 350,
                Background = Brushes.LightBlue,
                CornerRadius = new CornerRadius(14),
                Padding = new Thickness(8),
            };

            ScrollViewer scrollViewer = new ScrollViewer
            {
                Padding = new Thickness(1),
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                Background = Brushes.LightBlue,
            };

            StackPanel spLessons = new StackPanel
            {
                Orientation = Orientation.Vertical,
            };

            foreach (Lesson lesson in scheduleDayOfWeeks[day])
            {
                Border borderSingleLesson = new Border
                {
                    Padding = new Thickness(4),
                    Margin = new Thickness(0, 0, 0, 10),
                    CornerRadius = new CornerRadius(10),
                    Background = Brushes.White
                };



                StackPanel spSingleLessonAndEdit = new StackPanel
                {
                    Orientation = Orientation.Vertical,


                };

                TextBlock tbSingleLesson = new TextBlock
                {
                    FontWeight = FontWeights.Medium,
                    FontSize = 16,
                    Foreground = Brushes.DarkTurquoise,
                    TextWrapping = TextWrapping.NoWrap,
                    Text = lesson.ToString()
                };


                // actions
                DockPanel spActions = new DockPanel
                {
                    Visibility = isLoggedIn && lesson.Subject.TeacherID == teacherId ? Visibility.Visible : Visibility.Collapsed,
                    HorizontalAlignment = HorizontalAlignment.Stretch,

                };

                Button attendenceSingleLesson = new Button
                {
                    Width = 30,
                    Height = 31,
                    Background = Brushes.Transparent,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Top,
                    Tag = lesson.ID,
                    BorderBrush = Brushes.Transparent,

                };


                Image attendenceIcon = new Image
                {
                    Source = new BitmapImage(new Uri("C:\\Users\\gorce\\source\\repos\\SchoolSchedule\\SchoolSchedule\\Icons\\attendence.ico")),
                    Width = 25,
                    Height = 30
                };


                Image deleteIcon = new Image
                {
                    Source = new BitmapImage(new Uri("C:\\Users\\gorce\\source\\repos\\SchoolSchedule\\SchoolSchedule\\Icons\\delete.ico")),
                    Width = 25,
                    Height = 30
                };

                Button deleteLesson = new Button
                {
                    Width = 26,
                    Height = 31,
                    Background = Brushes.Transparent,
                    HorizontalAlignment = HorizontalAlignment.Right,
                    VerticalAlignment = VerticalAlignment.Top,
                    DataContext = lesson.ID,
                    BorderBrush = Brushes.Transparent,
                    Tag = lesson.ID,
                    Content = deleteIcon,

                };

                attendenceSingleLesson.Content = attendenceIcon;

                Button editSingleLesson = new Button
                {
                    Width = 26,
                    Height = 31,
                    Background = Brushes.Transparent,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    DataContext = lesson.ID,
                    BorderBrush = Brushes.Transparent,

                };

                Image editIcon = new Image
                {
                    Source = new BitmapImage(new Uri("C:\\Users\\gorce\\source\\repos\\SchoolSchedule\\SchoolSchedule\\Icons\\edit.ico")),
                    Width = 25,
                    Height = 30
                };

                deleteLesson.Click += mainWindow.DeleteLesson_Click;
                editSingleLesson.Content = editIcon;
                editSingleLesson.Click += mainWindow.ShowForm_Click;
                attendenceSingleLesson.Click += mainWindow.AttendenceSingleLesson_Click; ;


                spActions.Children.Add(editSingleLesson);
                spActions.Children.Add(attendenceSingleLesson);
                spActions.Children.Add(deleteLesson);

                spSingleLessonAndEdit.Children.Add(spActions);
                spSingleLessonAndEdit.Children.Add(tbSingleLesson);
                borderSingleLesson.Child = spSingleLessonAndEdit;
                spLessons.Children.Add(borderSingleLesson);
            }

            Border borderBtn = new Border
            {
                Margin = new Thickness(10),
                Background = Brushes.DarkTurquoise,
                CornerRadius = new CornerRadius(14),
            };
            Button btnAdd = new Button
            {
                Foreground = Brushes.White,
                FontWeight = FontWeights.Bold,
                Padding = new Thickness(5),
                Content = "Add lesson",
                FontSize = 17,
                BorderBrush = null,
                HorizontalAlignment = HorizontalAlignment.Center,
                Background = null,
                Tag = day,
                Visibility = isLoggedIn ? Visibility.Visible : Visibility.Collapsed,
            };

            btnAdd.Click += mainWindow.ShowForm_Click;

            borderBtn.Child = btnAdd;
            spLessons.Children.Add(borderBtn);

            scrollViewer.Content = spLessons;
            borderLessons.Child = scrollViewer;
            spDayOfWeek.Children.Add(borderLessons);

            borderContainer.Child = spDayOfWeek;

            mainWindow.gridSchedule.Children.Add(borderContainer);



        }
    }
}
