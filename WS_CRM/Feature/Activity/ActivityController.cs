using Microsoft.AspNetCore.Mvc;
using WS_CRM.Feature.Activity.dao;
using WS_CRM.Config;
using WS_CRM.Feature.Activity.dto;
using WS_CRM.Feature.Activity.Model;
using WS_CRM.Helper;
using Newtonsoft.Json;
using Microsoft.VisualBasic;
using System.Linq;
using System;
using System.Collections.Generic;
using System.IO;

namespace WS_CRM.Feature.Activity
{
    [ApiController]
    [Route("[controller]")]
    public class ActivityController : Controller
    {
        IActivityRepo _actDao;
        protected readonly IConfiguration _config;
        private readonly ILogger<ActivityController> _logger;
        public ActivityController(ILogger<ActivityController> logger,IActivityRepo actDao, IConfiguration config)
        {
            _logger = logger;
            _actDao = actDao;
            _config = config;
        }

        [HttpPost]
        [Route("CreateWarranty")]
        public async Task<IActionResult> CreateWarranty(CreateActivationWarranty request)
        {
            var result = new APIResultList<List<ws_warranty>>();
            var bodyJson = JsonConvert.SerializeObject(request);
            _logger.LogInformation(HelperLog.GetRequestLog("CreateWarranty", bodyJson));
            try
            {
                if (request != null)
                {
                    await _actDao.CreateWarranty(request);
                    result.is_ok = true;
                    result.message = "Success";
                    _logger.LogInformation(HelperLog.GetResponseSuccessLog("CreateWarranty", JsonConvert.SerializeObject(result)));
                }
            }
            catch (Exception ex)
            {
                result.is_ok = false;
                result.message = "Data failed to submit, please contact administrator";
                _logger.LogInformation(HelperLog.GetResponseErrorLog("CreateWarranty", JsonConvert.SerializeObject(result)));
            }
            return Ok(result);
        }

        [HttpGet]
        [Route("GetWarrantyList")]
        public async Task<IActionResult> GetWarrantyList()
        {
            var result = new APIResultList<List<ws_warranty>>();
            
            try
            {
                var data = await _actDao.GetAllWarranty();
                var totalData = await _actDao.RepoGetTotalAllWarranty();
                result.is_ok = true;
                result.message = "Success";
                result.data = data.ToList();
                result.totalRow = totalData;
            }
            catch (Exception ex)
            {

            }

            return Ok(result);

        }

        [HttpGet]
        [Route("GetDetailWarrantybyId")]
        public async Task<IActionResult> GetDetailWarrantybyId(long id)
        {
            var result = new APIResultList<ws_warranty>();
            try
            {
                if (id > 0)
                {
                    var data = await _actDao.GetWarrantyById(id);
                    result.data = data;
                    result.is_ok = true;
                    result.message = "Success";
                }
            }
            catch (Exception ex)
            {
                result.is_ok = false;
                result.message = "Data not found, please contact administrator";
            }
            return Ok(result);
        }

        [HttpDelete]
        [Route("DeleteWarrantybyId")]
        public async Task<IActionResult> DeleteWarrantybyId(long id)
        {
            var result = new APIResultList<ws_warranty>();
            try
            {
                if (id > 0)
                {
                    await _actDao.DeleteWarrantyById(id);
                    result.is_ok = true;
                    result.message = "Success";
                }
            }
            catch (Exception ex)
            {
                result.is_ok = false;
                result.message = "Data failed to delete, please contact administrator";
            }
            return Ok(result);
        }

        [HttpPost]
        [Route("UpdateWarranty")]
        public async Task<IActionResult> UpdateWarranty(UpdateWarrantyRequest data)
        {
            var result = new APIResultList<ws_warranty>();
            try
            {
                if (data != null && data.id > 0)
                {
                    var prod = HelperObj.convert<UpdateWarrantyRequest, ws_warranty>(data);
                    await _actDao.UpdateWarranty(prod);
                    result.is_ok = true;
                    result.message = "Success";
                }
            }
            catch (Exception ex)
            {
                result.is_ok = false;
                result.message = "Data failed to update, please contact administrator";
            }
            return Ok(result);
        }

        [HttpPost]
        [Route("Ticket/CreateTicket")]
        public async Task<IActionResult> CreateTicket(CreateTiketBase request)
        {
            var result = new APIResultList<List<ws_ticket>>();
            try
            {
                if (request != null)
                {
                    string ticketNo = "T0001";//must make logic to generate number automaticly
                                              //equest.ticket_header.ticket_no = ticketNo;
                                              //var header = await _actDao.GetTicketHeaderByTicketNo(ticketNo);
                                              //if (!string.IsNullOrEmpty(header.ticket_no))
                                              //    await _actDao.UpdateTicketHeader(request.ticket_header);
                    await _actDao.CreateTicketService(request.ticket_header);


                    if (request.ticket_unit != null)
                    {
                        var unitOld = await _actDao.GetAllTicketUnit(request.ticket_header.ticket_no);
                        foreach (var unit in request.ticket_unit)
                        {

                            unit.ticket_no = ticketNo;
                            //var unReq = HelperObj.convert<ws_ticket_unit, CreateTicketUnit>(unit);
                            //var unitSame = unitOld.Where(x => x.ticket_no == unit.ticket_no).FirstOrDefault();
                            //if (unitSame != null)
                            //    await _actDao.UpdateTicketUnit(unReq);
                            await _actDao.CreateTicketUnit(unit);
                        }
                    }

                    if (request.ticket_sparepart != null)
                    {
                        var spOld = await _actDao.GetAllTicketSparepart(request.ticket_header.ticket_no);
                        foreach (var sparepart in request.ticket_sparepart)
                        {
                            sparepart.ticket_no = ticketNo;
                            //ar spReq = HelperObj.convert<ws_ticket_sparepart, CreateTicketSparepart>(sparepart);
                            //r spSame = spOld.Where(x => x.ticket_no == sparepart.ticket_no).FirstOrDefault();
                            //if (spOld != null)
                            //    await _actDao.UpdateTicketSparepart(spReq);
                            await _actDao.CreateTicketSparepart(sparepart);
                        }
                    }

                    result.is_ok = true;
                    result.message = "Success";
                }
            }
            catch (Exception ex)
            {
                result.is_ok = false;
                result.message = "Data failed to submit, please contact administrator";
            }
            return Ok(result);
        }

        [HttpGet]
        [Route("Ticket/GetTicketDetail")]
        public async Task<IActionResult> GetTicketDetail(string ticket_no)
        {
            var result = new APIResultList<List<TicketDetailRespon>>();
            try
            {
                if (!string.IsNullOrEmpty(ticket_no))
                {
                    var header = await _actDao.GetTicketHeaderByTicketNo(ticket_no);
                    var unit = await _actDao.GetAllTicketUnit(ticket_no);
                    var sparepart = await _actDao.GetAllTicketSparepart(ticket_no);
                    //var endpointCustomer = "https://localhost:44314/Customer/" + AppConstant.CUSTOMER_GET_DETAIL + "?id=" + header.customer_id;
                    // var customers = await _actDao.GetCustomerById(endpointCustomer);
                    //get customer
                    //get employee for assign to
                    List<CreateTicketUnit> unitList = new List<CreateTicketUnit>();
                    foreach (var item_u in unit)
                    {
                        unitList.Add(new CreateTicketUnit
                        {
                            active = item_u.active,
                            product_name = item_u.product_name,
                            sku_code = item_u.sku_code,
                            qty = item_u.qty,
                            unit_line_no = item_u.unit_line_no,
                            created_by = item_u.created_by,
                            created_on = item_u.created_on
                        });

                    }

                    List<CreateTicketSparepart> spList = new List<CreateTicketSparepart>();
                    foreach (var sp in sparepart)
                    {
                        spList.Add(new CreateTicketSparepart
                        {
                            //active = sp.active,
                            sparepart_code = sp.sparepart_code,
                            sparepart_name = sp.sparepart_name,
                            unit_line_no = sp.unit_line_no,
                            uom = sp.uom,
                            qty = sp.qty,
                            product_name = sp.product_name,
                            created_by = sp.created_by,
                            created_on = sp.created_on
                        });
                    }
                    List<TicketDetailRespon> datas = new List<TicketDetailRespon>();
                    TicketDetailRespon detail = new TicketDetailRespon();

                    //ticket_sparepart = convertSparepart,
                    // ticket_unit = convertUnit,
                    detail.ticket_no = header.ticket_no;
                    detail.assign_to = header.assign_to;
                    detail.assign_name = "sistem";//nanti akan d ambil dari service employee jika sudah publish
                    detail.customer_id = header.customer_id;
                    detail.payment_method = header.payment_method;
                    detail.status = header.status;
                    detail.service_center = header.service_center;
                    detail.ticket_sparepart = spList;
                    detail.ticket_unit = unitList;
                    datas.Add(detail);



                    result.data = datas;
                    result.is_ok = true;
                    result.message = "Success";
                }
            }
            catch (Exception ex)
            {
                result.is_ok = false;
                result.message = "Data failed to submit, please contact administrator";
            }
            return Ok(result);
        }

        [HttpPost]
        [Route("GetTicketList")]
        public async Task<IActionResult> GetTicketList(GlobalFilter filter)
        {
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

            }

            return Ok(result);

        }

        [HttpPost]
        [Route("UpdateStatusTicket")]
        public async Task<IActionResult> UpdateStatusTicket(UpdateTicketStatusRequest data)
        {
            var result = new APIResultList<ws_ticket>();
            try
            {
                if (!string.IsNullOrEmpty(data.ticket_no))
                {
                    var ticket = HelperObj.convert<UpdateTicketStatusRequest, ws_ticket>(data);
                    ticket.modified_by = "sys";
                    ticket.modified_on = DateTime.UtcNow;
                    await _actDao.UpdateTicketStatus(ticket);
                    result.is_ok = true;
                    result.message = "Success";
                }
            }
            catch (Exception ex)
            {
                result.is_ok = false;
                result.message = "Data failed to update, please contact administrator";
            }
            return Ok(result);
        }

        [HttpDelete]
        [Route("DeleteTicketHeader")]
        public async Task<IActionResult> DeleteTicketHeader(string ticket_no)
        {
            var result = new APIResultList<ws_ticket>();
            try
            {
                await _actDao.NonActiveTicketHeader(ticket_no);
                result.is_ok = true;
                result.message = "Success";

            }
            catch (Exception ex)
            {
                result.is_ok = false;
                result.message = "Data failed to delete, please contact administrator";
            }
            return Ok(result);
        }
    }
}
