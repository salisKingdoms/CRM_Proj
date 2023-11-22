using WS_CRM.Feature.Activity.dto;
using WS_CRM.Helper;
using WS_CRM.Feature.Activity.Model;
using System.Linq;
using Dapper;

namespace WS_CRM.Feature.Activity.dao
{
    public interface IActivityRepo
    {
        public Task CreateWarranty(CreateActivationWarranty request);
        public Task<IEnumerable<ws_warranty>> RepoGetAllWarranty();
        public Task<int> RepoGetTotalAllWarranty();
    }
}
