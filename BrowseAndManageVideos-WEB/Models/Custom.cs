namespace BrowseAndManageVideos_WEB.Models
{
    public class UpdateMovieNameData
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Actor { get; set; }
        public string Rating { get; set; }
    }

    public class MovieID
    {
        public string Id { get; set; }
    }

    public class Search
    {
        public string search { get; set; }
    }

    public class Filter
    {
        public int State { get; set; }

    }
}
