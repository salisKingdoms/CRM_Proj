
//ing WebApi.Entities;
using WS_CRM.Helper;
using WS_CRM.Feature.Customer.dto;
using WS_CRM.Feature.Customer.Model;
using System.Linq;
using Dapper;

namespace WS_CRM.Feature.Customer.dao
{
    public interface ICustomerRepo
    {
        public Task<IEnumerable<Customers>> RepoGetAllCustomer();
        public Task CreateCustomer(CreateCustomerRequest request);
        public Task<Customers> GetCustomerById(long id);
        public Task DeleteCustomerById(long id);
        public Task UpdateCustomer(Customers cust);
        public Task<int> RepoGetTotalAllCustomer();

        #region member
        public Task<long> CreateMember(CreateMembersRequest request);
        public Task UpdateMemberCustomer(int cust_id);
        #endregion
    }
}
