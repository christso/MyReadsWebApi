using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyReadsWebApi.Models
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
            this.Database.EnsureCreated();
            this.Database.EnsureDeleted();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
    }

    public class User
    {
        public string Id { get; set; }
        public List<Book> Books { get; set; }
    }

    public class Author
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }

    public class Book
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public List<Author> Authors { get; set; }
        public string Shelf { get; set; }
    }

    public class ImageLink
    {
        public string Thumbnail { get; set; }
        public string SmallThumbnail { get; set; }
    }


}
