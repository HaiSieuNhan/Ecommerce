using Ecommerce.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.Service.Model.Category
{
    public class CategoryViewModel
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public Guid CategorytId { get; set; }
        public string CategoryName { get; set; }
        public Guid UserId { get; set; }
        public string NameUser { get; set; }
        public DateTime OrderDate { get; set; }
        public int Amount { get; set; }
    }

    public class SearchCategory : Paging
    {
        public string KeyWord { get; set; }

        public bool IsSortDesc { get; set; } = true;
    }
}
