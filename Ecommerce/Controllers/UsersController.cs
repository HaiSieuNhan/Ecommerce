using Ecommerce.Shared;
using Ecommerce.Shared.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Ecommerce.Controllers
{
    [Microsoft.AspNetCore.Components.Route("api/users")]
    [ApiController]
    [AuthorizationAttribute]
    public class UsersController
    {
        [HttpPost("get-all")]
        public async Task<ApiResponse<string>> GetAll([FromBody] string search)
        {
            return new ApiResponse<string>
            {
                Status = 1,
                Data = "Dao Hai"
            };
        }
    }
}
