using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MusicForum.Data;

namespace MusicForum.Models
{
    public class Discussion
    {
        public int DiscussionId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;

        [Display(Name = "Image")]
        public string? ImageFilename { get; set; }

        //Property for file upload, not mapped in EF
        [NotMapped]
        [Display(Name = "Photograph")]
        public IFormFile? ImageFile { get; set; } //nullable!

        [Display(Name = "Date Created")]
        public DateTime CreateDate { get; set; }

        // Navigation property
        public List<Comment>? Comments { get; set; } = new List<Comment>();

        //Foreign Key (AspNetUsers table)
        public string ApplicationUserId { get; set; } = string.Empty;

        //Navigation property
        public ApplicationUser? ApplicationUser { get; set; } //nullable!!
    }
}
