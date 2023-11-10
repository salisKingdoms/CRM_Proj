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

        private string QueryListCustomer(bool isList)
        {
            var query = isList ? "SELECT * FROM ws_customer" :"SELECT COUNT (*) AS JUMLAH FROM ws_customer" ;
            return query;
        }
        public async Task<IEnumerable<Customers>> RepoGetAllCustomer()
        {
            using var connection = _context.CreateConnection();
            var sql = QueryListCustomer(true);
            return await connection.QueryAsync<Customers>(sql);
        }
        public async Task<int> RepoGetTotalAllCustomer()
        {
            using var connection = _context.CreateConnection();
            var sql = QueryListCustomer(false);
            return await connection.QuerySingleOrDefaultAsync<int>(sql);
        }
        public async Task CreateCustomer(CreateCustomerRequest request)
        {
            using var connection = _context.CreateConnection();
            var sql = " INSERT INTO ws_customer" +
                        "(name, email, phone, address, is_member, created_by, created_on, modified_by, modified_on)" +
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

        public async Task<Customers> GetCustomerById(long id)
        {
            using var connection = _context.CreateConnection();
            var sql = " select * from ws_customer where id=@id";
            var param = new Dictionary<string, object>
            {
                { "id", id  },

            };
            return await connection.QuerySingleOrDefaultAsync<Customers>(sql, param);
        }

        public async Task DeleteCustomerById(long id)
        {
            using var connection = _context.CreateConnection();
            var sql = " delete from ws_customer where id=@id";
            var param = new Dictionary<string, object>
            {
                { "id", id  },

            };
            await connection.ExecuteAsync(sql, param);
        }

        public async Task UpdateCustomer(Customers cust)
        {
            using var connection = _context.CreateConnection();
            var sql = @"
            UPDATE ws_customer 
            SET name = @name,
                email = @email,
                phone = @phone, 
                address = @address, 
                is_member = @is_member, 
                modified_by = @modified_by,
                modified_on = @modified_on
            WHERE id = @id";
            await connection.ExecuteAsync(sql, cust);
        }

        public async Task UpdateMemberCustomer(int cust_id)
        {
            using var connection = _context.CreateConnection();
            var sql = @"
            UPDATE ws_customer 
            SET 
                is_member = true, 
                modified_by = @modified_by,
                modified_on = @modified_on
            WHERE id = @id";

            var param = new Dictionary<string, object>
            {
                { "cust_id", cust_id },
                { "modified_by","sys" },
                { "modified_on",DateTime.UtcNow}
            };
            await connection.ExecuteAsync(sql, param);
        }

        public async Task<long> CreateMember(CreateMembersRequest request)
        {
            using var connection = _context.CreateConnection();
            var sql = " INSERT INTO ws_member" +
                        "(cust_id, user_name, password, created_by, created_on, modified_by,modified_on,is_active)" +
                        "VALUES (@cust_id,@user_name,@password,@created_by,@created_on,@modified_by,@modified_on,@is_active)" +
                        " RETURNING cust_id;";
            var param = new Dictionary<string, object>
            {
                { "cust_id", request.customer_id  },
                { "user_name", request.user_name ?? "" },
                { "password", request.password ?? "" },
                { "created_by", "user" },
                { "created_on", DateTime.UtcNow },
                { "modified_by",null },
                { "modified_on",null},
                { "is_active",true }
            };
            var datas = await  connection.ExecuteScalarAsync<long>(sql, param);

            return datas;
        }

        public async Task DeleteMemberCustomer(int cust_id)
        {
            using var connection = _context.CreateConnection();
            var sql = @"
            UPDATE ws_member 
            SET 
                is_active = false,
                modified_by = @modified_by,
                modified_on = @modified_on
            WHERE id = @id

            UPDATE ws_customer
            SET
                is_member = false,
                modified_by = @modified_by,
                modified_on = @modified_on
            WHERE id = @id";

            var param = new Dictionary<string, object>
            {
                { "modified_by","sys" },
                { "modified_on",DateTime.UtcNow}
            };
            await connection.ExecuteAsync(sql, param);
        }
    }
}
