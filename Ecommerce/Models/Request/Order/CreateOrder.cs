using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Models.Request.Order
{
    public class CreateOrder
    {
        public string Name { get; set; }
        public Guid UserId { get; set; }
        public Guid ProductId { get; set; }
        public int Amount { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
