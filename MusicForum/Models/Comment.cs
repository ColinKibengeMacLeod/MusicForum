using MusicForum.Data;

namespace MusicForum.Models
{
    public class Comment
    {
        public int CommentId { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreateDate { get; set; }

        //Foreign Key
        public int DiscussionId { get; set; }

        // Navigation property
        public Discussion? Discussion { get; set; }

        //Foreign Key (AspNetUsers table)
        public string ApplicationUserId { get; set; } = string.Empty;

        //Navigation property
        public ApplicationUser? ApplicationUser { get; set; } //nullable!!
    }
}
