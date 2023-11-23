﻿using System;
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
        public Task<List<Customers>> GetAll();
        //public Task<IEnumerable<Customers>> GetAll();
        public Task CreateCustomer(CreateCustomerRequest request);
        public Task<Customers> GetCustomerById(long id);
        public Task DeleteCustomerById(long id);
        public Task UpdateCustomer(Customers data);
        public Task<int> GetTotalAllCustomer();
        public Task<long> CreateMember(CreateMembersRequest request);
        public Task UpdateMemberCustomer(int cust_id);
        public Task DeleteMemberCustomer(int cust_id);
        //tes
    }
}
