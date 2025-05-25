using Product_Details.Entities;
using Product_Details.Model;

namespace Product_Details.Interface
{
    public interface IUserService
    {
        Task<AuthenticateResponse?> Authenticate(AuthenticateRequest model);
        Task<IEnumerable<User>> GetAll();
        Task<User?> GetById(int id);
        Task<User> AddUser(User user);
        Task<User> UpdateUserById(int id, User user);
        Task<bool> DeleteUserById(int id);
        Task<User?> GetUserByUserName(string UserName);
       
    }
}
