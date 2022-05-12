using Ecommerce.Data;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Service
{
    public interface ICategoryServices
    {
        public Task Add(Category category);
        public List<Category> GetAll();
        public Category GetCategoryById(Guid categoryId);
    }
   public class CategoryServices : ICategoryServices
    {
        private readonly IConfiguration _config;
        private readonly IMongoCollection<Category> _categoryCollection;
        public CategoryServices(IConfiguration config)
        {
            _config = config;

            var connectionString = _config.GetSection("EcommerceDatabase:ConnectionString").Value;
            var databaseName = _config.GetSection("EcommerceDatabase:DatabaseName").Value;

            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);
            _categoryCollection = database.GetCollection<Category>("Category");

        }

        public async Task Add(Category category)
        {
           await _categoryCollection.InsertOneAsync(category);
        }

        public List<Category> GetAll() =>
            _categoryCollection.Find(x => x.IsDeleted == 0).ToList();

        public Category GetCategoryById(Guid categoryId) =>
            _categoryCollection.Find(x => x.IsDeleted == 0 && x.Id == categoryId).FirstOrDefault();
    }
}
