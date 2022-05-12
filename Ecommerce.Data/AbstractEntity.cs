using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.Data
{
   public class AbstractEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public int IsDeleted { get; set; } = 0;

        public Guid CreatedBy { get; set; } = Guid.Empty;
        /// <summary>
        /// DateTime.UtcNow
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Guid UpdatedBy { get; set; } = Guid.Empty;
        /// <summary>
        /// DateTime.UtcNow
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
