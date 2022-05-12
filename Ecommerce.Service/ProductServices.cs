using Ecommerce.Data;
using Ecommerce.Shared.Model;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Service
{
    public interface IProductServices
    {
        public Task Add(Product product);
        public List<Product> GetAll();
        public ApiResponse ValidDataCreate(Product product);
        public Product GetProductById(Guid productId);
    }

    public class ProductServices : IProductServices
    {
        private readonly IConfiguration _config;
        private readonly IMongoCollection<Product> _productCollection;
        private readonly ICategoryServices _categoryServices;
        public ProductServices(IConfiguration config, ICategoryServices categoryServices)
        {
            _config = config;
            var connectionString = _config.GetSection("EcommerceDatabase:ConnectionString").Value;
            var databaseName = _config.GetSection("EcommerceDatabase:DatabaseName").Value;
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);
            _productCollection = database.GetCollection<Product>("Product");
            _categoryServices = categoryServices;
        }


        public async Task Add(Product product)
        {
            await _productCollection.InsertOneAsync(product);
        }

        public List<Product> GetAll()
        {
            return _productCollection.Find(x => x.IsDeleted == 0).ToList();
        }

        public Product GetProductById(Guid productId)
        {
            return _productCollection.Find(x => x.IsDeleted == 0 && x.Id == productId).FirstOrDefault();
        }

        public ApiResponse ValidDataCreate(Product product)
        {
            var category = _categoryServices.GetCategoryById(product.CategoryId);

            if (category == null)
                return new ApiResponse
                {
                    Status = 0,
                    Msg = "Không tìm thấy danh mục"
                };

            if(product.Quantity < 0 || product.Price < 0)
                return new ApiResponse
                {
                    Status = 0,
                    Msg = "Giá bạn nhập không hợp lệ"
                };

            if (product.Quantity < 0)
                return new ApiResponse
                {
                    Status = 0,
                    Msg = "Số lượng bạn nhập không hợp lệ"
                };

            return new ApiResponse
            {
                Status = 1,
            };
        }
    }
}
