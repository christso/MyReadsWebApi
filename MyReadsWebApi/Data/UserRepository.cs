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
    public class UserRepository : IUserRepository
    {
        public List<User> Users = new List<User>();

        public UserRepository()
        {
            this.AddDefaultData();
        }

        public User FindOne(string id)
        {
            return Users.Where(x => x.Id == id).FirstOrDefault();
        }

        public Task AddUserAsync(User user)
        {
            return Task.Run(() => Users.Add(user));
        }

        public IEnumerable<User> GetAllUsers()
        {
            return Users;
        }

        public void AddDefaultData()
        {
            if (Users.Find(x => x.Id == "default") == null)
            {
                var user = new User()
                {
                    Id = "default",
                    Books = new List<Book>()
                    {
                        new Book()
                        {
                            Title = "The Linux Command Line",
                            Shelf = "read",
                            Id = "nggnmAEACAAJ",
                            Authors = new List<Author>()
                            {
                                new Author() { Name = "William E. Shotts, Jr." }
                            }
                        },
                        new Book()
                        {
                            Title = "Learning Web Development with React and Bootstrap",
                            Shelf = "currentlyReading",
                            Id = "sJf1vQAACAAJ",
                            Authors = new List<Author>
                            {
                                new Author() { Name = "Harmeet Singh" },
                                new Author() { Name = "Mehul Bhatt" }
                            }
                        },
                        new Book()
                        {
                            Title = "The Cuckoo's Calling",
                            Shelf = "wantToRead",
                            Id = "evuwdDLfAyYC",
                            Authors = new List<Author>
                            {
                                new Author() { Name = "Robert Galbraith" }
                            }
                        }

                    }
                };

                Users.Add(user);
            }
        }
    }
}
