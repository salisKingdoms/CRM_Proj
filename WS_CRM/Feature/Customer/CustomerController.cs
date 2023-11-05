using Microsoft.AspNetCore.Mvc;
using WS_CRM.Feature.Customer.dao;
using WS_CRM.Config;
using WS_CRM.Feature.Customer.dto;
using WS_CRM.Feature.Customer.Model;

namespace WS_CRM.Feature.Customer
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : Controller
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}
        //ivate IUserService _userService;
        ICustomerDao _customerDao;
        public CustomerController(ICustomerDao custDao)
        {
            _customerDao = custDao;
        }
        [HttpGet]
        [Route("GetCustomerList")]
        //roducesResponseType(typeof(apir
        public async Task<IActionResult> GetCustomersList()
        {
            var result = new APIResultList<List<Customers>>();
            try
            {
                var data = await _customerDao.GetAll();
                result.is_ok=true;
                result.message = "Success";
                result.data = data.ToList();
            }
            catch (Exception ex)
            {

            }
            
            return Ok(result);

        }
    }
}
