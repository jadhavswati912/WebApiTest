using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Product_Details.Entities;
using Product_Details.Interface;
using WebApiTest.Entities;
using WebApiTest.Interface;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860


namespace Product_Details.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        /// <summary>
        /// Constructor method to initilize config and DI
        /// </summary>
        /// <param name="UserService"></param>
        public UserController(IUserService UserService)
        {
            _userService = UserService;
        }
        // GET: api/<UserController>
        [HttpGet]
        public async Task<IActionResult>GetAll()
        {
            var usersList = await _userService.GetAll();

            return Ok(usersList);
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetById(id);
                if (user == null)
                {
                return NotFound(); // Return 404 if the user is not found
                }

            return Ok(user); // Return 200 with the user as the response
        }

        // POST api/<UserController>
        [HttpPost]
        public async Task<IActionResult> AddUser(User user)
        {
            var createduser = await _userService.AddUser(user);
            return Ok(createduser);
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserById(int id, User user)
        {
            var updateuser = await _userService.UpdateUserById(id, user);

            if (updateuser == null)
            {  return NotFound(); }
               
            return Ok(updateuser);

        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserById(int id)
        {
            var isDeleted = await _userService.DeleteUserById(id);
            if (isDeleted == false)
            {
                return NotFound();
            }
            return Ok(true);
        }
    }
}
