using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LibrarySystem.Models
{
    public class Book
    {
        public int ID { get; set; }

        [DisplayName("Book Name")]
        public string Name { get; set; }

        [DisplayName("Author Name")]
        public string AuthorName { get; set; }

        [DisplayName("Category")]
        public string Category { get; set; }

        public ICollection<User> Users { get; set; }

        [DisplayName("Number of Books Available")]
        public int Count { get; set; }

        [DisplayName("Price")]
        public double Price { get; set; }

        [DisplayName("Image")]
        public string ImageName { get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
