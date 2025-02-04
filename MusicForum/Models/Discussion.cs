using System;
using System.ComponentModel.DataAnnotations;

namespace MusicForum.Models
{
    public class Discussion
    {
        public int DiscussionId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string ImageFilename { get; set; } = string.Empty;
        [Display(Name = "Date Created")]
        public DateTime CreateDate { get; set; }

        // Navigation property
        public List<Comment>? Comments { get; set; } = new List<Comment>();
    }
}
