using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Dapper;
//ing WebApi.Entities;
using WS_CRM.Helper;
using WS_CRM.Feature.Customer.dto;
using WS_CRM.Feature.Customer.Model;

namespace WS_CRM.Feature.Customer.dao
{
    public interface ICustomerDao
    {
        Task<IEnumerable<Customers>> GetAll();
        public Task CreateCustomer(CreateCustomerRequest request);
        public Task<Customers> GetCustomerById(long id);
        //tes
    }
}
