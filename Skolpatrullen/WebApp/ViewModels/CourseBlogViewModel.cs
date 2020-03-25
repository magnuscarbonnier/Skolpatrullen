using Database.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ViewModels
{
    public class CourseBlogViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime PublishDate { get; set; }
        public int UserId { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
        public User User { get; set; }
        public IEnumerable<CourseBlogPost> BlogPosts { get; set; }
        public static explicit operator CourseBlogPost(CourseBlogViewModel vm)
        {
            return new CourseBlogPost
            {
                Id = vm.Id,
                Title = vm.Title,
                Content = vm.Content,
                PublishDate = vm.PublishDate,
                UserId = vm.UserId,
                CourseId = vm.CourseId
            };
        }
        public static explicit operator CourseBlogViewModel(CourseBlogPost post)
        {
            return new CourseBlogViewModel
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content,
                PublishDate = post.PublishDate,
                UserId = post.UserId,
                CourseId = post.CourseId
            };
        }
    }
}
