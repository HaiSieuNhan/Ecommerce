using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.Data
{
   public class Product : AbstractEntity
    {
        public string Name { get; set; }
        public Guid CategoryId { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
    }
}
