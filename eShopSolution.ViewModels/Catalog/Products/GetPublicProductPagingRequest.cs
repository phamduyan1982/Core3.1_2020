//using eShopSolution.Application.Dtos;
using eShopSolution.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModels.Catalog.Products
{
    public class GetPublicProductPagingRequest : PagingRequestBase
    {
        public int? CategoryId { set; get; }
        //public string Keyword { set; get; }
        //public List<int> categoryIds { set; get; }
        //public int CategoryId { get; internal set; }
    }
}
