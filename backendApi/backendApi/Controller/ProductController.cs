#region Usings
using backendApi.Infrastructure;
using backendApi.Middleware;
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
                return BadRequest(new GeneralErrorResultModel(ErrorCodes.CreatingProductError, errorMessage));
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
                return BadRequest(new GeneralErrorResultModel(ErrorCodes.GettingProductByIdError, ErrorMsg));
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
                return BadRequest(new GeneralErrorResultModel(ErrorCodes.DeletingProductError, ErrorMsg));
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
                return BadRequest(new GeneralErrorResultModel(ErrorCodes.GettlingAllProductError, errorMessage));
            }
        }
        #endregion
    }
}
