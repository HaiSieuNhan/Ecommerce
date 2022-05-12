using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.Shared.Model
{
    public class ApiResponse<T>
    {
        public int Status { get; set; }
        public string Msg { get; set; }
        public T Data { get; set; }
    }

    public class ApiResponse
    {
        public int Status { get; set; }
        public string Msg { get; set; }
    }
}
