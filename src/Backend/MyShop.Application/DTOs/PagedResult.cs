using System.Collections.Generic;
using System;

namespace MyShop.Application.DTOs.Common
{
   
    public class PagedResult<T>
    {
        public List<T> Items { get; set; }
        public int TotalRecords { get; set; } 
        public int PageIndex { get; set; }    
        public int PageSize { get; set; }     
        public int TotalPages => (int)Math.Ceiling((double)TotalRecords / PageSize);

        public PagedResult(List<T> items, int totalRecords, int pageIndex, int pageSize)
        {
            Items = items;
            TotalRecords = totalRecords;
            PageIndex = pageIndex;
            PageSize = pageSize;
        }
    }
}