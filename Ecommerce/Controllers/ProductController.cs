using Ecommerce.Data;
using Ecommerce.Models.Request;
using Ecommerce.Models.Request.Product;
using Ecommerce.Service;
using Ecommerce.Shared;
using Ecommerce.Shared.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ecommerce.Controllers
{
    [Microsoft.AspNetCore.Components.Route("api/product")]
    [ApiController]
    [AuthorizationAttribute]
    public class ProductController
    {
        private readonly IProductServices _productServices;
        public ProductController(IProductServices productServices)
        {
            _productServices = productServices;
        }

        [HttpPost("create-product")]
        public async Task<ApiResponse<string>> Create([FromBody] CreateProduct request)
        {
            try
            {
                var product = new Product()
                {
                    CategoryId = request.CategoryId,
                    Name = request.Name,
                    Description = request.Description,
                    Price = request.Price,
                    Quantity = request.Quantity
                };

                var validCreate = _productServices.ValidDataCreate(product);

                if(validCreate.Status == 1)
                {
                    await _productServices.Add(product);
                }
                else
                {
                    return new ApiResponse<string> { Status = validCreate.Status, Msg = validCreate.Msg };
                }

                return new ApiResponse<string>
                {
                    Status = 1,
                    Data = "Thêm sản phẩm thành công"
                };
            }
            catch (System.Exception ex)
            {
                return new ApiResponse<string>
                {
                    Status = 0,
                    Data = "Có lỗi xảy ra trong quá trình xử lý"
                };
            }
        }

        [HttpGet("get-all-product")]
        public async Task<ApiResponse<List<Product>>> GetAll()
        {
            try
            {
                var result = _productServices.GetAll();

                return new ApiResponse<List<Product>>
                {
                    Status = 1,
                    Data = result
                };
            }
            catch (System.Exception ex)
            {
                return new ApiResponse<List<Product>>
                {
                    Status = 0,
                    Msg = "Có lỗi xảy ra trong quá trình xử lý"
                };
            }
        }

        [HttpGet("get-details-product-by-{id}")]
        public async Task<ApiResponse<Product>> GetDetails(string id)
        {
            try
            {
                var productId = Guid.Parse(id);

                var product = _productServices.GetProductById(productId);

                return new ApiResponse<Product>
                {
                    Status = 1,
                    Data = product
                };
            }
            catch (System.Exception ex)
            {
                return new ApiResponse<Product>
                {
                    Status = 0,
                    Msg = "Có lỗi xảy ra trong quá trình xử lý"
                };
            }
        }
    }
}
