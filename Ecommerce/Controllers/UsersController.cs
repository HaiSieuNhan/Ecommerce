using Ecommerce.Data;
using Ecommerce.Models.Request;
using Ecommerce.Models.Request.User;
using Ecommerce.Service;
using Ecommerce.Shared;
using Ecommerce.Shared.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ecommerce.Controllers
{
    [Microsoft.AspNetCore.Components.Route("api/users")]
    [ApiController]
    [AuthorizationAttribute]
    public class UsersController
    {
        private IUserServices _userServices;
        public UsersController(IUserServices userServices)
        {
            _userServices = userServices;
        }

        [HttpPost("login")]
        public async Task<ApiResponse<string>> Login([FromBody] LoginRequest request)
        {
            try
            {
                var login = _userServices.Login(request.Username, request.Password);

                return new ApiResponse<string>
                {
                    Status = 1,
                    Data = login.Data
                };
            }
            catch (System.Exception ex)
            {

               await LoggerException.LogError(ex, "login", string.Empty);
                return new ApiResponse<string>
                {
                    Status = 0,
                    Data = "Có lỗi xảy ra trong quá trình xử lý"
                };
            }
        }

        [HttpPost("create")]
        public async Task<ApiResponse<string>> Create([FromBody] CreateUser request)
        {
            try
            {
                var userProfile = new UserProfile()
                {
                    Name = request.Name,
                    UserName = request.UserName,
                    PassWord = request.PassWord,
                    Address = request.Address
                };

                var resultValid = _userServices.ValidDataCreate(userProfile);
                if (resultValid.Status == 1)
                {
                    userProfile.PassWord = Helper.HashPassword(request.PassWord);
                    _userServices.Add(userProfile);
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
                await LoggerException.LogError(ex, "login", JsonConvert.SerializeObject(request));
                return new ApiResponse<string>
                {
                    Status = 0,
                    Data = "Có lỗi xảy ra trong quá trình xử lý"
                };
            }
        }

        [HttpGet("get-all")]
        public async Task<ApiResponse<List<UserProfile>>> GetAll()
        {
            try
            {
                var result = _userServices.GetAll();

                return new ApiResponse<List<UserProfile>>
                {
                    Status = 1,
                    Data = result
                };
            }
            catch (System.Exception ex)
            {
                return new ApiResponse<List<UserProfile>>
                {
                    Status = 0,
                    Msg = "Có lỗi xảy ra trong quá trình xử lý"
                };
            }
        }

        [HttpGet("{id}")]
        public async Task<ApiResponse<UserProfile>> GetDetails(string id)
        {
            try
            {
                var userId = Guid.Parse(id);

                var user = _userServices.GetUserById(userId);

                return new ApiResponse<UserProfile>
                {
                    Status = 1,
                    Data = user
                };
            }
            catch (System.Exception ex)
            {
                return new ApiResponse<UserProfile>
                {
                    Status = 0,
                    Msg = "Có lỗi xảy ra trong quá trình xử lý"
                };
            }
        }
    }
}
