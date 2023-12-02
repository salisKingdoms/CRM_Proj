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
using WS_CRM_Employee.Helper;
using WS_CRM_Employee.Feature.Employee.Dto;
using WS_CRM_Employee.Feature.Employee.Model;
using AutoMapper;
using WS_CRM_Employee.Config;

namespace WS_CRM_Employee.Feature.Employee.Dao
{
    public class EmployeeRepo : IEmployeeRepo
    {
        private DataContext _context;
        private readonly IMapper _mapper;
        public EmployeeRepo(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task CreateEmployee(ws_employee request)
        {
            using var connection = _context.CreateConnection();
            var lastCount = await GetCountEmployee();
            request.nip = "E" + DateTime.Now.Year.ToString() + "000" + (lastCount + 1);
            try
            {
                var sql = " INSERT INTO ws_employee" +
                        "(nip, name, phone, email, active, created_on, created_by, modified_on, modified_by)" +
                        "VALUES (@nip, @name, @phone, @email, @active, @created_on, @created_by, @modified_on, @modified_by )";
                var param = new Dictionary<string, object>
            {
                { "nip", request.nip },
                { "name", request.name  },
                { "phone", request.phone  },
                { "email", request.email ?? ""  },
                { "active", request.active },
                { "created_on", DateTime.UtcNow },
                { "created_by", request.created_by ?? ""},
                { "modified_on", null },
                { "modified_by", null}

            };
                await connection.ExecuteAsync(sql, param);
            }
            catch (Exception ex)
            {
                string s = ex.Message;
            }

        }

        private string QueryListEmployee(bool isList)
        {

            var query = (isList ? "SELECT * FROM ws_employee " : "SELECT COUNT (*) AS JUMLAH FROM ws_employee ");
            return query;
        }
        private async Task<IEnumerable<ws_employee>> RepoGetAllEmployee(string nip, string name)
        {
            using var connection = _context.CreateConnection();
            var sql = QueryListEmployee(true);
            string queryFilter = " where active=true ";
            if (!string.IsNullOrEmpty(nip))
            {
                queryFilter = " and nip=@nip ";
            }
            if (!string.IsNullOrEmpty(name))
            {
                queryFilter =  " and name=@name";
            }
            string sqlALL = sql + queryFilter;
            var param = new Dictionary<string, object>
            {
                { "nip", nip?? "" },
                { "name", name?? "" }
            };
            return await connection.QueryAsync<ws_employee>(sqlALL, param);
        }

        public async Task<List<ws_employee>> GetAllEmployee(string nip, string name)
        {
            var data = RepoGetAllEmployee(nip, name).Result.ToList();
            return data;
        }
        public async Task<int> RepoGetTotalAllEmployee(string nip, string name)
        {
            using var connection = _context.CreateConnection();
            var sql = QueryListEmployee(false);
            string queryFilter = " where active=true ";
            if (!string.IsNullOrEmpty(nip))
            {
                queryFilter = " and nip=@nip ";
            }
            if (!string.IsNullOrEmpty(name))
            {
                queryFilter = " and name=@name";
            }
            string sqlALL = sql + queryFilter;
            var param = new Dictionary<string, object>
            {
                { "nip", nip?? "" },
                { "name", name?? "" }
            };
            return await connection.QuerySingleOrDefaultAsync<int>(sqlALL, param);
        }

        public async Task DeleteEmployee(string nip)
        {
            using var connection = _context.CreateConnection();
            var sql = @" Update ws_employee SET active=false
                         WHERE nip = @nip";
            var param = new Dictionary<string, object>
            {
                { "nip", nip ?? ""  }
            };
            await connection.ExecuteAsync(sql, param);
        }

        public async Task<ws_employee> GetEmployeeByNIP(string nip)
        {
            using var connection = _context.CreateConnection();
            var sql = " select * from ws_employee where nip=@nip";
            var param = new Dictionary<string, object>
            {
                { "nip", nip  },

            };
            return await connection.QuerySingleOrDefaultAsync<ws_employee>(sql, param);
        }

        public async Task UpdateEmployee(ws_employee param)
        {
            using var connection = _context.CreateConnection();
            var sql = @"
            UPDATE ws_employee 
            SET name = @name,
                phone = @phone,
                email = @email, 
                active = @active,
                modified_by = @modified_by,
                modified_on = @modified_on
            WHERE nip = @nip";
            await connection.ExecuteAsync(sql, param);
        }

        private async Task<int> GetCountEmployee()
        {
            using var conn = _context.CreateConnection();
            var sql = @"select count(*) from ws_employee ";
            return await conn.QuerySingleOrDefaultAsync<int>(sql, null);
        }
    }
}
