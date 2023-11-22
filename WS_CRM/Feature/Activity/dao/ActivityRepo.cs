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
using WS_CRM.Feature.Activity.Model;
using WS_CRM.Feature.Activity.dto;
using WS_CRM.Feature.Customer.dto;
using WS_CRM.Feature.Customer.Model;

namespace WS_CRM.Feature.Activity.dao
{
    public class ActivityRepo : IActivityRepo
    {
        private DataContext _context;
        public ActivityRepo(DataContext context)
        {
            _context = context;
        }

        public async Task CreateWarranty(CreateActivationWarranty request)
        {
            using var connection = _context.ConnectionActivity();
            try
            {
                var sql = " INSERT INTO ws_warranty" +
                        "(warranty_no, company_code, receipt_no, receipt_date, invoice_no, invoice_date, article_code, article_name, serial_no, start_date,end_date,activate_by,activate_on,active,created_by,created_on,modified_by,modified_on,warranty_code" +
                        ")" +
                        "VALUES (@warranty_no, @company_code, @receipt_no, @receipt_date, @invoice_no, @invoice_date, @article_code, @article_name, @serial_no, @start_date,@end_date,@activate_by,@activate_on,@active,@created_by,@created_on,@modified_by,@modified_on,@warranty_code" +
                        ")";
                var param = new Dictionary<string, object>
            {
                { "warranty_no", request.warranty_no ?? "" },
                { "company_code", request.company_code ?? "" },
                { "receipt_no", request.receipt_no ?? "" },
                { "receipt_date", request.receipt_date ?? null },
                { "invoice_no", request.invoice_no },
                { "invoice_date", request.invoice_date ?? null },
                { "article_code", request.article_code ?? "" },
                { "article_name", request.article_name ?? "" },
                { "serial_no", request.serial_no ?? "" },
                { "start_date", request.start_date ?? null },
                { "end_date", request.end_date ?? null },
                { "activate_by", request.activate_by ?? "" },
                { "activate_on", request.activate_on ?? null },
                { "active", request.active},
                { "created_by", request.created_by ??""},
                { "created_on", request.created_on ?? null },
                { "modified_by",null },
                { "modified_on",null},
                { "warranty_code", request.warranty_code }

            };
                await connection.ExecuteAsync(sql, param);
            }
            catch(Exception ex)
            {
                string s = ex.Message;
            }

        }

        private string QueryListWarranty(bool isList)
        {
            var query = isList ? "SELECT * FROM ws_warranty" : "SELECT COUNT (*) AS JUMLAH FROM ws_warranty";
            return query;
        }
        public async Task<IEnumerable<ws_warranty>> RepoGetAllWarranty()
        {
            using var connection = _context.ConnectionActivity();
            var sql = QueryListWarranty(true);
            string sqlALL = sql + "limit @limit offset @offset";
            var param = new Dictionary<string, object>
            {
                { "limit",20},
                { "offset",0}
            };
            return await connection.QueryAsync<ws_warranty>(sql,param);
        }
        public async Task<int> RepoGetTotalAllWarranty()
        {
            using var connection = _context.ConnectionActivity();
            var sql = QueryListWarranty(false);
            return await connection.QuerySingleOrDefaultAsync<int>(sql);
        }
    }
}
