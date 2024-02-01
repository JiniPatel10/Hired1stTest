using core.Domain.ClassTypes;
using core.Domain.InterfaceRepositories;
using core.Domain.Models;
#region Usings
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#endregion
namespace core.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        #region Private variables
        private readonly IMongoDBContext _mongoContext = null;
        #endregion
        #region Constructor

        public ProductRepository(IMongoDBContext context)
        {
            _mongoContext = context;
        }
        #endregion
        #region Public methods

        /// <summary>
        /// save product in database
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public async Task<Product> Save(Product product)
        {
            if (string.IsNullOrEmpty(product.Id))
            {
                product.Created = DateTime.Now;
                product.Updated = DateTime.Now;
                await _mongoContext.Products.InsertOneAsync(product);
                return product;
            }
            else
            {
                product.Updated = DateTime.Now;
                var filter = Builders<Product>.Filter.Eq(v => v.Id, product.Id);
                await _mongoContext.Products.ReplaceOneAsync(filter, product);
                return product;
            }
        }




        /// <summary>
        /// get product list by userid
        /// </summary>
        /// <param name="pageInput"></param>
        /// <param name="userId"></param>
        /// <returns></returns>

        public async Task<PageResult<Product>> GetProductListByUserId(PageInput pageInput, string userId)
        {
            var builder = Builders<Product>.Filter;
            var filter = builder.Eq(x => x.CreatedBy, userId);

            pageInput.page = (int)Math.Ceiling((decimal)(pageInput.First / pageInput.Rows));


            var res = await _mongoContext.Products.Aggregate(new AggregateOptions { AllowDiskUse = true })
                .Match(filter)
                .Sort(pageInput.SortOrder == -1 ? Builders<Product>.Sort.Descending(pageInput.SortField).Ascending(x => x.Id) : Builders<Product>.Sort.Ascending(pageInput.SortField).Ascending(x => x.Id))
                .Skip(pageInput.page * pageInput.Rows)
                .Limit(pageInput.Rows)
                .ToListAsync();
            var count = await _mongoContext.Products.CountDocumentsAsync(filter);
            var result = new PageResult<Product>
            {
                Count = count,
                PageIndex = pageInput.page,
                PageSize = pageInput.Rows,
                Items = res
            };
            return result;
        }
        /// <summary>
        /// get product by id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<Product> GetById(string Id)
        {
            var filter = Builders<Product>.Filter.Eq(sub => sub.Id, Id);
            return await _mongoContext.Products.Find(filter).FirstOrDefaultAsync();
        }
        /// <summary>
        /// delete product by id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<bool> Delete(string Id)
        {
            var builder = Builders<Product>.Filter;
            var filter = builder.And(builder.Eq(x => x.Id, Id));
            var updateResult = await _mongoContext.Products.FindOneAndDeleteAsync(filter);
            if (updateResult != null) return true;
            else return false;

        }
        #endregion
    }

}
