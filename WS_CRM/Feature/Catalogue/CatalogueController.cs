using Microsoft.AspNetCore.Mvc;

namespace WS_CRM.Feature.Catalogue
{
    public class CatalogueController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
