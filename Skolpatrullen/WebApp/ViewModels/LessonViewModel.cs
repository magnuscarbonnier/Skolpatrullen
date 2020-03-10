using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database.Models;

namespace WebApp.ViewModels
{
    public class LessonViewModel
    {
        public int id { get; set; }
        public string text { get; set; }
        public string start_date { get; set; }
        public string end_date { get; set; }
        public string course { get; set; }

        public static explicit operator LessonViewModel(Lesson lesson)
        {
            return new LessonViewModel
            {
                id = lesson.Id,
                text = lesson.Name,
                start_date = lesson.StartDate.ToString("yyyy-MM-dd HH:mm"),
                end_date = lesson.EndDate.ToString("yyyy-MM-dd HH:mm"),
                course = lesson.Course.Name
            };
        }

        public static explicit operator Lesson(LessonViewModel lessonVM)
        {
            return new Lesson
            {
                Id = lessonVM.id,
                Name = lessonVM.text,
                StartDate = DateTime.Parse(lessonVM.start_date, System.Globalization.CultureInfo.InvariantCulture),
                EndDate = DateTime.Parse(lessonVM.end_date, System.Globalization.CultureInfo.InvariantCulture)
            };
        }
    }
}
