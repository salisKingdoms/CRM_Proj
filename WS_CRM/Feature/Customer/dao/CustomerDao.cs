using Dapper;
//ing WebApi.Entities;
using WS_CRM.Helper;
using WS_CRM.Feature.Customer.dto;
using AutoMapper;
//ing BCrypt.Net;
using WS_CRM.Feature.Customer.Model;

namespace WS_CRM.Feature.Customer.dao
{
    public class CustomerDao :ICustomerDao
    {
        private ICustomerRepo _custRepository;
        private readonly IMapper _mapper;
        
        public CustomerDao(ICustomerRepo custRepo, IMapper mapper)
        {
            _custRepository = custRepo;
            _mapper = mapper;
        }
        public async Task<List<Customers>> GetAll()
        {
            var data = _custRepository.RepoGetAllCustomer().Result.ToList();
            return data;
        }
        public async Task CreateCustomer(CreateCustomerRequest request)
        {
             await _custRepository.CreateCustomer(request);
        }
        public async Task<Customers> GetCustomerById(long id)
        {
           
            return await _custRepository.GetCustomerById(id);
        }
        public async Task DeleteCustomerById(long id)
        {

            await _custRepository.DeleteCustomerById(id);
        }
        public async Task UpdateCustomer(Customers data)
        {
            await _custRepository.UpdateCustomer(data);
        }
        public async Task<int> GetTotalAllCustomer()
        {

            return await _custRepository.RepoGetTotalAllCustomer();
        }

        #region Member
        public async Task<long> CreateMember(CreateMembersRequest request)
        {
            return await _custRepository.CreateMember(request);
        }
        public async Task UpdateMemberCustomer(int cust_id)
        {

            await _custRepository.UpdateMemberCustomer(cust_id);
        }
        public async Task DeleteMemberCustomer(int cust_id)
        {
            await _custRepository.DeleteMemberCustomer(cust_id);
        }
        #endregion

    }
}
