using Microsoft.AspNetCore.Mvc;
using WS_CRM.Feature.Customer.dao;
using WS_CRM.Config;
using WS_CRM.Feature.Customer.dto;
using WS_CRM.Feature.Customer.Model;
using WS_CRM.Helper;

namespace WS_CRM.Feature.Customer
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : Controller
    {
        ICustomerDao _customerDao;
        public CustomerController(ICustomerDao custDao)
        {
            _customerDao = custDao;
        }

        [HttpGet]
        [Route("GetCustomerList")]
     // [ProducesResponseType(typeof(APIResult<List<Customer>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCustomersList()
        {
            var result = new APIResultList<List<Customers>>();
            try
            {
                var data = await _customerDao.GetAll();
                var totalData = await _customerDao.GetTotalAllCustomer();
                result.is_ok=true;
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
        [Route("CreateCustomer")]
        public async Task<IActionResult> CreateCustomer (CreateCustomerRequest request)
        {
            var result = new APIResultList<List<Customers>>();
            try
            {
                if(request != null)
                {
                    await _customerDao.CreateCustomer(request);
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
        [Route("GetDetailCustomerbyId")]
        public async Task<IActionResult> GetDetailCustomerbyId(long id)
        {
            var result = new APIResultList<Customers>();
            try
            {
                if (id > 0)
                {
                    var data = await _customerDao.GetCustomerById(id);
                    result.data = data;
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

        //public async Task DeleteCustomerById(long id)
        [HttpDelete]
        [Route("DeleteCustomerbyId")]
        public async Task<IActionResult> DeleteCustomerbyId(long id)
        {
            var result = new APIResultList<Customers>();
            try
            {
                if (id > 0)
                {
                    await _customerDao.DeleteCustomerById(id);
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
        [Route("UpdateCustomer")]
        public async Task<IActionResult> UpdateCustomer(UpdateCustomerRequest data)
        {
            var result = new APIResultList<Customers>();
            try
            {
                if (data != null && data.id > 0)
                {
                    var cust = HelperObj.convert<UpdateCustomerRequest,Customers>(data);
                    //cust.modified_on = data.modified_on;
                    await _customerDao.UpdateCustomer(cust);
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
        [Route("CreateMember")]
        public async Task<IActionResult> CreateMember(CreateMembersRequest request)
        {
            var result = new APIResultList<List<Members>>();
            try
            {
                if (request != null)
                {
                    var cust_id = await _customerDao.CreateMember(request);
                    if(cust_id > 0)
                    {
                        int id_cust = Convert.ToInt32(cust_id);
                        var UpCustomerMember =  _customerDao.UpdateMemberCustomer(id_cust);
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

        [HttpDelete]
        [Route("DeleteMember")]
        public async Task<IActionResult> DeleteMember(long cust_id)
        {
            var result = new APIResultList<List<Members>>();
            try
            {
                if (cust_id > 0)
                {
                    int customerId = Convert.ToInt32(cust_id);
                    var del_member= _customerDao.DeleteMemberCustomer(customerId);
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
