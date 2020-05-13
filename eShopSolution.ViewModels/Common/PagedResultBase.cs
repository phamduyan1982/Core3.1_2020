using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModels.Common
{
    public class PagedResultBase
    {
        public int PageIndex { set; get; }
        public int PageSize { set; get; }
        public int TotalRecords { set; get; }

        public int PageCount
        {
            get
            {
                var pageCount = (double)TotalRecords / PageSize;
                return (int)Math.Ceiling(pageCount);
            }
        }

    }
}
