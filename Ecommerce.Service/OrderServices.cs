using Ecommerce.Data;
using Ecommerce.Service.Model;
using Ecommerce.Shared;
using Ecommerce.Shared.Model;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Service
{
    public interface IOrderServices
    {
        public Task Add(Order product);
        public Paged<OrderViewModel> GetAll(SearchOrder request);
        public ApiResponse ValidDataCreate(Order product);
        public Order GetOrderById(Guid orderId);
    }

    public class OrderServices : IOrderServices
    {
        private readonly IConfiguration _config;
        private readonly IMongoCollection<Order> _orderCollection;
        private readonly IProductServices _productServices;
        private readonly IUserServices _userServices;
        private readonly ICategoryServices _categoryServices;

        public OrderServices(IConfiguration config, IProductServices productServices, IUserServices userServices, ICategoryServices categoryServices)
        {
            _config = config;
            var connectionString = _config.GetSection("EcommerceDatabase:ConnectionString").Value;
            var databaseName = _config.GetSection("EcommerceDatabase:DatabaseName").Value;
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);
            _orderCollection = database.GetCollection<Order>("Order");
            _productServices = productServices;
            _userServices = userServices;
            _categoryServices = categoryServices;
        }

        public async Task Add(Order order)
        {
            await _orderCollection.InsertOneAsync(order);
        }

        public Paged<OrderViewModel> GetAll(SearchOrder request)
        {
            var orderDb = _orderCollection.Find(x => x.IsDeleted == 0).ToList().AsQueryable();
            var dicUser = _userServices.GetAll().ToDictionary(x => x.Id, x => x.Name);
            var dicCategory = _categoryServices.GetAll().ToDictionary(x => x.Id, x => x.Name);

            var orderModels = new List<OrderViewModel>();

            foreach (var data in orderDb)
            {
                var product = _productServices.GetProductById(data.ProductId);

                orderModels.Add(new OrderViewModel
                {
                    UserId = data.UserId,
                    NameUser = dicUser.ContainsKey(data.UserId) ? dicUser[data.UserId] : null,
                    ProductId = data.ProductId,
                    ProductName  = product != null ? product.Name : null,
                    CategorytId = product != null ? product.Id : (Guid?)null,
                    CategoryName = product != null && dicCategory.ContainsKey(product.CategoryId) ? dicCategory[product.CategoryId] : null,
                    OrderDate = data.OrderDate,
                    Amount = data.Amount,
                });
            }

            if (!string.IsNullOrEmpty(request.KeyWord))
            {
                var keyWord = request.KeyWord.ToLower().Trim();
                orderModels = orderModels.Where(x => x.ProductName.ToLower().Trim().Contains(keyWord)).ToList();
            }

            if (!request.IsSortProductNameDesc)
            {
                orderModels = orderModels.OrderBy(x => x.ProductName).ToList();
            }
            else
            {
                orderModels = orderModels.OrderByDescending(x => x.ProductName).ToList();
            }

            // paging
            var paged = new Paged<OrderViewModel>(orderModels, request);
            var result = paged.Convert<OrderViewModel>();
            result.Data = paged.Data;
            return result;
        }

        public static Func<IQueryable<T>, IQueryable<T>> Page<T>(int pageAt = 1, int pageSize = 20)
        {
            var myPage = pageAt < 1 ? 1 : pageAt;
            var myPageSize = pageSize <= 0 ? 20 : pageSize;
            return source => source.Skip((myPage - 1) * pageSize).Take(myPageSize);
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
