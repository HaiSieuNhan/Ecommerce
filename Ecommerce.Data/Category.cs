using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.Data
{
   public  class Category : AbstractEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
