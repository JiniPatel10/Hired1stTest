using core.Domain.ClassTypes;
using core.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core.Domain.InterfaceRepositories
{
    public interface IProductRepository
    {
        Task<Product> Save(Product product);
        Task<PageResult<Product>> GetProductListByUserId(PageInput pageInput, string userId);
        Task<Product> GetById(string Id);
        Task<bool> Delete(string Id);
    }
}
