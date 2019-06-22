using Microsoft.EntityFrameworkCore;
using MyReadsWebApi.Models;
using MyReadsWebApi.ViewModels;
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
        void CreateDefaultUser(string id);
        User Delete(string id);
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

        public User Delete(string userId)
        {
            var user = _context.Users.Find(userId);
            if (user == null)
            {
                return null;
            }
            _context.Users.Remove(user);
            _context.SaveChanges();
            return user;
        }

        public async Task AddUserAsync(User user)
        {
            await _context.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public User FindOne(string id)
        {
            return _context.Users.Where(user => user.Id == id)
                .Include(user => user.BooksLink)
                .ThenInclude(bookLink => bookLink.Book)
                .ThenInclude(book => book.AuthorsLink)
                .ThenInclude(authorLink => authorLink.Author)
                .FirstOrDefault();
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

        public void CreateDefaultUser(string id)
        {
            UserRepositoryHelper.CreateDefaultUser(_context, id);
        }
    }

    public class UserRepositoryHelper
    {
        public const string DefaultId = "default";

        public static UserViewModel Map(User user)
        {
            var userVm = new UserViewModel();
            userVm.Id = user.Id;
            foreach (var bookLink in user.BooksLink)
            {
                userVm.Books.Add(new BookViewModel()
                {
                    Id = bookLink.Book.Id,
                    Shelf = bookLink.Book.Shelf,
                    Title = bookLink.Book.Title,
                    Authors = bookLink.Book.AuthorsLink.Select((link, index) => link.Author.Name)
                        .ToList()
                });
            }
            return userVm;
        }

        public static User CreateDefaultUser(LibraryContext context)
        {
            return CreateDefaultUser(context);
        }

        public static void CreateDefaultUser(LibraryContext context, string userId)
        {
            if (context.Authors.Count() == 0)
            {
                var authors = new List<Author>()
                {
                    new Author("William E. Shotts, Jr."),
                    new Author("Harmeet Singh"),
                    new Author("Mehul Bhatt"),
                    new Author("Robert Galbraith")
                };
                context.Authors.AddRange(authors);
                context.SaveChanges();
            }

            if (context.Books.Count() == 0)
            {
                var book1 = new Book()
                {
                    Title = "The Linux Command Line",
                    Shelf = "read",
                    Id = "nggnmAEACAAJ"
                };
                book1.AuthorsLink = new List<BookAuthor>()
                {
                new BookAuthor() { Author = context.Authors.Find("William E. Shotts, Jr.") }
                };

                var book2 = new Book()
                {
                    Title = "Learning Web Development with React and Bootstrap",
                    Shelf = "currentlyReading",
                    Id = "sJf1vQAACAAJ"
                };
                book2.AuthorsLink = new List<BookAuthor>
                {
                new BookAuthor() { Author = context.Authors.Find("Harmeet Singh") },
                new BookAuthor() { Author = context.Authors.Find("Mehul Bhatt") }
                };

                var book3 = new Book()
                {
                    Title = "The Cuckoo's Calling",
                    Shelf = "wantToRead",
                    Id = "evuwdDLfAyYC"
                };
                book3.AuthorsLink = new List<BookAuthor>
                {
                new BookAuthor() { Author = context.Authors.Find("Robert Galbraith") }
                };
                var books = new List<Book>()
                {
                    book1,
                    book2,
                    book3
                };
                context.Books.AddRange(books);
            }

            var user = new User()
            {
                Id = userId
            };
            user.BooksLink = context.Books.ToList()
                .Select((book, index) => new UserBook()
            {
                Book = book,
                User = user
            }).ToList();
            context.Users.Add(user);
            context.SaveChanges();
        }
    }
}
