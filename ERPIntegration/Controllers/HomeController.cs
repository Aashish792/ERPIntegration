using ERPIntegration.Models;
using ERPIntegration.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
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

        // ✅ Fix: Safely retrieve the session value
        public async Task<IActionResult> Index()
        {
            try
            {
                string? token = HttpContext.Session.GetString("Token");

                if (string.IsNullOrEmpty(token))
                {
                    _logger.LogInformation("No token found in session. Fetching a new one.");
                    token = await _erpService.GetTokenAsync();
                    HttpContext.Session.SetString("Token", token);
                }
                else
                {
                    _logger.LogInformation("Using existing session token.");
                }

                string customersJson = await _erpService.GetCustomersJsonAsync(token);
                var viewModel = new TokenAndCustomerViewModel
                {
                    Token = token,
                    CustomersJson = customersJson
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching customer data.");
                return View("Error", new ErrorViewModel { RequestId = HttpContext.TraceIdentifier });
            }
        }

        [HttpPost]
        public IActionResult Logout()
        {
            try
            {
                HttpContext.Session.Clear(); // ✅ Clears session
                _logger.LogInformation("User logged out. Session cleared.");
                return RedirectToAction("LogoutSuccess");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error logging out.");
                return View("Error", new ErrorViewModel { RequestId = HttpContext.TraceIdentifier });
            }
        }

        public IActionResult LogoutSuccess()
        {
            return View();
        }
    }
}
