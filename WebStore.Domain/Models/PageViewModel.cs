using System;

namespace WebStore.Domain.Models
{
    public class PageViewModel
    {
        public int TotalItems { get; set; }

        public int PageSize { get; set; }

        public int PageNumber { get; set; }

        public int TotalPages => (TotalItems + PageSize - 1) / PageSize;
    }
}
