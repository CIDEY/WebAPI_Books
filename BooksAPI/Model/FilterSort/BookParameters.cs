namespace BooksAPI.Model.FilterSort
{
    public class BookParameters
    {
        public string? SearchTerm { get; set; }
        public int? MinYear { get; set; }
        public int? MaxYear { get; set; }
        public double? MinRating { get; set; }
        public double? MaxRating { get; set; }
        public string? SortBy { get; set; }
        public bool SortDescending { get; set; }
    }
}
