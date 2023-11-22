using Microsoft.AspNetCore.Mvc;
using WS_CRM.Feature.Activity.dao;
using WS_CRM.Config;
using WS_CRM.Feature.Activity.dto;
using WS_CRM.Feature.Activity.Model;
using WS_CRM.Helper;
using WS_CRM.Feature.Activity.Model;
using WS_CRM.Feature.Customer.Model;

namespace WS_CRM.Feature.Activity
{
    [ApiController]
    [Route("[controller]")]
    public class ActivityController : Controller
    {
        IActivityDao _actDao;
        public ActivityController(IActivityDao actDao)
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
                    await _actDao.CreateActivity(request);
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
                var totalData = await _actDao.GetTotalAllWarranty();
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
    }
}
