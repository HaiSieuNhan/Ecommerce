using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.Data
{
    public class UserProfile : AbstractEntity
    {
        public string Name { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public string Address { get; set; }
    }
}
