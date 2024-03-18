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
using AutoMapper;
using WS_CRM.Config;

namespace WS_CRM.Feature.Activity.dao
{
    public class ActivityRepo : IActivityRepo
    {
        private DataContext _context;
        private readonly IMapper _mapper;
        public ActivityRepo(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<ws_warranty>> GetAllWarranty()
        {
            var data = RepoGetAllWarranty().Result.ToList();
            return data;
        }
        public async Task CreateWarranty(CreateActivationWarranty request)
        {
            using var connection = _context.CreateConnection();
            try
            {
                var sql = " INSERT INTO ws_warranty" +
                        "(warranty_no, company_code, invoice_no, invoice_date, article_code, article_name, serial_no, start_date,end_date,activate_by,activate_on,active,created_by,created_on,modified_by,modified_on,warranty_code" +
                        ")" +
                        "VALUES (@warranty_no, @company_code, @invoice_no, @invoice_date, @article_code, @article_name, @serial_no, @start_date,@end_date,@activate_by,@activate_on,@active,@created_by,@created_on,@modified_by,@modified_on,@warranty_code" +
                        ")";
                var param = new Dictionary<string, object>
            {
                { "warranty_no", request.warranty_no ?? "" },
                { "company_code", request.company_code ?? "" },
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
            query += " where active=true ";
            return query;
        }
        private async Task<IEnumerable<ws_warranty>> RepoGetAllWarranty()
        {
            using var connection = _context.CreateConnection();
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
            using var connection = _context.CreateConnection();
            var sql = QueryListWarranty(false);
            return await connection.QuerySingleOrDefaultAsync<int>(sql);
        }

        public async Task<ws_warranty> GetWarrantyById(long id)
        {
            using var connection = _context.CreateConnection();
            var sql = " select * from ws_warranty where id=@id";
            var param = new Dictionary<string, object>
            {
                { "id", id  },

            };
            return await connection.QuerySingleOrDefaultAsync<ws_warranty>(sql, param);
        }

        public async Task DeleteWarrantyById(long id)
        {
            using var connection = _context.CreateConnection();
            var sql = @" UPDATE ws_warranty 
            SET active = false,
                modified_by = @modified_by,
                modified_on = @modified_on
            WHERE id = @id"; 
            var param = new Dictionary<string, object>
            {
                { "id", id  },

            };
            await connection.ExecuteAsync(sql, param);
        }

        public async Task UpdateWarranty(ws_warranty param)
        {
            using var connection = _context.CreateConnection();
            var sql = @"
            UPDATE ws_warranty 
            SET company_code = @company_code,
                invoice_no = @invoice_no,
                invoice_date = @invoice_date, 
                article_code = @article_code, 
                article_name = @article_name,
                serial_no = @serial_no,
                start_date = @start_date,
                end_date = @end_date,
                modified_by = @modified_by,
                modified_on = @modified_on
            WHERE id = @id";
            await connection.ExecuteAsync(sql, param);
        }
       
        public async Task CreateTicketService(CreateTicket request)
        {
            using var connection = _context.CreateConnection();
            try
            {
                var sql = " INSERT INTO ws_ticket" +
                        "(ticket_no, status, customer_id, service_center, assign_to, payment_method,created_by,created_on,modified_by,modified_on " +
                        ")" +
                        "VALUES (@ticket_no, @status, @customer_id, @service_center, @assign_to, @payment_method,@created_by,@created_on,@modified_by,@modified_on" +
                        ")";
                var param = new Dictionary<string, object>
            {
                { "ticket_no", request.ticket_no ?? "" },
                { "status", request.status ?? "" },
                { "customer_id", request.customer_id },
                { "service_center", request.service_center ?? "" },
                { "assign_to", request.assign_to ?? "" },
                { "payment_method", request.payment_method ?? "" },
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

        private string QueryListTicket(bool isList, string filters)
        {
            //(isList ? "SELECT * FROM ms_product " : "SELECT COUNT (*) AS JUMLAH FROM ms_product ") + filters;
            var query = (isList ? "SELECT * FROM ws_ticket " : "SELECT COUNT (*) AS JUMLAH FROM ws_ticket ") + filters;
            return query;
        }
        private async Task<IEnumerable<ws_ticket>> RepoGetAllTicket(GlobalFilter filter)
        {
            using var connection = _context.CreateConnection();
            ws_ticket_database_filter dbModel = new ws_ticket_database_filter();
            (string, Dictionary<string, object>) whereParam = CustomUtility.GetWhere(dbModel, false, filter);
            var sql = QueryListTicket(true, whereParam.Item1);
            if(!whereParam.Item1.Contains(" "))
            {
                sql += " and active=true";
            }
            else
            {
                sql += " where active=true";
            }
            string sqlALL = sql + " limit @limit offset @offset";
            var param = new Dictionary<string, object>
            {
                { "limit",filter.limit ?? 0},
                { "offset",filter.offset??0}
            };

            return await connection.QueryAsync<ws_ticket>(sqlALL, param.Concat(whereParam.Item2).ToDictionary(x => x.Key, x => x.Value));

        }
        public async Task<int> RepoGetTotalAllTicket(GlobalFilter filter)
        {
            using var connection = _context.CreateConnection();
            ws_ticket_database_filter dbModel = new ws_ticket_database_filter();
            (string, Dictionary<string, object>) whereParam = CustomUtility.GetWhere(dbModel, false, filter);
            var sql = QueryListTicket(false, whereParam.Item1);
            if (!whereParam.Item1.Contains(" "))
            {
                sql += " and active=true";
            }
            else
            {
                sql += " where active=true";
            }
            var param = new Dictionary<string, object>
            {

            };
            return await connection.QuerySingleOrDefaultAsync<int>(sql, param.Concat(whereParam.Item2).ToDictionary(x => x.Key, x => x.Value));
        }

        public async Task<List<ws_ticket>> GetAllTicketHeader(GlobalFilter filter)
        {
            var data = RepoGetAllTicket(filter).Result.ToList();
            return data;
        }

        public async Task<ws_ticket> GetTicketHeaderByTicketNo(string ticket_no)
        {
            using var connection = _context.CreateConnection();
            var sql = " select * from ws_ticket where ticket_no=@ticket_no";
            var param = new Dictionary<string, object>
            {
                { "ticket_no", ticket_no  },

            };
            return await connection.QuerySingleOrDefaultAsync<ws_ticket>(sql, param);
        }
        public async Task UpdateTicketHeader(ws_ticket param)
        {
            using var connection = _context.CreateConnection();
            var sql = @"
            UPDATE ws_ticket 
            SET status = @status,
                assign_to = @assign_to,
                payment_method = @payment_method, 
                modified_by = @modified_by,
                modified_on = @modified_on
            WHERE id = @id";
            await connection.ExecuteAsync(sql, param);
        }
        public async Task CreateTicketUnit(CreateTicketUnit request)
        {
            using var connection = _context.CreateConnection();
            try
            {
                var sql = " INSERT INTO ws_ticket_unit" +
                        "(ticket_no, sku_code, product_name, qty, unit_line_no, warranty_no,active,created_by,created_on,modified_by,modified_on " +
                        ")" +
                        "VALUES (@ticket_no, @sku_code, @product_name, @qty, @unit_line_no, @warranty_no,@active,@created_by,@created_on,@modified_by,@modified_on " +
                        ")";
                var param = new Dictionary<string, object>
            {
                { "ticket_no", request.ticket_no ?? "" },
                { "sku_code", request.sku_code ?? "" },
                { "product_name", request.product_name ?? "" },
                { "qty", request.qty  },
                { "unit_line_no", request.unit_line_no  },
                { "warranty_no", request.warranty_no ?? "" },
                { "active", request.active  },
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

        private string QueryListTicketUnit(bool isList)
        {

            var query = (isList ? "SELECT * FROM ws_ticket_unit " : "SELECT COUNT (*) AS JUMLAH FROM ws_ticket_unit ");
            return query;
        }
        private async Task<IEnumerable<ws_ticket_unit>> RepoGetAllTicketUnit(string ticket_no)
        {
            using var connection = _context.CreateConnection();
            var sql = QueryListTicketUnit(true);
            string queryFilter = " where ticket_no=@ticket_no ";
            string sqlALL = sql + queryFilter ;
            var param = new Dictionary<string, object>
            {
                { "ticket_no", ticket_no?? "" }
            };
            return await connection.QueryAsync<ws_ticket_unit>(sqlALL, param);
        }

        public async Task<List<ws_ticket_unit>> GetAllTicketUnit(string ticket_no)
        {
            var data = RepoGetAllTicketUnit(ticket_no).Result.ToList();
            return data;
        }
        public async Task<int> RepoGetTotalAllTicketUnit(string ticket_no)
        {
            using var connection = _context.CreateConnection();
            var sql = QueryListTicketUnit(false);
            string queryFilter = " where ticket_no=@ticket_no ";
            string sqlALL = sql + queryFilter;
            var param = new Dictionary<string, object>
            {
                { "ticket_no", ticket_no?? "" }
            };
            return await connection.QuerySingleOrDefaultAsync<int>(sqlALL, param);
        }

        public async Task DeleteTicketUnit(string ticket_no, int? unit_line)
        {
            using var connection = _context.CreateConnection();
            var sql = @" UPDATE ws_ticket_unit 
            SET active = false,
                modified_by = @modified_by,
                modified_on = @modified_on
            WHERE ticket_no = @ticket_no and unit_line_no=@unit_line";
            var param = new Dictionary<string, object>
            {
                { "ticket_no", ticket_no ?? ""  },
                { "unit_line",unit_line ?? null }

            };
            await connection.ExecuteAsync(sql, param);
        }

        public async Task UpdateTicketUnit(ws_ticket_unit param)
        {
            using var connection = _context.CreateConnection();
            var sql = @"
            UPDATE ws_ticket_unit 
            SET sku_code = @sku_code,
                product_name = @product_name,
                qty = @qty, 
                modified_by = @modified_by,
                modified_on = @modified_on
            WHERE id = @id";
            await connection.ExecuteAsync(sql, param);
        } 

        public async Task CreateTicketSparepart(CreateTicketSparepart request)
        {
            using var connection = _context.CreateConnection();
            try
            {
                var sql = " INSERT INTO ws_ticket_sparepart" +
                        "(ticket_no, sparepart_code, sparepart_name, product_name, qty, unit_line_no,uom,created_by,created_on,modified_by,modified_on " +
                        ")" +
                        "VALUES (@ticket_no, @sparepart_code, @sparepart_name, @product_name, @qty, @unit_line_no,@uom,@created_by,@created_on,@modified_by,@modified_on " +
                        ")";
                var param = new Dictionary<string, object>
            {
                { "ticket_no", request.ticket_no ?? "" },
                { "sparepart_code", request.sparepart_code ?? "" },
                { "sparepart_name", request.sparepart_name ?? "" },
                { "product_name", request.product_name ?? ""  },
                { "qty", request.qty  },
                { "unit_line_no", request.unit_line_no  },
                { "uom", request.uom  },
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

        private string QueryListTicketSparepart(bool isList)
        {

            var query = (isList ? "SELECT * FROM ws_ticket_sparepart " : "SELECT COUNT (*) AS JUMLAH FROM ws_ticket_sparepart ");
            return query;
        }
        private async Task<IEnumerable<ws_ticket_sparepart>> RepoGetAllTicketSparepart(string ticket_no)
        {
            using var connection = _context.CreateConnection();
            var sql = QueryListTicketUnit(true);
            string queryFilter = " where ticket_no=@ticket_no ";
            string sqlALL = sql + queryFilter;
            var param = new Dictionary<string, object>
            {
                { "ticket_no", ticket_no?? "" }
            };
            return await connection.QueryAsync<ws_ticket_sparepart>(sqlALL, param);
        }

        public async Task<List<ws_ticket_sparepart>> GetAllTicketSparepart(string ticket_no)
        {
            var data = RepoGetAllTicketSparepart(ticket_no).Result.ToList();
            return data;
        }
        public async Task<int> RepoGetTotalAllTicketSparepart(string ticket_no)
        {
            using var connection = _context.CreateConnection();
            var sql = QueryListTicketSparepart(false);
            string queryFilter = " where ticket_no=@ticket_no ";
            string sqlALL = sql + queryFilter;
            var param = new Dictionary<string, object>
            {
                { "ticket_no", ticket_no?? "" }
            };
            return await connection.QuerySingleOrDefaultAsync<int>(sqlALL, param);
        }

        public async Task DeleteTicketSparepart(string ticket_no, int? unit_line)
        {
            using var connection = _context.CreateConnection();
            var sql = @" UPDATE ws_ticket_sparepart 
            SET active = false,
                modified_by = @modified_by,
                modified_on = @modified_on
            WHERE ticket_no = @ticket_no and unit_line_no=@unit_line";
            var param = new Dictionary<string, object>
            {
                { "ticket_no", ticket_no ?? ""  },
                { "unit_line",unit_line ?? null }

            };
            await connection.ExecuteAsync(sql, param);
        }

        public async Task UpdateTicketSparepart(ws_ticket_unit param)
        {
            using var connection = _context.CreateConnection();
            var sql = @"
            UPDATE ws_ticket_sparepart 
            SET sparepart_name = @sparepart_name,
                product_name = @product_name,
                unit_line_no = @unit_line_no,
                uom = @product_name,
                qty = @qty, 
                modified_by = @modified_by,
                modified_on = @modified_on
            WHERE ticket_no = @ticket_no";
            await connection.ExecuteAsync(sql, param);
        }
        public async Task<APIResult<CustomerRespon>> GetCustomerById(string endpoint)
        {
            return CallAPIHelper.RunAPIServiceRequestGET(new APIResult<CustomerRespon>(), endpoint);
        }

        public async Task UpdateTicketStatus(ws_ticket request)
        {
            using var connection = _context.CreateConnection();
            var sql = @"
            UPDATE ws_ticket 
            SET status = @status,
                modified_by = @modified_by,
                modified_on = @modified_on
            WHERE ticket_no = @ticket_no";

            var param = new Dictionary<string, object>
            {
                { "ticket_no", request.ticket_no ?? ""  },
                { "status",request.status ?? "" },
                { "modified_by", request.modified_by??"" },
                { "modified_on", request.modified_on ?? null}

            };
            await connection.ExecuteAsync(sql, param);
        }

        public async Task NonActiveTicketHeader(string ticket_no)
        {
            using var connection = _context.CreateConnection();
            var sql = @"
            UPDATE ws_ticket 
            SET active = false,
                modified_by = @modified_by,
                modified_on = @modified_on
            WHERE ticket_no = @ticket_no";

            var param = new Dictionary<string, object>
            {
                { "ticket_no", ticket_no ?? ""  },
                { "modified_by","sys" },
                { "modified_on", DateTime.UtcNow}

            };
            await connection.ExecuteAsync(sql, param);
        }

        public async Task<APIResult<EmployeeRespon>> GetEmployeeByNIP(string endpoint)
        {
            return CallAPIHelper.RunAPIServiceRequestGET(new APIResult<EmployeeRespon>(), endpoint);
        }

    }
}
