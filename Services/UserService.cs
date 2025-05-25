using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Product_Details.Entities;
using Product_Details.Interface;
using Product_Details.Model;
using WebApiTest.Context;
using WebApiTest.Entities;

namespace Product_Details.Services
{
    public class UserService : IUserService
    {

        private readonly AppSettings _appSettings;
        private readonly AppDbContext _context;


        public object user => throw new NotImplementedException();

        public UserService(IOptions<AppSettings> appSettings, AppDbContext context)

        {
            _appSettings = appSettings.Value;
            _context = context;  //db is hold a instance of atabase context (appdbcontext)                                         
        }


        public async Task<AuthenticateResponse?> Authenticate(AuthenticateRequest model)//(login method)
        {

            var user = await _context.user.SingleOrDefaultAsync(x => x.Username == model.UserName && x.Password == model.Password);
            // return null if user not found 
            if (user == null)
                return null;

            // authentication successful so generate jwt token 
            var token = await generateJwtToken(user);
            return new AuthenticateResponse(user, token);
        }
        //get all  user using condition
        public async Task<IEnumerable<User>> GetAll()
        {
            return await _context.user.Where(static x => x.isActive == true).ToListAsync();
        }

        public async Task<User?> GetById(int id)
        {
            return await _context.user.FirstOrDefaultAsync(u => u.Id == id);
        }

        // helper methods 
        private async Task<string> generateJwtToken(User user)
        {
            //Generate token that is valid for 7 days 
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = await Task.Run(() =>
            {
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)//this is algorithum of SecurityAlgorithms
                };
                return tokenHandler.CreateToken(tokenDescriptor);
            });
            return tokenHandler.WriteToken(token);
        }

        //add user method 
        public async Task<User> AddUser(User user)
        {
            _context.user.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }
        public async Task<bool> DeleteUserById(int id)
        {
            var Existinguser = await _context.user.FindAsync(id);
            if (Existinguser == null)
                return false;
            _context.user.Remove(Existinguser);
            await _context.SaveChangesAsync();
            return true;

        }
        public async Task<User> UpdateUserById(int id, User user)
        {
            var existinguser = await _context.user.FirstOrDefaultAsync(x => x.Id == id);
            if (existinguser == null)
            {
                return null;
            }
            existinguser.FirstName = user.FirstName;
            existinguser.LastName = user.LastName;
            existinguser.Username = user.Username;
            existinguser.Password = user.Password;
            existinguser.isActive = user.isActive;
            await _context.SaveChangesAsync();

            return existinguser;
        }

        public  async Task<User?> GetUserByUserName(string UserName)
        {
            var userobj = await _context.user.FirstOrDefaultAsync(  u => u.Username ==UserName);
            if (userobj == null)

            { return null; }
            return userobj;
            

        }

       
    }
}



