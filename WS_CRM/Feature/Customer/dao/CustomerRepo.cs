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
using WS_CRM.Feature.Customer.dto;

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

        public async Task CreateCustomer(CreateCustomerRequest request)
        {
            using var connection = _context.CreateConnection();
            var sql = " INSERT INTO Customers"+
                        "(id,name, email, phone, address, is_member, created_by, created_on, modified_by, modified_on)"+
                        "VALUES (@name, @email,@phone,@addres,@is_member,@created_by,@created_on,@modified_by,@modified_on)";
            var param = new Dictionary<string, object>
            {
                { "name", request.name ?? "" },
                { "email", request.email ?? "" },
                { "phone", request.phone ?? "" },
                { "addres", request.address ?? "" },
                { "is_member", request.is_member },
                { "created_by", request.created_by ?? "" },
                { "created_on", request.created_on },
                { "modified_by",null },
                { "modified_on",null}
            };
            await connection.ExecuteAsync(sql, param);
        }

        //public async Task Create(User user)
        //{
        //    using var connection = _context.CreateConnection();
        //    var sql = """
        //    INSERT INTO Users (Title, FirstName, LastName, Email, Role, PasswordHash)
        //    VALUES (@Title, @FirstName, @LastName, @Email, @Role, @PasswordHash)
        //""";
        //    await connection.ExecuteAsync(sql, user);
        //}
    }
}
