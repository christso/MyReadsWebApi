using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyReadsWebApi.Models
{
    public class LibraryContext : DbContext
    {
        public LibraryContext(DbContextOptions<LibraryContext> options) 
            : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserBook>()
                .HasKey(x => new { x.UserId, x.BookId });

            modelBuilder.Entity<BookAuthor>()
                .HasKey(x => new { x.BookId, x.AuthorId });
        }
    }

    public class UserBook
    {
        public string BookId { get; set; }
        public string UserId { get; set; }

        [JsonIgnore]
        public Book Book { get; set; }
        [JsonIgnore]
        public User User { get; set; }
    }

    public class User
    {
        public string Id { get; set; }
        public ICollection<UserBook> BooksLink { get; set; }
    }

    public class BookAuthor
    {
        public string BookId { get; set; }
        public string AuthorId { get; set; }
        [JsonIgnore]
        public Book Book { get; set; }
        [JsonIgnore]
        public Author Author { get; set; }
    }

    public class Book
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public ICollection<BookAuthor> AuthorsLink { get; set; }
        public string Shelf { get; set; }
    }

    public class Author
    {
        public Author(string name)
        {
            this.Name = name;
            this.Id = name;
        }

        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class ImageLink
    {
        public string Thumbnail { get; set; }
        public string SmallThumbnail { get; set; }
    }
}
