using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.InMemory;
using MyReadsWebApi.Models;

namespace MyReadsWebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly DataContext _context;
    
        public BooksController(DataContext context)
        {
            _context = context;

            if (_context.Books.Count() == 0)
            {
                var book1 = new Book()
                {
                    Title = "The Linux Command Line",
                    Shelf = "read",
                    Id = "nggnmAEACAAJ",
                    Authors = new List<Author>()
                    {
                        new Author() { Name = "William E. Shotts, Jr." }
                    }
                };
                _context.Books.Add(book1);

                var book2 = new Book()
                {
                    Title = "Learning Web Development with React and Bootstrap",
                    Shelf = "currentlyReading",
                    Id = "sJf1vQAACAAJ",
                    Authors = new List<Author>
                    {
                        new Author() { Name = "Harmeet Singh" },
                        new Author() { Name = "Mehul Bhatt" }
                    }
                };
                _context.Books.Add(book2);

                var book3 = new Book()
                {
                    Title = "The Cuckoo's Calling",
                    Shelf = "wantToRead",
                    Id = "evuwdDLfAyYC",
                    Authors = new List<Author>
                    {
                        new Author() { Name = "Robert Galbraith" }
                    }
                };
                _context.Books.Add(book3);

                _context.SaveChanges();
            }
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<Book>> Get()
        {
            return _context.Books.ToList();
        }

        public User GetUser()
        {
            var token = GetToken();
            var user = _context.Users.Where(u => u.Id == token).FirstOrDefault();
            return user;
        }

        public string GetToken()
        {
            return Request.Headers["Authorization"].FirstOrDefault();
        }

    }
}