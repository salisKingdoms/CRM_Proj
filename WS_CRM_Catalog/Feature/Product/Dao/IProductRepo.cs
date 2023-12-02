using WS_CRM_Catalog.Feature.Product.Dto;
using WS_CRM_Catalog.Helper;
using WS_CRM_Catalog.Feature.Product.Model;
using System.Linq;
using Dapper;
using System;
using System.Collections.Generic;
using System.IO;

namespace WS_CRM_Catalog.Feature.Product.Dao
{
    public interface IProductRepo
    {
        public Task CreateProduct(CreateProductParam request);
        public Task<List<ms_product>> GetAllProduct(GlobalFilter param);
        public Task<int> RepoGetTotalAllProduct(GlobalFilter param);
        public Task<ms_product> GetProductById(long id);
        public Task DeleteProductById(long id);
        public Task UpdateProduct(ms_product param);
        public Task CreateSparepart(CreateSparepartParam request);
        public Task<List<ms_sparepart>> GetAllSparepart(GlobalFilter param);
        public Task<int> RepoGetTotalAllSparepart(GlobalFilter filter);
        public Task<ms_sparepart> GetSparepartById(long id);
        public Task DeleteSparepartById(long id);
        public Task UpdateSparepart(ms_sparepart param);
    }
}
