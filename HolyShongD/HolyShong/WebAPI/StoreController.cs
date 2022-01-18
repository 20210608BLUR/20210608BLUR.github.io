using HolyShong.Services;
using HolyShong.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HolyShong.WebAPI
{
    public class StoreController : ApiController
    {
        private readonly ProductService _productService;
        public StoreController()
        {
            _productService = new ProductService();
        }


        [HttpPost]
        public IHttpActionResult GetProductModal(ProductCardViewModel productCard)
        {
            var result = _productService.GetStoreProductByProductId(productCard.ProductId);
            return Ok(result);
        }

    }
}
