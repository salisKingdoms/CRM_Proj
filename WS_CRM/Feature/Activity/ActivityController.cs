using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WS_CRM.BackgroundJob;
using WS_CRM.Config;
using WS_CRM.Feature.Activity.dao;
using WS_CRM.Feature.Activity.dto;
using WS_CRM.Feature.Activity.Model;
using WS_CRM.Helper;

namespace WS_CRM.Feature.Activity
{
    [ApiController]
    [Route("[controller]")]
    public class ActivityController : Controller
    {
        IActivityRepo _actDao;
        protected readonly IConfiguration _config;
        private readonly ILogger<ActivityController> _logger;
        private readonly IJwtFunction _jwtFunction;
        private readonly IBackgroundTaskQueue _queue;
        private readonly GroqAIService _groqAiService;
        private readonly ActivityService _activityService;
        public ActivityController(ILogger<ActivityController> logger,IActivityRepo actDao, IConfiguration config, IJwtFunction jwtFunction, IBackgroundTaskQueue AIqueue, ActivityService actService)
        {
            _logger = logger;
            _actDao = actDao;
            _config = config;
            _jwtFunction = jwtFunction;
            _queue = AIqueue;
            _activityService = actService;
        }

        [HttpPost]
        [Route("CreateWarranty")]
        public async Task<IActionResult> CreateWarranty(CreateActivationWarranty request)
        {
            var tokenVerification = _jwtFunction.TokenVerification(Request);
            if (!tokenVerification.is_ok) return Unauthorized(tokenVerification);

            var bodyJson = JsonConvert.SerializeObject(request);
            _logger.LogInformation(HelperLog.GetRequestLog("CreateWarranty", bodyJson));

            var results = await _activityService.CreateWarranty(request);
            if (results.is_ok)
            {
                _logger.LogInformation(HelperLog.GetResponseSuccessLog("CreateWarranty",JsonConvert.SerializeObject(results)));
            }
            else
            {
                _logger.LogWarning(HelperLog.GetResponseErrorLog("CreateWarranty",JsonConvert.SerializeObject(results)));
            }

            return Ok(results);
        }

        [HttpGet]
        [Route("GetWarrantyList")]
        [SwaggerOrderBy(typeof(ws_warranty))]
        public async Task<IActionResult> GetWarrantyList([FromQuery] GlobalFilter request)
        {
            var tokenVerification = _jwtFunction.TokenVerification(Request);
            if (!tokenVerification.is_ok) return Unauthorized(tokenVerification);

            var bodyJson = JsonConvert.SerializeObject(request);
            _logger.LogInformation(HelperLog.GetRequestLog("GetWarrantyList", bodyJson));

            var result = await _activityService.GetWarrantyList(request);
            if (result.is_ok)
            {
                _logger.LogInformation(HelperLog.GetResponseSuccessLog("GetWarrantyList",JsonConvert.SerializeObject(result)));
            }
            else
            {
                _logger.LogWarning(HelperLog.GetResponseErrorLog("GetWarrantyList",JsonConvert.SerializeObject(result)));
            }
            /*  move to service // try
            // {
            //     var newData = await _activityService.GetListWaranty(request);
            //     var totalData = await _actDao.RepoGetTotalAllWarranty(request);
            //     result.is_ok = true;
            //     result.message = "Success";
            //     result.data = newData.ToList();
            //     result.totalRow = totalData;
            // }
            // catch (Exception ex)
            // {
            //     result.is_ok = false;
            //     result.message = "Data Not Found";
            // }*/

            return Ok(result);

        }

        [HttpGet]
        [Route("GetDetailWarrantyByCode")]
        public async Task<IActionResult> GetDetailWarrantyByCode(string warrantyCode)
        {
            var tokenVerification = _jwtFunction.TokenVerification(Request);
            if (!tokenVerification.is_ok) return Unauthorized(tokenVerification);

            var bodyJson = JsonConvert.SerializeObject(warrantyCode);
            _logger.LogInformation(HelperLog.GetRequestLog("GetDetailWarrantyByCode", bodyJson));

            var result = await _activityService.GetWarrantyDetail(warrantyCode);
            if (result.is_ok)
            {
                _logger.LogInformation(HelperLog.GetResponseSuccessLog("GetDetailWarrantyByCode",JsonConvert.SerializeObject(result)));
            }
            else
            {
                _logger.LogWarning(HelperLog.GetResponseErrorLog("GetDetailWarrantyByCode",JsonConvert.SerializeObject(result)));
            }

            return Ok(result);
        }

        [HttpDelete]
        [Route("DeleteWarrantybyWarrantyCode")]
        public async Task<IActionResult> DeleteWarrantybyWarrantyCode(string warrantyCode)
        {
            var tokenVerification = _jwtFunction.TokenVerification(Request);
            if (!tokenVerification.is_ok) return Unauthorized(tokenVerification);

            var bodyJson = JsonConvert.SerializeObject(warrantyCode);
            _logger.LogInformation(HelperLog.GetRequestLog("DeleteWarranty", bodyJson));

            var result = await _activityService.DeleteWarrantybyCode(warrantyCode);
            if (result.is_ok)
            {
                _logger.LogInformation(HelperLog.GetResponseSuccessLog("DeleteWarranty",JsonConvert.SerializeObject(result)));
            }
            else
            {
                _logger.LogWarning(HelperLog.GetResponseErrorLog("DeleteWarranty",JsonConvert.SerializeObject(result)));
            }

            return Ok(result);
        }

        [HttpPost]
        [Route("UpdateWarranty")]
        public async Task<IActionResult> UpdateWarranty(UpdateWarrantyRequest data)
        {
            var tokenVerification = _jwtFunction.TokenVerification(Request);
            if (!tokenVerification.is_ok) return Unauthorized(tokenVerification);

            var bodyJson = JsonConvert.SerializeObject(data);
            _logger.LogInformation(HelperLog.GetRequestLog("UpdateWarranty", bodyJson));

            var result = await _activityService.UpdateWarranty(data);
            if (result.is_ok)
            {
                _logger.LogInformation(HelperLog.GetResponseSuccessLog("UpdateWarranty",JsonConvert.SerializeObject(result)));
            }
            else
            {
                _logger.LogWarning(HelperLog.GetResponseErrorLog("UpdateWarranty",JsonConvert.SerializeObject(result)));
            }
           
            return Ok(result);
        }

        [HttpPost]
        [Route("Ticket/CreateTicket")]
        public async Task<IActionResult> CreateTicket(CreateTiketBase request)
        {
            var tokenVerification = _jwtFunction.TokenVerification(Request);
            if (!tokenVerification.is_ok) return Unauthorized(tokenVerification);

             var bodyJson = JsonConvert.SerializeObject(request);
            _logger.LogInformation(HelperLog.GetRequestLog("Ticket/CreateTicket", bodyJson));

            var result = await _activityService.CreateTicketAsync(request);
            if (result.is_ok)
            {
                _logger.LogInformation(HelperLog.GetResponseSuccessLog("Ticket/CreateTicket",JsonConvert.SerializeObject(result)));
            }
            else
            {
                _logger.LogWarning(HelperLog.GetResponseErrorLog("Ticket/CreateTicket",JsonConvert.SerializeObject(result)));
            }
           
            return Ok(result);
        }

        [HttpGet]
        [Route("Ticket/GetTicketDetail")]
        public async Task<IActionResult> GetTicketDetail(string ticket_no)
        {
            var tokenVerification = _jwtFunction.TokenVerification(Request);
            if (!tokenVerification.is_ok) return Unauthorized(tokenVerification);

            var bodyJson = JsonConvert.SerializeObject(ticket_no);
            _logger.LogInformation(HelperLog.GetRequestLog("Ticket/GetTicketDetail", bodyJson));

            var result = await _activityService.GetTicketDetail(ticket_no);
            if (result.is_ok)
            {
                _logger.LogInformation(HelperLog.GetResponseSuccessLog("Ticket/GetTicketDetail",JsonConvert.SerializeObject(result)));
            }
            else
            {
                _logger.LogWarning(HelperLog.GetResponseErrorLog("Ticket/GetTicketDetail",JsonConvert.SerializeObject(result)));
            }

            return Ok(result);
        }

        [HttpPost]
        [Route("GetTicketList")]
        public async Task<IActionResult> GetTicketList(GlobalFilter filter)
        {
            var tokenVerification = _jwtFunction.TokenVerification(Request);
            if (!tokenVerification.is_ok) return Unauthorized(tokenVerification);

            var bodyJson = JsonConvert.SerializeObject(filter);
            _logger.LogInformation(HelperLog.GetRequestLog("GetTicketList", bodyJson));
            
            var result = await _activityService.GetTicketList(filter);
            if (result.is_ok)
            {
                _logger.LogInformation(HelperLog.GetResponseSuccessLog("GetTicketList",JsonConvert.SerializeObject(result)));
            }
            else
            {
                _logger.LogWarning(HelperLog.GetResponseErrorLog("GetTicketList",JsonConvert.SerializeObject(result)));
            }

            return Ok(result);

        }

        [HttpPost]
        [Route("UpdateStatusTicket")]
        public async Task<IActionResult> UpdateStatusTicket(UpdateTicketStatusRequest data)
        {
            var tokenVerification = _jwtFunction.TokenVerification(Request);
            if (!tokenVerification.is_ok) return Unauthorized(tokenVerification);

            var bodyJson = JsonConvert.SerializeObject(data);
            _logger.LogInformation(HelperLog.GetRequestLog("UpdateStatusTicket", bodyJson));

            var result = await _activityService.UpdateStatusTicket(data);
            if (result.is_ok)
            {
                _logger.LogInformation(HelperLog.GetResponseSuccessLog("UpdateStatusTicket",JsonConvert.SerializeObject(result)));
            }
            else
            {
                _logger.LogWarning(HelperLog.GetResponseErrorLog("UpdateStatusTicket",JsonConvert.SerializeObject(result)));
            }

            return Ok(result);
        }

        [HttpDelete]
        [Route("DeleteTicketHeader")]
        public async Task<IActionResult> DeleteTicketHeader(string ticket_no)
        {
            var tokenVerification = _jwtFunction.TokenVerification(Request);
            if (!tokenVerification.is_ok) return Unauthorized(tokenVerification);

            var bodyJson = JsonConvert.SerializeObject(ticket_no);
            _logger.LogInformation(HelperLog.GetRequestLog("DeleteTicketHeader", bodyJson));
            var result = await _activityService.DeleteTicketHeader(ticket_no);
            if (result.is_ok)
            {
                _logger.LogInformation(HelperLog.GetResponseSuccessLog("DeleteTicketHeader",JsonConvert.SerializeObject(result)));
            }
            else
            {
                _logger.LogWarning(HelperLog.GetResponseErrorLog("DeleteTicketHeader",JsonConvert.SerializeObject(result)));
            }
            
            return Ok(result);
        }
    }
}
