﻿namespace BooksAPI.Model.FilterSort
{
    public class GenreParameters
    {
        public string? SearchTerm { get; set; }
        public string? SortBy { get; set; }
        public bool SortDescending { get; set; }
    }
}
