using Microsoft.AspNetCore.Mvc;
using MyFITJob.Api.Models;
using MyFITJob.BusinessLogic;

namespace MyFITJob.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JobOffersController(IJobOfferService jobOfferService, ILogger<JobOffersController> logger) : ControllerBase
    {
        public ILogger<JobOffersController> _logger { get; } = logger;

        [HttpGet]
        public async Task<ActionResult<List<JobOfferDto>>> GetAllJobOffersAsync()
        {
            var allJobOffers = await jobOfferService.GetJobOffersAsync(String.Empty);
            return Ok(allJobOffers.Select(JobOfferDto.FromDomain));
        }
    }
}