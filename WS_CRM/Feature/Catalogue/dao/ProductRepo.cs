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
using WS_CRM.Feature.Catalogue.Model;
using WS_CRM.Feature.Catalogue.dto;


namespace WS_CRM.Feature.Catalogue.dao
{
    public class ProductRepo : IProductRepo
    {
        private DataContext _context;
        public ProductRepo(DataContext context)
        {
            _context = context;
        }
        public async Task CreateProduct(CreateProductParam request)
        {
            using var connection = _context.ConnectionCatalogue();
            try
            {
                var sql = " INSERT INTO ms_product" +
                        "(sku_code, product_name, qty, is_trade_in, unit_line_no,created_by,created_on,modified_by,modified_on)" +
                        "VALUES (@sku_code, @product_name, @qty, @is_trade_in, @unit_line_no,@created_by,@created_on,@modified_by,@modified_on" +
                        ")";
                var param = new Dictionary<string, object>
            {
                { "sku_code", request.sku_code ?? "" },
                { "product_name", request.product_name ?? "" },
                { "qty", request.qty },
                { "is_trade_in", request.is_trade_in ?? false },
                { "unit_line_no", request.unit_line_no ??0  },
                { "created_by", request.created_by ??""},
                { "created_on", request.created_on ?? null },
                { "modified_by",null },
                { "modified_on",null}
            };
                await connection.ExecuteAsync(sql, param);
            }
            catch (Exception ex)
            {
                string s = ex.Message;
            }

        }

        private string QueryListProduct(bool isList)
        {
            var query = isList ? "SELECT * FROM ms_product" : "SELECT COUNT (*) AS JUMLAH FROM ms_product";
            return query;
        }
        public async Task<IEnumerable<ms_product>> RepoGetAllProduct()
        {
            using var connection = _context.ConnectionCatalogue();
            var sql = QueryListProduct(true);
            string sqlALL = sql + "limit @limit offset @offset";
            var param = new Dictionary<string, object>
            {
                { "limit",20},
                { "offset",0}
            };
            return await connection.QueryAsync<ms_product>(sql, param);
        }
        public async Task<int> RepoGetTotalAllProduct()
        {
            using var connection = _context.ConnectionCatalogue();
            var sql = QueryListProduct(false);
            return await connection.QuerySingleOrDefaultAsync<int>(sql);
        }
    }
}
