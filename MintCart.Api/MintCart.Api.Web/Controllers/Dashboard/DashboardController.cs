using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using MintCart.Api.Dashboard.Business.Interface;
using MintCart.Api.Core.Controllers;
using Microsoft.AspNetCore.Authorization;

namespace MintCart.Api.Dashboard.API.Controllers
{
    [Route("api/dashboard")]
    public class DashboardController : BaseController<DashboardController>
    {
        private readonly ILogger<DashboardController> _logger;
        private readonly IDashboardInteractor _dashboardInteractor;

        public DashboardController(
            ILogger<DashboardController> logger,
            IDashboardInteractor dashboardInteractor) : base(logger)
        {
            _logger = logger;
            _dashboardInteractor = dashboardInteractor;
        }

        /// <summary>
        /// Returns all dashboard metrics in a single call:
        /// KPIs, payables summary, and recent activities.
        /// </summary>
        [HttpGet]
        [Route("metrics")]
        public async Task<IActionResult> GetDashboardMetrics()
        {
            var res = await _dashboardInteractor.GetDashboardMetricsAsync();
            return await CreateApiResponse(res);
        }

        /// <summary>
        /// Returns only the KPI cards (Total Sales, Low Stock, Complaints, Balance).
        /// </summary>
        [HttpGet]
        [Route("kpis")]
        public async Task<IActionResult> GetKpis()
        {
            var res = await _dashboardInteractor.GetKpisAsync();
            return await CreateApiResponse(res);
        }

        /// <summary>
        /// Returns only the company payables summary.
        /// </summary>
        [HttpGet]
        [Route("payables")]
        public async Task<IActionResult> GetPayables()
        {
            var res = await _dashboardInteractor.GetPayablesAsync();
            return await CreateApiResponse(res);
        }

        /// <summary>
        /// Returns recent activities (Sales, Complaints, Purchases) unified feed.
        /// </summary>
        /// <param name="top">Maximum number of records to return (default: 10).</param>
        [HttpGet]
        [Route("activities")]
        public async Task<IActionResult> GetRecentActivities([FromQuery] int top = 10)
        {
            var res = await _dashboardInteractor.GetRecentActivitiesAsync(top);
            return await CreateApiResponse(res);
        }
    }
}
