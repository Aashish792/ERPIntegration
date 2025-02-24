using ERPIntegration.Models;
using ERPIntegration.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ERPIntegration.Controllers
{
    public class HomeController : Controller
    {
        private readonly IErpIntegrationService _erpService;

        public HomeController(IErpIntegrationService erpService)
        {
            _erpService = erpService;
        }

        // Home page: Retrieve token, then use that token to fetch the customer JSON.
        public async Task<IActionResult> Index()
        {
            try
            {
                string token = await _erpService.GetTokenAsync();
                string customersJson = await _erpService.GetCustomersJsonAsync(token);

                var viewModel = new TokenAndCustomerViewModel
                {
                    Token = token,
                    CustomersJson = customersJson
                };

                return View(viewModel);
            }
            catch (System.Exception ex)
            {
                return Content("Error: " + ex.Message);
            }
        }
    }
}
