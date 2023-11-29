using WS_CRM.Feature.Catalogue.dto;
using WS_CRM.Helper;
using WS_CRM.Feature.Catalogue.Model;
using System.Linq;
using Dapper;

namespace WS_CRM.Feature.Catalogue.dao
{
    public interface IProductRepo
    {
        public Task CreateProduct(CreateProductParam request);
        public Task<IEnumerable<ms_product>> RepoGetAllProduct();
        public Task<int> RepoGetTotalAllProduct();
    }
}
