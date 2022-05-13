using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.Data
{
    public class LogException : AbstractEntity
    {
        public string Exception { get; set; }

        public string InnerException { get; set; }

        public string StackTrace { get; set; }

        public string DataJson { get; set; }

        public int Type { get; set; }

        public string FunctionName { get; set; }
    }
}
