using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.InMemory;
using MyReadsWebApi.Data;
using MyReadsWebApi.Models;
using MyReadsWebApi.ViewModels;

namespace MyReadsWebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public BooksController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost("default")]
        public ActionResult PostDefault()
        {
            return Ok();
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<BookViewModel>> Get()
        {
            var user = GetUser();
            var userId = GetToken();
            if (user == null) 
            {
                _userRepository.CreateDefaultUser(userId);
                user = _userRepository.FindOne(userId);
            }

            var userVm = UserRepositoryHelper.Map(user);
            return Ok(userVm.Books);
        }

        public User GetUser()
        {
            var token = GetToken();
            var user = _userRepository.FindOne(token);
            return user;
        }

        public string GetToken()
        {
            return Request.Headers["Authorization"].FirstOrDefault();
        }

    }
}