using Ecommerce.Data;
using Ecommerce.Shared.Model;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Service
{
    public interface IOrderServices
    {
        public Task Add(Order product);
        public IQueryable<Order> GetAll();
        public ApiResponse ValidDataCreate(Order product);
        public Order GetOrderById(Guid orderId);
    }

    public class OrderServices : IOrderServices
    {
        private readonly IConfiguration _config;
        private readonly IMongoCollection<Order> _orderCollection;
        private readonly IProductServices _productServices;
        private readonly IUserServices _userServices;
        public OrderServices(IConfiguration config, IProductServices productServices, IUserServices userServices)
        {
            _config = config;
            var connectionString = _config.GetSection("EcommerceDatabase:ConnectionString").Value;
            var databaseName = _config.GetSection("EcommerceDatabase:DatabaseName").Value;
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);
            _orderCollection = database.GetCollection<Order>("Order");
            _productServices = productServices;
            _userServices = userServices;
        }


        public async Task Add(Order order)
        {
            await _orderCollection.InsertOneAsync(order);
        }

        public IQueryable<Order> GetAll()
        {
            return _orderCollection.Find(x => x.IsDeleted == 0).ToList().AsQueryable();


        }

        public Order GetOrderById(Guid orderId)
        {
            return _orderCollection.Find(x => x.IsDeleted == 0 && x.Id == orderId).FirstOrDefault();
        }

        public ApiResponse ValidDataCreate(Order order)
        {
            var userProfile = _userServices.GetUserById(order.UserId);
            var product = _productServices.GetProductById(order.ProductId);

            if (userProfile == null)
                return new ApiResponse {Status = 0,Msg = "Không tìm thấy người dùng" };

            if (product == null)
                return new ApiResponse { Status = 0, Msg = "Không tìm thấy sản phẩm" };

            return new ApiResponse
            {
                Status = 1,
            };
        }
    }

    
}
