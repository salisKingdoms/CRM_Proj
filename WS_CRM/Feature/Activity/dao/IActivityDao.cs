using WS_CRM.Feature.Activity.dto;
using WS_CRM.Feature.Activity.Model;

namespace WS_CRM.Feature.Activity.dao
{
    public interface IActivityDao
    {
        public Task CreateActivity(CreateActivationWarranty request);
        public Task<IEnumerable<ws_warranty>> GetAllWarranty();
        public Task<int> GetTotalAllWarranty();
    }
}
