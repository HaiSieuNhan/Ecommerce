using Ecommerce.Data;
using Ecommerce.Models.Request;
using Ecommerce.Models.Request.Order;
using Ecommerce.Service;
using Ecommerce.Service.Model;
using Ecommerce.Shared;
using Ecommerce.Shared.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ecommerce.Controllers
{
    [Microsoft.AspNetCore.Components.Route("api/order")]
    [ApiController]
    [AuthorizationAttribute]
    public class OrderController
    {
        private readonly IOrderServices _orderServices;
        public OrderController(IOrderServices orderServices)
        {
            _orderServices = orderServices;
        }

        [HttpPost("create-order")]
        public async Task<ApiResponse<string>> Create([FromBody] CreateOrder request)
        {
            try
            {
                var order = new Order()
                {
                    Name = request.Name,
                    UserId = request.UserId,
                    ProductId = request.ProductId,
                    Amount = request.Amount,
                    OrderDate = request.OrderDate,
                };

                var resultValid = _orderServices.ValidDataCreate(order);
                if (resultValid.Status == 1)
                {
                   await _orderServices.Add(order);
                }
                else
                {
                    return new ApiResponse<string>
                    {
                        Status = resultValid.Status,
                        Msg = resultValid.Msg
                    };
                }

                return new ApiResponse<string>
                {
                    Status = 1,
                    Data = "Thêm người dùng thành công"
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

        [HttpPost("get-all-order")]
        public async Task<ApiResponse<Paged<OrderViewModel>>> GetAll([FromBody] SearchOrder searchOrder)
        {
            try
            {
                var result = _orderServices.GetAll(searchOrder);

                return new ApiResponse<Paged<OrderViewModel>>
                {
                    Status = 1,
                    Data = result
                };
            }
            catch (System.Exception ex)
            {
                return new ApiResponse<Paged<OrderViewModel>>
                {
                    Status = 0,
                    Msg = "Có lỗi xảy ra trong quá trình xử lý"
                };
            }
        }

        [HttpGet("get-detail-order-by-{id}")]
        public async Task<ApiResponse<Order>> GetDetails(string id)
        {
            try
            {
                var orderId = Guid.Parse(id);

                var order = _orderServices.GetOrderById(orderId);

                return new ApiResponse<Order>
                {
                    Status = 1,
                    Data = order
                };
            }
            catch (System.Exception ex)
            {
                return new ApiResponse<Order>
                {
                    Status = 0,
                    Msg = "Có lỗi xảy ra trong quá trình xử lý"
                };
            }
        }
    }
}
