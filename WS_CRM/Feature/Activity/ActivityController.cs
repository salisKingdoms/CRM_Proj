using Microsoft.AspNetCore.Mvc;
using WS_CRM.Feature.Activity.dao;
using WS_CRM.Config;
using WS_CRM.Feature.Activity.dto;
using WS_CRM.Feature.Activity.Model;
using WS_CRM.Helper;
using WS_CRM.Feature.Activity.Model;

namespace WS_CRM.Feature.Activity
{
    [ApiController]
    [Route("[controller]")]
    public class ActivityController : Controller
    {
        IActivityRepo _actDao;
        public ActivityController(IActivityRepo actDao)
        {
            _actDao = actDao;
        }

        [HttpPost]
        [Route("CreateWarranty")]
        public async Task<IActionResult> CreateWarranty(CreateActivationWarranty request)
        {
            var result = new APIResultList<List<ws_warranty>>();
            try
            {
                if (request != null)
                {
                    await _actDao.CreateWarranty(request);
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
                    request.ticket_header.ticket_no = ticketNo;
                    await _actDao.CreateTicketService(request.ticket_header);

                    if(request.ticket_unit != null)
                    {
                        foreach (var unit in request.ticket_unit)
                        {
                            unit.ticket_no = ticketNo;
                            await _actDao.CreateTicketUnit(unit);
                        }
                    }

                    if (request.ticket_sparepart != null)
                    {
                        foreach (var sparepart in request.ticket_sparepart)
                        {
                            sparepart.ticket_no = ticketNo;
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

        [HttpPost]
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
                    //get customer
                    //get employee for assign to

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

    }
}
