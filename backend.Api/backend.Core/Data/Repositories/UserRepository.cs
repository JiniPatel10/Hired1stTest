using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using System;
using core.Domain.Repositories;

namespace twoladder.Core.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoDBContext _mongoContext = null;

        public UserRepository(IMongoDBContext context)
        {
            _mongoContext = context;
        }

    }
}
