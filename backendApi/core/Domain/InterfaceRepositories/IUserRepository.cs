using core.Domain.ClassTypes;
using core.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<User> Save(User user);
        Task<User> GetUserByEmail(string email);
        Task<User> CheckUser(string email, string password);
        Task<PageResult<User>> GetUserList(PageInput pageInput);
        Task<User> GetById(string Id);
        Task<bool> Delete(string Id);
        Task<bool> ChangePassword(string userId, string password);
        Task<bool> ChangeEmail(string userId, string email);
        Task<bool> UpdateUser(User user);
    }
}
