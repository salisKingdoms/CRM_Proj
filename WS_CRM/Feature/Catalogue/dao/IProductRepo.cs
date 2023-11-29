using WS_CRM.Feature.Catalogue.dto;
using WS_CRM.Helper;
using WS_CRM.Feature.Catalogue.Model;
using System.Linq;
using Dapper;
using System;
using System.Collections.Generic;
using System.IO;

namespace WS_CRM.Feature.Catalogue.dao
{
    public interface IProductRepo
    {
        public Task CreateProduct(CreateProductParam request);
        public Task<List<ms_product>> GetAllProduct(GlobalFilter param);
        public Task<int> RepoGetTotalAllProduct(GlobalFilter param);
        public Task<ms_product> GetProductById(long id);
        public Task DeleteProductById(long id);
        public Task UpdateProduct(ms_product param);
    }
}
