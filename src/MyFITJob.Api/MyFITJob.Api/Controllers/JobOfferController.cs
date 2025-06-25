using Microsoft.AspNetCore.Mvc;
using MyFITJob.Api.Models;
using MyFITJob.BusinessLogic;
using BusinessLogicDTOs = MyFITJob.BusinessLogic.DTOs;

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
            return Ok(allJobOffers.Select(BusinessLogicDTOs.JobOfferDto.FromDomain));
        }

        [HttpPost]
        public async Task<ActionResult<BusinessLogicDTOs.JobOfferDto>> CreateJobOfferAsync([FromBody] BusinessLogicDTOs.CreateJobOfferDto createJobOfferDto)
        {
            try
            {
                _logger.LogInformation("Réception d'une demande de création d'offre d'emploi: {Title}", createJobOfferDto.Title);

                var createdJobOffer = await jobOfferService.CreateJobOfferAsync(createJobOfferDto);
                var jobOfferDto = BusinessLogicDTOs.JobOfferDto.FromDomain(createdJobOffer);

                _logger.LogInformation("Offre d'emploi créée avec succès. ID: {JobOfferId}", createdJobOffer.Id);

                return Ok(jobOfferDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la création de l'offre d'emploi: {Title}", createJobOfferDto.Title);
                return StatusCode(500, new { message = "Une erreur interne s'est produite lors de la création de l'offre d'emploi." });
            }
        }
    }
}