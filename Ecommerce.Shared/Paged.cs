using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecommerce.Shared
{
    public interface IPaging
    {
        int? PageSize { get; set; }
        int? PageIndex { get; set; }
    }

    public class Paging : IPaging
    {
        /// <summary>
        /// Số bản ghi trên 1 trang
        /// </summary>
        public int? PageSize { get; set; }

        /// <summary>
        /// Trang hiện tại
        /// </summary>
        public int? PageIndex { get; set; }
    }

    public interface IPaged<T>
    {
        int Size { get; set; }
        int TotalItems { get; set; }
        int Page { get; set; }
        int TotalPages { get; set; }
        IEnumerable<T> Data { get; set; }
    }

    public class Paged<T> : IPaged<T>
    {
        public int Size { get; set; }
        public int TotalItems { get; set; }
        public int Page { get; set; }
        public int TotalPages { get; set; }
        public IEnumerable<T> Data { get; set; }

        public Paged() { }

        public Paged(IQueryable<T> source, IPaging paging)
        {
            var total = source.Count();

            if (paging == null)
            {
                Data = source.ToArray();
                TotalItems = total;
                return;
            }

            if (paging.PageSize == null || paging.PageIndex == null)
            {
                Data = source.ToArray();
                TotalItems = total;
                return;
            }

            Size = paging.PageSize ?? 10;
            Page = paging.PageIndex ?? 1;

            TotalPages = total / Size;
            if (total % paging.PageSize > 0)
                TotalPages++;

            TotalItems = total;
            Data = source.Skip((Page - 1) * Size).Take(Size).AsQueryable().ToList();
        }

        public Paged(IEnumerable<T> source, IPaging paging)
        {
            var total = source.Count();

            if (paging == null)
            {
                Data = source.ToArray();
                TotalItems = total;
                return;
            }

            if (paging.PageSize == null || paging.PageIndex == null)
            {
                Data = source.ToArray();
                TotalItems = total;
                return;
            }

            Size = paging.PageSize ?? 10;
            Page = paging.PageIndex ?? 1;

            TotalPages = total / Size;
            if (total % paging.PageSize > 0)
                TotalPages++;

            TotalItems = total;
            Data = source.Skip((Page - 1) * Size).Take(Size).AsQueryable().ToList();
        }

        public Paged<T1> Convert<T1>()
        {
            return new Paged<T1>
            {
                Size = this.Size,
                Page = this.Page,
                TotalItems = this.TotalItems,
                TotalPages = this.TotalPages,
            };
        }
    }
}
