#region Usings
using backendApi.Infrastructure;
using core.Domain.ClassTypes;
using core.Domain.InterfaceRepositories;
using core.Domain.Models;
using core.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
#endregion

namespace backendApi.Controller
{
    [Route("api/[controller]")]

    public class ProductController : ControllerBase
    {
        #region Private variables

        private readonly IProductRepository _productRepository;
        #endregion
        #region Constructor

        public ProductController(
                   IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        #endregion
        #region Public methods

        [HttpPost("createProduct")]
        public async Task<IActionResult> CreateProduct([FromBody] Product product)
        {
            try
            {
                    product.Created = DateTime.Now;
                    product = await _productRepository.Save(product);

            }
            catch (Exception ex)
            {
                string errorMessage = "Error occur while creating product in database";
                return BadRequest(errorMessage);
            }
            return Ok(product);
        }


        [HttpGet("getProductById/{id}")]
        public async Task<IActionResult> GetProductById(string id)
        {
            try
            {
                var result = await _productRepository.GetById(id);

                if (result == null)
                    return NotFound("Product not found");
                return Ok(result);
            }
            catch (Exception ex)
            {
                string ErrorMsg = "Error getting product by id" + ex.Message;
                return BadRequest(ErrorMsg);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var result = await _productRepository.Delete(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                string ErrorMsg = "Error deleting product by id" + ex.Message;
                return BadRequest(ErrorMsg);
            }
        }
        [HttpPost("getProductListByUserId/{id}")]
        public async Task<IActionResult> GetProductListByUserId([FromBody] PageInput pageInput, string id)
        {
            try
            {
                PageResult<Product> productList = await _productRepository.GetProductListByUserId(pageInput, id);
                return Ok(productList);
            }
            catch (Exception ex)
            {
                string errorMessage = "Error occur while getting the product list by id" + ex.Message;
                return NotFound(errorMessage);
            }
        }
        #endregion
    }
}
