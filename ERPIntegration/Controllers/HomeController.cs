using ERPIntegration.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ERPIntegration.Controllers
{
    public class HomeController : Controller
    {
        private readonly IErpIntegrationService _erpService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IErpIntegrationService erpService, ILogger<HomeController> logger)
        {
            _erpService = erpService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                string token = await _erpService.GetTokenAsync();
                string customersJson = await _erpService.GetCustomersJsonAsync(token);

                var viewModel = new ERPIntegration.Models.TokenAndCustomerViewModel
                {
                    Token = token,
                    CustomersJson = customersJson
                };

                return View(viewModel);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error fetching customer data.");
                return View("Error", "An error occurred. Please try again later.");
            }
        }

        public async Task<IActionResult> Logout(string token)
        {
            try
            {
                await _erpService.LogoutAsync(token);
                return RedirectToAction("Index");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error logging out.");
                return View("Error", "Logout failed. Please try again.");
            }
        }
    }
}
