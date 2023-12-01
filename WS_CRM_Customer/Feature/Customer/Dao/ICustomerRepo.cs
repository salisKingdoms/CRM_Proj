using WS_CRM_Customer.Feature.Customer.Dto;
using WS_CRM_Customer.Helper;
using WS_CRM_Customer.Feature.Customer.Model;
using System.Linq;
using Dapper;
using System;
using WS_CRM_Customer.Feature.Customer.Dto;
using WS_CRM_Customer.Helper;
using WS_CRM_Customer.Feature.Customer.Model;
using System.Linq;
using Dapper;
using System;
using System.Collections.Generic;
using System.IO;

namespace WS_CRM_Customer.Feature.Customer.Dao
{
    public interface ICustomerRepo
    {
        public  Task CreateCustomer(CreateCustomerRequest request);
        public  Task<Customers> GetCustomerById(long id);
        public  Task<List<Customers>> GetAllCustomer(long id, string name);
        public  Task<int> RepoGetTotalAllCustomer();
        public  Task DeleteCustomerById(long id);
        public  Task UpdateCustomer(Customers cust);
        public  Task UpdateMemberCustomer(int cust_id);
        public  Task<long> CreateMember(CreateMembersRequest request);
        public  Task DeleteMemberCustomer(int cust_id);
    }
}
