using Microsoft.AspNetCore.Mvc;
using WS_CRM_Employee.Feature.Employee.Dao;
using WS_CRM_Employee.Config;
using WS_CRM_Employee.Feature.Employee.Dto;
using WS_CRM_Employee.Feature.Employee.Model;
using WS_CRM_Employee.Helper;
using System.Text;

namespace WS_CRM_Employee.Feature.Employee
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeeController : Controller
    {
        IEmployeeRepo _employeRepo;

        public EmployeeController(IEmployeeRepo employeRepo)
        {
            _employeRepo = employeRepo;
        }

        [HttpPost]
        [Route("CreateEmployee")]
        public async Task<IActionResult> CreateEmployee(CreateEmployeeRequest request)
        {
            var result = new APIResultList<List<ws_employee>>();
            try
            {
                string user = "person";
                if (request != null)
                {
                    var data = HelperObj.convert<CreateEmployeeRequest, ws_employee>(request);
                    data.created_by = user;
                    data.active = true;
                    await _employeRepo.CreateEmployee(data);
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
        [Route("GetEmployeeList")]
        public async Task<IActionResult> GetEmployeeList()
        {
            var result = new APIResultList<List<ws_employee>>();
            try
            {
                var data = await _employeRepo.GetAllEmployee("", "");
                var totalData = await _employeRepo.RepoGetTotalAllEmployee("", "");
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
        [Route("GetDetailEmployeebyId")]
        public async Task<IActionResult> GetDetailEmployeebyNIP(string nip)
        {
            var result = new APIResultList<ws_employee>();
            try
            {
                if (!string.IsNullOrEmpty(nip))
                {
                    var data = await _employeRepo.GetEmployeeByNIP(nip);
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
        [Route("DeleteEmployeebyNIP")]
        public async Task<IActionResult> DeleteEmployeebyNIP(string nip)
        {
            var result = new APIResultList<ws_employee>();
            try
            {
                if (!string.IsNullOrEmpty(nip))
                {
                    await _employeRepo.DeleteEmployee(nip);
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
        [Route("UpdateEmployee")]
        public async Task<IActionResult> UpdateEmployee(UpdateEmployeeRequest request)
        {
            var result = new APIResultList<List<ws_employee>>();
            try
            {
                string user = "person";
                if (request != null)
                {
                    var data = HelperObj.convert<UpdateEmployeeRequest, ws_employee>(request);
                    data.modified_by = user;
                    data.modified_on = DateTime.UtcNow;
                    await _employeRepo.UpdateEmployee(data);
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
