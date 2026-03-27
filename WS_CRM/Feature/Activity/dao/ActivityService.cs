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
using WS_CRM.BackgroundJob;

namespace WS_CRM.Feature.Activity.dao {
    
    public class ActivityService
    {
         IActivityRepo _actDao;
         private readonly IMapper _mapper;
         private readonly DataContext _context;
         private readonly IBackgroundTaskQueue _queue;
        private readonly AppConfig _appConfig;
        public ActivityService(IActivityRepo actDao,IMapper mapper, DataContext context,IBackgroundTaskQueue queue, AppConfig config)
        {
            _actDao = actDao;
            _mapper = mapper;
            _context = context;
            _queue = queue;
            _appConfig = config;
        }

        public async Task<List<WarrantyRespon>> GetListWaranty(GlobalFilter request)
        {
            List<WarrantyRespon> warranties = new List<WarrantyRespon>();
            var data = await _actDao.GetAllWarranty(request);
            if(data!= null)
            {
    
                var mappingData = data.Select(x=> new WarrantyRespon
                {
                    WarrantyCode = x.warranty_code,
                    CompanyCode = x.company_code,
                    InvoiceNumber = x.invoice_no,
                    InvoiceDate = x.invoice_date,
                    ArticleUnitCode = x.article_code,
                    ArticleUnitName = x.article_name,
                    SerialNumber = x.serial_no,
                    StartDateWarranty = x.start_date,
                    ExpiredDate = x.end_date,
                    ActivatedBy = x.activate_by,
                    ActivatedOn = x.activate_on,
                    Status = x.active,
                    CreatedBy = x.created_by,
                    CreatedOn = x.created_on,
                    UpdatedBy = x.modified_by,
                    UpdatedOn = x.modified_on
                });

                warranties = mappingData.ToList();
            }

            return warranties;
        }
    
        public async Task<APIResultList<List<WarrantyRespon>>> GetWarrantyList(GlobalFilter request)
        {
            var result = new APIResultList<List<WarrantyRespon>>();
            
            try
            {
                if (request == null)
                    throw new Exception("Request null");

                List<WarrantyRespon> warranties = new List<WarrantyRespon>();
                var newData = await _actDao.GetAllWarranty(request);
                if(newData!= null)
                {
                    var mappingData = newData.Select(x=> new WarrantyRespon
                    {
                        WarrantyCode = x.warranty_code,
                        CompanyCode = x.company_code,
                        InvoiceNumber = x.invoice_no,
                        InvoiceDate = x.invoice_date,
                        ArticleUnitCode = x.article_code,
                        ArticleUnitName = x.article_name,
                        SerialNumber = x.serial_no,
                        StartDateWarranty = x.start_date,
                        ExpiredDate = x.end_date,
                        ActivatedBy = x.activate_by,
                        ActivatedOn = x.activate_on,
                        Status = x.active,
                        CreatedBy = x.created_by,
                        CreatedOn = x.created_on,
                        UpdatedBy = x.modified_by,
                        UpdatedOn = x.modified_on
                    });

                    warranties = mappingData.ToList();
                }
                var totalData = await _actDao.RepoGetTotalAllWarranty(request);

                result.is_ok = true;
                result.message = "Success";
                result.data = warranties;
                result.totalRow = totalData;
            }
            catch (Exception ex)
            {
                result.is_ok = false;
                result.message = "Data Not Found" + ex.Message;
            }

            return result;

        }
        public async Task<APIResultList<bool>> CreateWarranty(CreateActivationWarranty request)
        {
            var result = new APIResultList<bool>();

            try
            {
                if (request == null)
                    throw new Exception("Request null");

                await _actDao.CreateWarranty(request);

                result.is_ok = true;
                result.message = "Success";
                result.data = true;
            }
            catch (Exception)
            {
                result.is_ok = false;
                result.message = "Data failed to submit";
            }

            return result;
        }
        public async Task<APIResultList<WarrantyRespon>> GetWarrantyDetail(string warrantyCode)
        {
            var result = new APIResultList<WarrantyRespon>();

            try
            {
                var entity = await _actDao.GetWarrantyByWrrantyNo(warrantyCode);

                if (entity == null)
                    throw new Exception("Not found");

                var response = new WarrantyRespon
                {
                    WarrantyCode = entity.warranty_no,
                     CompanyCode = entity.company_code,
                    InvoiceNumber = entity.invoice_no,
                    InvoiceDate = entity.invoice_date,
                    ArticleUnitCode = entity.article_code,
                    ArticleUnitName = entity.article_name,
                    SerialNumber = entity.serial_no,
                    StartDateWarranty = entity.start_date,
                    ExpiredDate = entity.end_date,
                    ActivatedBy = entity.activate_by,
                    ActivatedOn = entity.activate_on,
                    Status = entity.active,
                    CreatedBy = entity.created_by,
                    CreatedOn = entity.created_on,
                    UpdatedBy = entity.modified_by,
                    UpdatedOn = entity.modified_on
                };

                result.is_ok = true;
                result.message = "Success";
                result.data = response;
            }
            catch (Exception)
            {
                result.is_ok = false;
                result.message = "Data not found";
            }

            return result;
        }

        public async Task<APIResultList<bool>> DeleteWarrantybyCode(string warrantyCode)
        {
            var result = new APIResultList<bool>();
            try
            {
                if (string.IsNullOrEmpty(warrantyCode))
                    throw new Exception("Request null");

                    var  entity = await _actDao.GetWarrantyByWrrantyNo(warrantyCode);
                    if(!string.IsNullOrEmpty(entity.warranty_code))
                    {
                        await _actDao.DeleteWarrantyByWarrantyCode(warrantyCode);
                        result.is_ok = true;
                        result.message = "Success";
                        result.data = true;
                    }
            }
            catch (Exception ex)
            {
                result.is_ok = false;
                result.message = "Data failed to delete, please contact administrator";
            }
            return result;
        }

        public async Task<APIResultList<bool>> UpdateWarranty(UpdateWarrantyRequest param)
        {
            var result = new APIResultList<bool>();
 
            try
            {
                if (param == null && string.IsNullOrEmpty(param.warranty_code))
                    throw new Exception("Request null");

                    var mapping = HelperObj.convert<UpdateWarrantyRequest, ws_warranty>(param);
                     await _actDao.UpdateWarranty(mapping);
                    result.is_ok = true;
                    result.message = "Success";
                    result.data = true;
                
            }
            catch (Exception ex)
            {
                result.is_ok = false;
                result.message = "Data failed to update, please contact administrator";
            }
            return result;
        }

        public async Task<APIResult> CreateTicketAsync(CreateTiketBase request)
        {
            await using var _conn = await _context.CreateOpenConnectionAsync();
            await using var trans = await _conn.BeginTransactionAsync();

            try
            {
                string ticketNoUpdate = await ticketNumbering();
                request.ticket_header.ticket_no = ticketNoUpdate;
                await _actDao.CreateTicketService(request.ticket_header);

                //unit detail
                foreach(var unit in request.ticket_unit ?? [])
                {
                    unit.ticket_no = ticketNoUpdate;
                    await _actDao.CreateTicketUnit(unit);

                     // 👉 enqueue AI classification
                            if (!string.IsNullOrEmpty(unit.complaint_text))
                            {
                                await _queue.EnqueueAsync(new AIJob
                                {
                                    WarrantyNo = unit.warranty_no,
                                    UnitId = unit.unit_line_no,
                                    ComplaintText = unit.complaint_text
                                });
                            }
                }

                //sparepart detail
                foreach(var sparepart in request.ticket_sparepart ?? [])
                {
                    sparepart.ticket_no = ticketNoUpdate;
                    await _actDao.CreateTicketSparepart(sparepart);
                }
                
                //commit trans
                await trans.CommitAsync();
                return new APIResult{ is_ok = true, message = "Success"};
            }
            catch (Exception ex)
            {
                //rollback trans
                await trans.RollbackAsync();
                return new APIResult
                {
                    is_ok = false,
                    message = "Failed to submit data : " + ex.Message
                };
            }
        }

        private async Task<string> ticketNumbering ()
        {
            string number = string.Empty;

            var lastCount = await _actDao.GetLastTicketNumber();
            var sequence = lastCount +1;
            var today = DateTime.UtcNow;
            number = $"TKT-{today:yyyy-MM-dd}-{sequence:D5}";

            return number;
        }

        public async Task<APIResultList<List<TicketDetailRespon>>> GetTicketDetail(string ticket_no)
        {
            var result = new APIResultList<List<TicketDetailRespon>>();
            if (string.IsNullOrWhiteSpace(ticket_no))
            {
                result.is_ok = false;
                result.message = "Ticket number is required";
                return result;
            }

            try
            {

                    var header = await _actDao.GetTicketHeaderByTicketNo(ticket_no);
                    if (header == null)
                    {
                        result.is_ok = false;
                        result.message = "Ticket number not found";
                        return result;
                    }

                    var unit = await _actDao.GetAllTicketUnit(ticket_no);
                    var sparepart = await _actDao.GetAllTicketSparepart(ticket_no);
                    
                    var endpointCustomer = $"{_appConfig.CustomerService_urlAPI}{AppConstant.CUSTOMER_GET_DETAIL}?id={header.customer_id}";
                    var endpointEmployee = $"{_appConfig.EmployeeService_urlAPI}{AppConstant.EMPLOYEE_GET_DETAIL}?nip={header.assign_to}";
                    //get customer from WS_CRM_CUSTOMER_SERVICE with localhost path
                    var customers = await _actDao.GetCustomerById(endpointCustomer);
                    //get employee from WS_CRM_CEmployee with localhost path
                    var employees = await _actDao.GetEmployeeByNIP(endpointEmployee);

                    /* IF IN THE FUTURE WILL CONCERN TO ROBUST TIMING , MUST CHANGE SEQUENCIAL TO WHENALL METHODE
                    but await in every task need to deleted , add this code :(reminder for seeing microsoft documentation before implement)
                    await Task.WhenAll(unit, sparepart, customers, employees);
                    */

                    //mapping unit using LINQ
                     var unitList = unit.Select(u => new CreateTicketUnit
                     {
                        active = u.active,
                        product_name = u.product_name,
                        sku_code = u.sku_code,
                        qty = u.qty,
                        unit_line_no = u.unit_line_no,
                        created_by = u.created_by,
                        created_on = u.created_on
                     }).ToList();
                    
                     var spList = sparepart.Select(sp=> new CreateTicketSparepart
                     {
                            sparepart_code = sp.sparepart_code,
                            sparepart_name = sp.sparepart_name,
                            unit_line_no = sp.unit_line_no,
                            uom = sp.uom,
                            qty = sp.qty,
                            product_name = sp.product_name,
                            created_by = sp.created_by,
                            created_on = sp.created_on
                     }).ToList();

                    
                    var detail = new TicketDetailRespon
                    {
                        ticket_no = header.ticket_no,
                        assign_to = header.assign_to,
                        assign_name = employees?.data?.name,
                        customer_id = header.customer_id,
                        payment_method = header.payment_method,
                        status = header.status,
                        service_center = header.service_center,
                        ticket_unit = unitList,
                        ticket_sparepart = spList
                    };

                    result.data = new List<TicketDetailRespon> { detail };
                    result.is_ok = true;
                    result.message = "Success";
            }
            catch (Exception ex)
            {
                result.is_ok = false;
                result.message = "Data failed to submit, please contact administrator";
            }
            return result;
        }
    
        public async Task<APIResultList<List<ws_ticket>>> GetTicketList(GlobalFilter filter)
        {
             if (filter == null)
                    throw new Exception("Request null");
            var result = new APIResultList<List<ws_ticket>>();
            try
            {
                var data = await _actDao.GetAllTicketHeader(filter);
                var totalData = await _actDao.RepoGetTotalAllTicket(filter);
                result.is_ok = true;
                result.message = "Success";
                result.data = data.ToList();
                result.totalRow = totalData;
            }
            catch (Exception ex)
            {
                result.is_ok = false;
                result.message = "Data Not Found" + ex.Message;
            }

            return result;

        }

        public async Task<APIResultList<ws_ticket>> UpdateStatusTicket(UpdateTicketStatusRequest data)
        {
            var result = new APIResultList<ws_ticket>();
            try
            {
                if(string.IsNullOrWhiteSpace(data.ticket_no))
                {
                    result.is_ok = false;
                    result.message = "Ticket number is required";
                    return result;
                }
                
                    var ticket = HelperObj.convert<UpdateTicketStatusRequest, ws_ticket>(data);
                    ticket.modified_by = "sys";
                    ticket.modified_on = DateTime.UtcNow;
                    await _actDao.UpdateTicketStatus(ticket);
                    result.is_ok = true;
                    result.message = "Success";
            }
            catch (Exception ex)
            {
                result.is_ok = false;
                result.message = "Data failed to update, please contact administrator";
            }
            return result;
        }

        public async Task<APIResultList<ws_ticket>> DeleteTicketHeader(string ticket_no)
        {
            var result = new APIResultList<ws_ticket>();
            try
            {
                if(string.IsNullOrWhiteSpace(ticket_no))
                {
                    result.is_ok = false;
                    result.message = "Ticket number is required";
                    return result;
                }

                var ticketHeader = await _actDao.GetTicketHeaderByTicketNo(ticket_no);
                if(ticketHeader == null)
                {
                    result.is_ok = false;
                    result.message = "Ticket number is required";
                    return result;
                }

                await _actDao.NonActiveTicketHeader(ticket_no);
                result.is_ok = true;
                result.message = "Success";

            }
            catch (Exception ex)
            {
                result.is_ok = false;
                result.message = "Data failed to delete, please contact administrator";
            }
            return result;
        }
    }
}