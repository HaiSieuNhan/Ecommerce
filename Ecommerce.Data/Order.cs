using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.Data
{
    public class Order : AbstractEntity
    {
        public string Name { get; set; }
        public Guid UserId { get; set; }
        public Guid ProductId { get; set; }
        public int Amount { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
