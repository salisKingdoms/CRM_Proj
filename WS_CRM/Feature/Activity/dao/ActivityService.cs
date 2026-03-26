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

namespace WS_CRM.Feature.Activity.dao {
    
    public class ActivityService
    {
         IActivityRepo _actDao;
         private readonly IMapper _mapper;
        
        public ActivityService(IActivityRepo actDao,IMapper mapper)
        {
            _actDao = actDao;
            _mapper = mapper;
            
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

        /*public async Task<APIResult> CreateTicketAsync(CreateTiketBase request)
        {
            using var connection = _db.CreateConnection();
            await connection.OpenAsync();

            using var transaction = connection.BeginTransaction();

            try
            {
                string ticketNo = await GenerateTicketNoAsync(connection, transaction);

                request.ticket_header.ticket_no = ticketNo;

                await _actDao.CreateTicketService(
                        request.ticket_header,
                        connection,
                        transaction);

                if (request.ticket_unit != null)
                {
                    foreach (var unit in request.ticket_unit)
                    {
                        unit.ticket_no = ticketNo;

                        await _actDao.CreateTicketUnit(
                                unit,
                                connection,
                                transaction);
                    }
                }

                if (request.ticket_sparepart != null)
                {
                    foreach (var sparepart in request.ticket_sparepart)
                    {
                        sparepart.ticket_no = ticketNo;

                        await _actDao.CreateTicketSparepart(
                                sparepart,
                                connection,
                                transaction);
                    }
                }

                await transaction.CommitAsync();

                return new APIResult
                {
                    is_ok = true,
                    message = "Success"
                };
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();

                return new APIResult
                {
                    is_ok = false,
                    message = "Failed to submit data"
                };
            }
        }*/
    
    }
}