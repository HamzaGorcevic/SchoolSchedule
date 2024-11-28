using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SchoolSchedule.ModelsManager
{
    public class UiManager
    {
        private MainWindow mainWindow;
        private int counter;

        public UiManager(MainWindow window, int counter = 0)
        {
            mainWindow = window;
            this.counter = counter;
        }

        public void GenerateUi(string day, Dictionary<string, ObservableCollection<Lesson>> scheduleDayOfWeeks, bool isLoggedIn, int teacherId, int counter = 0)
        {
            this.counter = counter;

            Border borderContainer = new Border
            {
                Padding = new Thickness(6),
                CornerRadius = new CornerRadius(15),
                Margin = new Thickness(2),
                Height = 350,
                Width = 240
            };
            Grid.SetColumn(borderContainer, this.counter);
            Grid.SetRow(borderContainer, 1);

            StackPanel spDayOfWeek = new StackPanel();
            Border borderDayOfWeek = new Border
            {
                Margin = new Thickness(0, 0, 0, 6),
                Background = Brushes.LightBlue,
                CornerRadius = new CornerRadius(8),
                Height = 40
            };

            TextBlock tbDayOfWeek = new TextBlock
            {
                FontWeight = FontWeights.Bold,
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = 16,
                HorizontalAlignment = HorizontalAlignment.Center,
                Text = day,
                Foreground = Brushes.DarkSlateGray,
            };

            borderDayOfWeek.Child = tbDayOfWeek;
            spDayOfWeek.Children.Add(borderDayOfWeek);

            Border borderLessons = new Border
            {
                VerticalAlignment = VerticalAlignment.Stretch,
                Height = 280,
                Background = Brushes.LightBlue,
                CornerRadius = new CornerRadius(12),
                Padding = new Thickness(6),
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
                    Padding = new Thickness(3),
                    Margin = new Thickness(0, 0, 0, 6),
                    CornerRadius = new CornerRadius(8),
                    Background = Brushes.White,
                };

                StackPanel spSingleLessonAndEdit = new StackPanel
                {
                    Orientation = Orientation.Vertical,
                };

                // Display Teacher's Name
                TextBlock tbTeacherName = new TextBlock
                {
                    FontWeight = FontWeights.Light,
                    FontSize = 13,
                    Foreground = Brushes.Gray,
                    Text = "Prof.  " + lesson.Subject?.Teacher?.FirstName ?? "Unknown Teacher",  // Ensure null safety
                    Visibility =  lesson.Subject.TeacherID != teacherId ? Visibility.Visible : Visibility.Collapsed,

                };

                // Lesson details
                TextBlock tbSingleLesson = new TextBlock
                {
                    FontWeight = FontWeights.Medium,
                    FontSize = 14,
                    Foreground = Brushes.DarkSlateGray,
                    TextWrapping = TextWrapping.NoWrap,
                    Text = lesson.ToString(),
                };

                // Actions (edit, delete, attendance)
                DockPanel spActions = new DockPanel
                {
                    Visibility = isLoggedIn && lesson.Subject.TeacherID == teacherId ? Visibility.Visible : Visibility.Collapsed,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                };

                Button attendenceSingleLesson = new Button
                {
                    Width = 24,
                    Height = 24,
                    Background = Brushes.Transparent,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Top,
                    Tag = lesson.ID,
                    BorderBrush = Brushes.Transparent,
                    Content = new Image
                    {
                        Source = new BitmapImage(new Uri("C:\\Users\\gorce\\source\\repos\\SchoolSchedule\\SchoolSchedule\\Icons\\attendence.ico")),
                        Width = 20,
                        Height = 20
                    }
                };

                Button deleteLesson = new Button
                {
                    Width = 24,
                    Height = 24,
                    Background = Brushes.Transparent,
                    HorizontalAlignment = HorizontalAlignment.Right,
                    VerticalAlignment = VerticalAlignment.Top,
                    Tag = lesson.ID,
                    BorderBrush = Brushes.Transparent,
                    Content = new Image
                    {
                        Source = new BitmapImage(new Uri("C:\\Users\\gorce\\source\\repos\\SchoolSchedule\\SchoolSchedule\\Icons\\delete.ico")),
                        Width = 20,
                        Height = 20
                    }
                };

                Button editSingleLesson = new Button
                {
                    Width = 24,
                    Height = 24,
                    Background = Brushes.Transparent,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Tag = lesson.ID,
                    BorderBrush = Brushes.Transparent,
                    Content = new Image
                    {
                        Source = new BitmapImage(new Uri("C:\\Users\\gorce\\source\\repos\\SchoolSchedule\\SchoolSchedule\\Icons\\edit.ico")),
                        Width = 20,
                        Height = 20
                    }
                };

                deleteLesson.Click += mainWindow.DeleteLesson_Click;
                editSingleLesson.Click += mainWindow.ShowForm_Click;
                attendenceSingleLesson.Click += mainWindow.AttendenceSingleLesson_Click;

                spActions.Children.Add(editSingleLesson);
                spActions.Children.Add(attendenceSingleLesson);
                spActions.Children.Add(deleteLesson);

                spSingleLessonAndEdit.Children.Add(spActions);
                spSingleLessonAndEdit.Children.Add(tbSingleLesson);
                spSingleLessonAndEdit.Children.Add(tbTeacherName); // Add teacher name
                borderSingleLesson.Child = spSingleLessonAndEdit;
                spLessons.Children.Add(borderSingleLesson);
            }

            Border borderBtn = new Border
            {
                Margin = new Thickness(6),
                Background = Brushes.DarkSlateGray,
                CornerRadius = new CornerRadius(12),
                Height = 40,
                Visibility = isLoggedIn ? Visibility.Visible : Visibility.Collapsed,
                Cursor = Cursors.Hand
            };
            Button btnAdd = new Button
            {
                Foreground = Brushes.White,
                FontWeight = FontWeights.Bold,
                Padding = new Thickness(3),
                Content = "Add lesson",
                FontSize = 14,
                BorderBrush = null,
                HorizontalAlignment = HorizontalAlignment.Center,
                Background = Brushes.DarkSlateGray,
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
