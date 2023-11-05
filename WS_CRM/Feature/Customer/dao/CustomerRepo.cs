using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO; 
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using WS_CRM.Helper;
using WS_CRM.Feature.Customer.Model;

namespace WS_CRM.Feature.Customer.dao
{
    public class CustomerRepo : ICustomerRepo
    {
        private DataContext _context;

        public CustomerRepo(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Customers>> RepoGetAllCustomer()
        {
            using var connection = _context.CreateConnection();
            var sql = " SELECT * FROM Customers";
            return await connection.QueryAsync<Customers>(sql);
        }
    }
}
