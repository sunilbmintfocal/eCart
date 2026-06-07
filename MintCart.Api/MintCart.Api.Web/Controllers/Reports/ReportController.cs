using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MintCart.Api.Business.Reports.Interface;
using MintCart.Api.Core.Controllers;

namespace MintCart.Api.Reports.API.Controllers
{
    [Authorize]
    [Route("api/report")]
    public class ReportController : BaseController<ReportController>
    {
        private readonly ILogger<ReportController> _logger;
        private readonly IReportInteractor _reportInteractor;

        public ReportController(ILogger<ReportController> logger, IReportInteractor reportInteractor) : base(logger)
        {
            _logger = logger;
            _reportInteractor = reportInteractor;
        }

        /// <summary>
        /// Returns daily sales report rows, optionally filtered by date range and company.
        /// </summary>
        /// <param name="fromDate">Start date (inclusive). Format: yyyy-MM-dd</param>
        /// <param name="toDate">End date (inclusive). Format: yyyy-MM-dd</param>
        /// <param name="company">Company name to filter by (optional).</param>
        [HttpGet]
        [Route("daily-sales")]
        public async Task<IActionResult> GetDailySalesReport(
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate,
            [FromQuery] string? company)
        {
            var res = await _reportInteractor.GetDailySalesReportAsync(fromDate, toDate, company);
            return await CreateApiResponse(res);
        }
    }
}
