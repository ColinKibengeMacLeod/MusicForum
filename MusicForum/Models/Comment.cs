namespace MusicForum.Models
{
    public class Comment
    {
        public int CommentId { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreateDate { get; set; }

        //Foreign Key
        public int ThreadId { get; set; }

        // Navigation property
        public Thread? Thread { get; set; }
    }
}
