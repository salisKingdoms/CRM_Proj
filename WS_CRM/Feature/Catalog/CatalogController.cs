using Microsoft.AspNetCore.Mvc;
using WS_CRM.Feature.Catalog.dao;
using WS_CRM.Feature.Catalog.dto;
using WS_CRM.Feature.Catalog.Model;
using WS_CRM.Config;
using WS_CRM.Helper;

//using Microsoft.AspNetCore.Mvc;
//using WS_CRM.Feature.Customer.dao;
//using WS_CRM.Config;
//using WS_CRM.Feature.Customer.dto;
//using WS_CRM.Feature.Customer.Model;
//using WS_CRM.Helper;

namespace WS_CRM.Feature.Catalog
{
    [ApiController]
    [Route("[controller]")]
    public class CatalogController : Controller
    {
        IProductRepo _prodRepo;

        public CatalogController(IProductRepo prodDao)
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

        [HttpPost]
        [Route("CreateSparepart")]
        public async Task<IActionResult> CreateSparepart(CreateSparepartParam request)
        {
            var result = new APIResultList<List<ms_sparepart>>();
            try
            {
                if (request != null)
                {
                    await _prodRepo.CreateSparepart(request);
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
        [Route("GetSparepartList")]
        public async Task<IActionResult> GetSparepartList(GlobalFilter param)
        {
            var result = new APIResultList<List<ms_sparepart>>();
            try
            {
                var data = await _prodRepo.GetAllSparepart(param);
                var totalData = await _prodRepo.RepoGetTotalAllSparepart(param);
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
        [Route("GetDetailSparepartbyId")]
        public async Task<IActionResult> GetDetailSparepartbyId(long id)
        {
            var result = new APIResultList<ms_sparepart>();
            try
            {
                if (id > 0)
                {
                    var data = await _prodRepo.GetSparepartById(id);
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
        [Route("DeleteSparepartbyId")]
        public async Task<IActionResult> DeleteSparepartbyId(long id)
        {
            var result = new APIResultList<ms_sparepart>();
            try
            {
                if (id > 0)
                {
                    await _prodRepo.DeleteSparepartById(id);
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
        [Route("UpdateSparepart")]
        public async Task<IActionResult> UpdateSparepart(UpdateSparepartRequest data)
        {
            var result = new APIResultList<ms_sparepart>();
            try
            {
                if (data != null && data.id > 0)
                {
                    var up = HelperObj.convert<UpdateSparepartRequest, ms_sparepart>(data);
                    await _prodRepo.UpdateSparepart(up);
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
