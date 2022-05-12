using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Models.Request
{
    public class CreateUser
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }
    }
}
