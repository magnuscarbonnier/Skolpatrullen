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
        public string HashTags { get; set; }
        public DateTime PublishDate { get; set; }
        public int UserId { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
        public User User { get; set; }
        public IEnumerable<CourseBlogPost> BlogPosts { get; set; }
        public CourseBlogPost ToBlogPost(CourseBlogPost courseBlogPost)
        {
            courseBlogPost.Title = Title;
            courseBlogPost.Content = Content;
            courseBlogPost.HashTags = HashTags;
            courseBlogPost.PublishDate = PublishDate;
            courseBlogPost.UserId = UserId;
            courseBlogPost.CourseId = CourseId;
            return courseBlogPost;
        }
    }
}
