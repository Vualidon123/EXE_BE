namespace EXE_BE.Models
{
    public class Post
    {
        public int id { get; set; }
        public int userId { get; set; }
        public DateTime date { get; set; }
        public string detail { get; set; }
        public int likes { get; set; }
        public string title { get; set; }
    }
}

