#region Usings
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using System;
using core.Domain.Repositories;
using core.Domain.Models;
using MongoDB.Driver;
using core.Domain.ClassTypes;
#endregion
namespace core.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        #region Private variables

        private readonly IMongoDBContext _mongoContext = null;
        #endregion
        #region Constructor

        public UserRepository(IMongoDBContext context)
        {
            _mongoContext = context;
        }
        #endregion
        #region Public methods

        /// <summary>
        /// save user in database
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<User> Save(User user)
        {
            if (string.IsNullOrEmpty(user.Id))
            {
                user.Created = DateTime.Now;
                user.Updated = DateTime.Now;
                await _mongoContext.Users.InsertOneAsync(user);
                return user;
            }
            else
            {
                user.Updated = DateTime.Now;
                var filter = Builders<User>.Filter.Eq(v => v.Id, user.Id);
                await _mongoContext.Users.ReplaceOneAsync(filter, user);
                return user;
            }
        }

        /// <summary>
        /// update user password
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<bool> ChangePassword(string userId, string password)
        {

            var filter = Builders<User>.Filter.Eq(v => v.Id, userId);
            var update = Builders<User>.Update
                .Set(x => x.Password, password)
                .Set(x=> x.Updated, DateTime.Now);

            var updateResult = await _mongoContext.Users.UpdateOneAsync(filter, update);
            bool resultFlag = false;
            if (updateResult.IsAcknowledged && updateResult.IsModifiedCountAvailable)
            {
                resultFlag = updateResult.MatchedCount == 1;
            }
            return resultFlag;

        }


        /// <summary>
        /// update user email
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<bool> ChangeEmail(string userId, string email)
        {

            var filter = Builders<User>.Filter.Eq(v => v.Id, userId);
            var update = Builders<User>.Update
                .Set(x => x.Email, email)
                .Set(x => x.Updated, DateTime.Now);

            var updateResult = await _mongoContext.Users.UpdateOneAsync(filter, update);
            bool resultFlag = false;
            if (updateResult.IsAcknowledged && updateResult.IsModifiedCountAvailable)
            {
                resultFlag = updateResult.MatchedCount == 1;
            }
            return resultFlag;

        }

        /// <summary>
        /// update user details
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<bool> UpdateUser(User user)
        {

            var filter = Builders<User>.Filter.Eq(v => v.Id, user.Id);
            var update = Builders<User>.Update
                .Set(x => x.FirstName, user.FirstName)
                .Set(x => x.LastName, user.LastName)
                .Set(x => x.Updated, DateTime.Now);

            var updateResult = await _mongoContext.Users.UpdateOneAsync(filter, update);
            bool resultFlag = false;
            if (updateResult.IsAcknowledged && updateResult.IsModifiedCountAvailable)
            {
                resultFlag = updateResult.MatchedCount == 1;
            }
            return resultFlag;

        }


        /// <summary>
        /// Get user by email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<User> GetUserByEmail(string email)
        {
            return await _mongoContext.Users.Find(x => x.Email.ToLower() == email.ToLower()).FirstOrDefaultAsync();
        }

        /// <summary>
        /// check if there is a user with entered credentials
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<User> CheckUser(string email, string password)
        {
            return await _mongoContext.Users.Find(x => x.Email.ToLower() == email.ToLower() && x.Password.ToLower() == password.ToLower()).FirstOrDefaultAsync();
        }

        /// <summary>
        /// get all user list
        /// </summary>
        /// <param name="pageInput"></param>
        /// <returns></returns>

        public async Task<PageResult<User>> GetUserList(PageInput pageInput)
        {
            var filter = Builders<User>.Filter.Empty;

            pageInput.page = (int)Math.Ceiling((decimal)(pageInput.First / pageInput.Rows));


            var res = await _mongoContext.Users.Aggregate(new AggregateOptions { AllowDiskUse = true })
                .Match(filter)
                .Sort(pageInput.SortOrder == -1 ? Builders<User>.Sort.Descending(pageInput.SortField).Ascending(x => x.Id) : Builders<User>.Sort.Ascending(pageInput.SortField).Ascending(x => x.Id))
                .Skip(pageInput.page * pageInput.Rows)
                .Limit(pageInput.Rows)
                .ToListAsync();
            var count = await _mongoContext.Users.CountDocumentsAsync(filter);
            var result = new PageResult<User>
            {
                Count = count,
                PageIndex = pageInput.page,
                PageSize = pageInput.Rows,
                Items = res
            };
            return result;
        }
        /// <summary>
        /// get user by id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<User> GetById(string Id)
        {
            var filter = Builders<User>.Filter.Eq(sub => sub.Id, Id);
            return await _mongoContext.Users.Find(filter).FirstOrDefaultAsync();
        }
        /// <summary>
        /// delete user by id
        /// </summary>
        /// <param name="pageInput"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<bool> Delete(string Id)
        {
            var builder = Builders<User>.Filter;
            var filter = builder.And(builder.Eq(x => x.Id, Id));
            var updateResult = await _mongoContext.Users.FindOneAndDeleteAsync(filter);
            if (updateResult != null) return true;
            else return false;

        }
        #endregion
    }
}
