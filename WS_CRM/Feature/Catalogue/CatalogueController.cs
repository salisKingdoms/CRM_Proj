using Microsoft.AspNetCore.Mvc;
using WS_CRM.Feature.Catalogue.dao;
using WS_CRM.Feature.Catalogue.dto;
using WS_CRM.Feature.Catalogue.Model;
using WS_CRM.Config;
using WS_CRM.Helper;

//using Microsoft.AspNetCore.Mvc;
//using WS_CRM.Feature.Customer.dao;
//using WS_CRM.Config;
//using WS_CRM.Feature.Customer.dto;
//using WS_CRM.Feature.Customer.Model;
//using WS_CRM.Helper;

namespace WS_CRM.Feature.Catalogue
{
    [ApiController]
    [Route("[controller]")]
    public class CatalogueController : Controller
    {
        IProductRepo _prodRepo;

        public CatalogueController(IProductRepo prodDao)
        {
            _prodRepo = prodDao;
        }

        [HttpPost]
        [Route("CreateProduct")]
        public async Task<IActionResult> CreateProduct(CreateProductParam request)
        {
            var result = new APIResultList<List<ms_product>>();
            try
            {
                if (request != null)
                {
                    await _prodRepo.CreateProduct(request);
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
        [Route("GetProductList")]
        public async Task<IActionResult> GetProductList(GlobalFilter param)
        {
            var result = new APIResultList<List<ms_product>>();
            try
            {
                var data = await _prodRepo.GetAllProduct(param);
                var totalData = await _prodRepo.RepoGetTotalAllProduct(param);
                result.is_ok = true;
                result.message = "Success";
                result.data = data.ToList();
                result.totalRow = totalData;
            }
            catch (Exception ex)
            {
                result.is_ok = false;
                result.message = "Data failed to load, please contact administrator";
            }

            return Ok(result);

        }

        [HttpGet]
        [Route("GetDetailProductbyId")]
        public async Task<IActionResult> GetDetailProductbyId(long id)
        {
            var result = new APIResultList<ms_product>();
            try
            {
                if (id > 0)
                {
                    var data = await _prodRepo.GetProductById(id);
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
        [Route("DeleteProductbyId")]
        public async Task<IActionResult> DeleteProductbyId(long id)
        {
            var result = new APIResultList<ms_product>();
            try
            {
                if (id > 0)
                {
                    await _prodRepo.DeleteProductById(id);
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
        [Route("UpdateProduct")]
        public async Task<IActionResult> UpdateProduct(UpdateProductRequest data)
        {
            var result = new APIResultList<ms_product>();
            try
            {
                if (data != null && data.id > 0)
                {
                    var prod = HelperObj.convert<UpdateProductRequest, ms_product>(data);
                    await _prodRepo.UpdateProduct(prod);
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
    }
}
