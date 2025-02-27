using System.Threading.Tasks;
using ClassLibrary1.Models;

namespace ClassLibrary1.Services
{
    public interface IUserService
    {
        Task<User> GetUserByIdAsync(int userId);
        Task<User> GetUserByUsernameAsync(string username);
        Task<bool> CreateUserAsync(User user);
    }
}
