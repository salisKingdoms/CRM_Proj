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
using WS_CRM.Feature.Catalog.Model;
using WS_CRM.Feature.Catalog.dto;
using AutoMapper;
using WS_CRM.Config;

namespace WS_CRM.Feature.Catalog.dao
{
    public class ProductRepo : IProductRepo
    {
        private DataContext _context;
        private readonly IMapper _mapper;
        public ProductRepo(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        #region Master Product
        public async Task<List<ms_product>> GetAllProduct(GlobalFilter param)
        {
            var data = RepoGetAllProduct(param).Result.ToList();
            return data;
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

        private string QueryListProduct(bool isList, string filters)
        {
            
            var query = (isList ? "SELECT * FROM ms_product " : "SELECT COUNT (*) AS JUMLAH FROM ms_product ") + filters;
            return query;
        }
        private async Task<IEnumerable<ms_product>> RepoGetAllProduct(GlobalFilter filter)
        {
            using var connection = _context.ConnectionCatalogue();
            ms_product_database_filter dbModel = new ms_product_database_filter();
            (string, Dictionary<string, object>) whereParam = CustomUtility.GetWhere(dbModel, false, filter);
            var sql = QueryListProduct(true, whereParam.Item1);
            string sqlALL = sql + "limit @limit offset @offset";
            var param = new Dictionary<string, object>
            {
                { "limit",filter.limit ?? 0},
                { "offset",filter.offset??0}
            };
            return await connection.QueryAsync<ms_product>(sql, param.Concat(whereParam.Item2).ToDictionary(x=>x.Key, x=>x.Value));
        }
        public async Task<int> RepoGetTotalAllProduct(GlobalFilter filter)
        {
            using var connection = _context.ConnectionCatalogue();
            ms_product_database_filter dbModel = new ms_product_database_filter();
            (string, Dictionary<string, object>) whereParam = CustomUtility.GetWhere(dbModel, false, filter);
            var sql = QueryListProduct(false, whereParam.Item1);
            var param = new Dictionary<string, object>
            {

            };
            return await connection.QuerySingleOrDefaultAsync<int>(sql, param.Concat(whereParam.Item2).ToDictionary(x => x.Key, x => x.Value));
        }

        public async Task<ms_product> GetProductById(long id)
        {
            using var connection = _context.ConnectionCatalogue();
            var sql = " select * from ms_product where id=@id";
            var param = new Dictionary<string, object>
            {
                { "id", id  },

            };
            return await connection.QuerySingleOrDefaultAsync<ms_product>(sql, param);
        }

        public async Task DeleteProductById(long id)
        {
            using var connection = _context.ConnectionCatalogue();
            var sql = " delete from ms_product where id=@id";
            var param = new Dictionary<string, object>
            {
                { "id", id  },

            };
            await connection.ExecuteAsync(sql, param);
        }

        public async Task UpdateProduct(ms_product param)
        {
            using var connection = _context.ConnectionCatalogue();
            var sql = @"
            UPDATE ms_product 
            SET product_name = @product_name,
                qty = @qty,
                is_trade_in = @is_trade_in, 
                unit_line_no = @unit_line_no, 
                modified_by = @modified_by,
                modified_on = @modified_on
            WHERE id = @id";
            await connection.ExecuteAsync(sql, param);
        }
        #endregion

        #region Master Sparepart
        public async Task CreateSparepart(CreateSparepartParam request)
        {
            using var connection = _context.ConnectionCatalogue();
            try
            {
                var sql = " INSERT INTO ms_sparepart" +
                        "(sparepart_code, sparepart_name, sku_code, sku_name, uom,qty,warranty_duration,created_by,created_on,modified_by,modified_on)" +
                        "VALUES (@sparepart_code, @sparepart_name, @sku_code, @sku_name, @uom,@qty,@warranty_duration,@created_by,@created_on,@modified_by,@modified_on" +
                        ")";
                var param = new Dictionary<string, object>
            {
                { "sparepart_code", request.sparepart_code ?? "" },
                { "sparepart_name", request.sparepart_name ?? "" },
                { "sku_code", request.sku_code ?? "" },
                { "sku_name", request.sku_name ?? "" },
                { "uom", request.uom ??""  },
                { "qty", request.qty ?? 0 },
                { "warranty_duration", request.warranty_duration ??0  },
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

        private string QueryListSparepart(bool isList, string filters)
        {

            var query = (isList ? "SELECT * FROM ms_sparepart " : "SELECT COUNT (*) AS JUMLAH FROM ms_sparepart ") + filters;
            return query;
        }
        private async Task<IEnumerable<ms_sparepart>> RepoGetAllSparepart(GlobalFilter filter)
        {
            using var connection = _context.ConnectionCatalogue();
            ms_sparepart_database_filter dbModel = new ms_sparepart_database_filter();
            (string, Dictionary<string, object>) whereParam = CustomUtility.GetWhere(dbModel, false, filter);
            var sql = QueryListSparepart(true, whereParam.Item1);
            string sqlALL = sql + "limit @limit offset @offset";
            var param = new Dictionary<string, object>
            {
                { "limit",filter.limit ?? 0},
                { "offset",filter.offset??0}
            };
            return await connection.QueryAsync<ms_sparepart>(sql, param.Concat(whereParam.Item2).ToDictionary(x => x.Key, x => x.Value));
        }

        public async Task<List<ms_sparepart>> GetAllSparepart(GlobalFilter param)
        {
            var data = RepoGetAllSparepart(param).Result.ToList();
            return data;
        }

        public async Task<int> RepoGetTotalAllSparepart(GlobalFilter filter)
        {
            using var connection = _context.ConnectionCatalogue();
            ms_sparepart_database_filter dbModel = new ms_sparepart_database_filter();
            (string, Dictionary<string, object>) whereParam = CustomUtility.GetWhere(dbModel, false, filter);
            var sql = QueryListSparepart(false, whereParam.Item1);
            var param = new Dictionary<string, object>
            {

            };
            return await connection.QuerySingleOrDefaultAsync<int>(sql, param.Concat(whereParam.Item2).ToDictionary(x => x.Key, x => x.Value));
        }

        public async Task<ms_sparepart> GetSparepartById(long id)
        {
            using var connection = _context.ConnectionCatalogue();
            var sql = " select * from ms_sparepart where id=@id";
            var param = new Dictionary<string, object>
            {
                { "id", id  },

            };
            return await connection.QuerySingleOrDefaultAsync<ms_sparepart>(sql, param);
        }

        public async Task DeleteSparepartById(long id)
        {
            using var connection = _context.ConnectionCatalogue();
            var sql = " delete from ms_sparepart where id=@id";
            var param = new Dictionary<string, object>
            {
                { "id", id  },

            };
            await connection.ExecuteAsync(sql, param);
        }

        public async Task UpdateSparepart(ms_sparepart param)
        {
            using var connection = _context.ConnectionCatalogue();
            var sql = @"
            UPDATE ms_sparepart 
            SET sparepart_name = @sparepart_name,
                sku_code = @sku_code,
                sku_name = @sku_name, 
                uom = @uom, 
                qty = @qty,
                warranty_duration = @warranty_duration,
                modified_by = @modified_by,
                modified_on = @modified_on
            WHERE id = @id";
            await connection.ExecuteAsync(sql, param);
        }
        #endregion
    }
}
