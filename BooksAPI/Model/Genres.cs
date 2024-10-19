﻿using System.ComponentModel.DataAnnotations;

namespace BooksAPI.Model
{
    public class Genres
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
