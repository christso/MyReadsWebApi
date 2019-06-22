using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _userRepository.AddDefaultData();
        }

        [HttpPost("default")]
        public ActionResult PostDefault()
        {
            try
            {
                _userRepository.AddDefaultData();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpDelete("{userId}")]
        public ActionResult<User> Delete(string userId)
        {
            try
            {
                return Ok(_userRepository.Delete(userId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        public ActionResult<User> Post([FromBody] User user)
        {
            try
            {
                _userRepository.AddUserAsync(user).GetAwaiter().GetResult();
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        public ActionResult<IEnumerable<UserViewModel>> Get()
        {
            try
            {
                var users = _userRepository.GetAllUsers();
                var userVms = users.Select((user, index) =>
                {
                    return UserRepositoryHelper.Map(user);
                }).ToList();
                return Ok(userVms);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
