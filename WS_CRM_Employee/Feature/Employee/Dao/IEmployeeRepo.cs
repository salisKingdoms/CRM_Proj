using WS_CRM_Employee.Feature.Employee.Dto;
using WS_CRM_Employee.Helper;
using WS_CRM_Employee.Feature.Employee.Model;
using System.Linq;
using Dapper;
using System;
using System.Collections.Generic;
using System.IO;

namespace WS_CRM_Employee.Feature.Employee.Dao
{
    public interface IEmployeeRepo
    {
        public Task CreateEmployee(ws_employee request);
        public Task<List<ws_employee>> GetAllEmployee(string nip, string name);
        public Task<int> RepoGetTotalAllEmployee(string nip, string name);
        public Task DeleteEmployee(string nip);
        public Task<ws_employee> GetEmployeeByNIP(string nip);
        public Task UpdateEmployee(ws_employee param);

    }
}
