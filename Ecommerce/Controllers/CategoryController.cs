using Ecommerce.Data;
using Ecommerce.Models.Request;
using Ecommerce.Service;
using Ecommerce.Shared.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ecommerce.Controllers
{
    [Microsoft.AspNetCore.Components.Route("api/category")]
    [ApiController]
    [AuthorizationAttribute]
    public class CategoryController
    {
        private ICategoryServices _categoryServices;
        public CategoryController(ICategoryServices categoryServices)
        {
            _categoryServices = categoryServices;
        }

        [HttpPost("create-category")]
        public async Task<ApiResponse<string>> Create([FromBody] CreateCategory request)
        {
            try
            {
                var category = new Category()
                {
                    Name = request.Name,
                };

                _categoryServices.Add(category);

                return new ApiResponse<string>
                {
                    Status = 1,
                    Data = "Thêm danh mục thành công"
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

        [HttpGet("get-all-category")]
        public async Task<ApiResponse<List<Category>>> GetAll()
        {
            try
            {
                var result = _categoryServices.GetAll();

                return new ApiResponse<List<Category>>
                {
                    Status = 1,
                    Data = result
                };
            }
            catch (System.Exception ex)
            {
                return new ApiResponse<List<Category>>
                {
                    Status = 0,
                    Msg = "Có lỗi xảy ra trong quá trình xử lý"
                };
            }
        }

        [HttpGet("get-details-category-by-{id}")]
        public async Task<ApiResponse<Category>> GetDetails(string id)
        {
            try
            {
                var categoryId = Guid.Parse(id);

                var category = _categoryServices.GetCategoryById(categoryId);

                return new ApiResponse<Category>
                {
                    Status = 1,
                    Data = category
                };
            }
            catch (System.Exception ex)
            {
                return new ApiResponse<Category>
                {
                    Status = 0,
                    Msg = "Có lỗi xảy ra trong quá trình xử lý"
                };
            }
        }
    }
}
