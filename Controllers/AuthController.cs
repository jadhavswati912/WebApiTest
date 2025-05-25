using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Product_Details.Interface;
using Product_Details.Model;
using Product_Details.Services;

namespace Product_Details.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly TokenService _tokenService;
        public AuthController(IUserService userService, TokenService tokenService)  
        {
            _userService = userService;
             _tokenService= tokenService;

        }

        public AuthenticateRequest UserName { get; private set; }
        public object Username { get; private set; }
        public object Password { get; private set; }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(AuthenticateRequest model)
        { 
            if (model.UserName == null | model.Password == null)
            {
                return Ok("Please enter ur UserName And Password");
                
            }
            var token = _userService.Authenticate(model);
            if (token == null)
            {
                return Ok("Please enter Correct UserName");
            }
            //var token = _tokenService.GenerateToken(model.UserName);
            //{
                return Ok(new { token });
            //}
        }
    }
}
