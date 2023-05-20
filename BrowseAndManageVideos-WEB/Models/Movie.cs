namespace BrowseAndManageVideos_WEB.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string? Description { get; set; }
        public string? Actor { get; set; }
        public string? ContentType { get; set; }
        public bool IsDeleted { get; set; }
        public string? Rating { get; set; }
        public string Size { get; set; }
        public string? FrameWidth { get; set; }
        public string? FrameHeight { get; set; }
        public string? TotalBitrate { get; set; }
        public string? EncodingBitrate { get; set; }
    }
}
