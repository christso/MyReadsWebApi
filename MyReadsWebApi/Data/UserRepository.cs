using Microsoft.EntityFrameworkCore;
using MyReadsWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyReadsWebApi.Data
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAllUsers();
        Task AddUserAsync(User user);
        User FindOne(string id);
        void AddDefaultData();
    }

    public class EfUserRepository : IUserRepository
    {
        private LibraryContext _context;
        public EfUserRepository(LibraryContext context)
        {
            _context = context;
            this.AddDefaultData();
        }

        public async Task AddUserAsync(User user)
        {
            await _context.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public User FindOne(string id)
        {
            return _context.Users.Find(id);
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _context.Users
                .Include(user => user.BooksLink)
                .ThenInclude(bookLink => bookLink.Book)
                .ThenInclude(book => book.AuthorsLink)
                .ThenInclude(authorLink => authorLink.Author)
                .ToList();
        }

        public void AddDefaultData()
        {
            var users = _context.Users;
            var user = users.Find(UserRepositoryHelper.DefaultId);
            if (user == null)
            {
                UserRepositoryHelper.CreateDefaultUser(_context);
            }
        }   
    }

    public class UserRepositoryHelper
    {
        public const string DefaultId = "default";

        public static User CreateDefaultUser(LibraryContext context)
        {
            var authors = new List<Author>()
            {
                new Author("William E. Shotts, Jr."),
                new Author("Harmeet Singh"),
                new Author("Mehul Bhatt"),
                new Author("Robert Galbraith")
            };
            context.Authors.AddRange(authors);

            var book1 = new Book()
            {
                Title = "The Linux Command Line",
                Shelf = "read",
                Id = "nggnmAEACAAJ"
            };
            book1.AuthorsLink = new List<BookAuthor>()
            {
                new BookAuthor() { Author = authors[0], Book = book1 }
            };

            var book2 = new Book()
            {
                Title = "Learning Web Development with React and Bootstrap",
                Shelf = "currentlyReading",
                Id = "sJf1vQAACAAJ"
            };
            book2.AuthorsLink = new List<BookAuthor>
            {
                new BookAuthor() { Author = authors[1], Book = book2 },
                new BookAuthor() { Author = authors[2], Book = book2 }
            };

            var book3 = new Book()
            {
                Title = "The Cuckoo's Calling",
                Shelf = "wantToRead",
                Id = "evuwdDLfAyYC"
            };
            book3.AuthorsLink = new List<BookAuthor>
            {
                new BookAuthor() { Author = authors[3], Book = book3 }
            };
            var books = new List<Book>()
            {
                book1,
                book2,
                book3
            };
            context.Books.AddRange(books);

            var user = new User()
            {
                Id = DefaultId
            };
            user.BooksLink = books.Select((book, index) =>
            {
                return new UserBook()
                {
                    Book = book,
                    User = user
                };
            }).ToList();
            context.Users.Add(user);
            context.SaveChanges();
            return user;
        }
    }
}
