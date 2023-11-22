using AutoMapper;
using WS_CRM.Feature.Activity.dto;
using WS_CRM.Feature.Activity.Model;
using WS_CRM.Feature.Customer.dao;
using WS_CRM.Feature.Customer.dto;
using WS_CRM.Feature.Customer.Model;

namespace WS_CRM.Feature.Activity.dao
{
    public class ActivityDao: IActivityDao
    {
        private IActivityRepo _actRepository;
        private readonly IMapper _mapper;

        public ActivityDao(IActivityRepo actRepo, IMapper mapper)
        {
            _actRepository = actRepo;
            _mapper = mapper;
        }
        public async Task CreateActivity(CreateActivationWarranty request)
        {
            await _actRepository.CreateWarranty(request);
        }

        public async Task<IEnumerable<ws_warranty>> GetAllWarranty()
        {
            return await _actRepository.RepoGetAllWarranty();
        }
        public async Task<int> GetTotalAllWarranty()
        {

            return await _actRepository.RepoGetTotalAllWarranty();
        }
    }
}
