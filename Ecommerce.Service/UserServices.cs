using Ecommerce.Data;
using Ecommerce.Shared;
using Ecommerce.Shared.Caching;
using Ecommerce.Shared.Model;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.Service
{
    public interface IUserServices
    {
        public UserProfile Add(UserProfile student);
        public List<UserProfile> GetAll();
        public ApiResponse ValidDataCreate(UserProfile user);
        public UserProfile GetUserById(Guid userId);
        public ApiResponse<string> Login(string userName, string passWord);
        public UserProfile GetUserByToken(string token = "");
    }

    public class UserServices : IUserServices
    {
        private readonly IConfiguration _config;
        private readonly IMongoCollection<UserProfile> _userProfilesCollection;

        private readonly ICachingService _cachingService;

        public UserServices(IConfiguration config, ICachingService cachingService)
        {
            _config = config;

            var connectionString = _config.GetSection("EcommerceDatabase:ConnectionString").Value;
            var databaseName = _config.GetSection("EcommerceDatabase:DatabaseName").Value;

            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);
            _userProfilesCollection = database.GetCollection<UserProfile>("UserProfiles");
            _cachingService = cachingService;
        }

        public UserProfile Add(UserProfile user)
        {
            _userProfilesCollection.InsertOne(user);
            return user;
        }

        public ApiResponse ValidDataCreate(UserProfile user)
        {
            if (_userProfilesCollection.Find(x => x.IsDeleted == 0 && x.UserName == user.UserName).Any())
                return new ApiResponse
                {
                    Status = 0,
                    Msg = "Đã tồn tại User Name"
                };

            return new ApiResponse
            {
                Status = 1,
            };
        }

        public List<UserProfile> GetAll() =>
            _userProfilesCollection.Find(x => x.IsDeleted == 0).ToList();

        public UserProfile GetUserById(Guid UserId) =>
            _userProfilesCollection.Find(x => x.IsDeleted == 0 && x.Id == UserId).FirstOrDefault();

        public ApiResponse<string> Login(string userName, string passWord)
        {
            var user = _userProfilesCollection.Find(x => x.IsDeleted == 0 && x.UserName == userName).FirstOrDefault();

            if(user == null)
                return new ApiResponse<string> { Status = 0, Msg = "Không tìm thấy user" };

            if (Helper.HashPassword(user.PassWord) != Helper.HashPassword(passWord))
                return new ApiResponse<string> { Status = 0, Msg = "Mật khẩu không chính xác" };

            var token = Helper.GenerateToken(user.UserName);

            _cachingService.Add<UserProfile>(user, $"Token:{token}:User");

            return new ApiResponse<string> { Status = 1,Data = token };
        }

        public UserProfile GetUserByToken(string token = "")
        {
            return !string.IsNullOrEmpty(token) ?  _cachingService.Get<UserProfile>($"Token:{token}:User") : (UserProfile)null;
        }
    }
}
