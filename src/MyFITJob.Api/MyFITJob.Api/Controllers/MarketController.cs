using Microsoft.AspNetCore.Mvc;
using MyFITJob.BusinessLogic;
using MyFITJob.BusinessLogic.Services;

namespace MyFITJob.Api.Controllers
{
    [ApiController]
    [Route("api/market")]
    public class MarketController : ControllerBase
    {
        private readonly IJobOfferService _jobOfferService;
        private readonly ISkillExtractorService _skillExtractorService;
        private readonly ILogger<MarketController> _logger;

        public MarketController(
            IJobOfferService jobOfferService,
            ISkillExtractorService skillExtractorService,
            ILogger<MarketController> logger)
        {
            _jobOfferService = jobOfferService;
            _skillExtractorService = skillExtractorService;
            _logger = logger;
        }

        [HttpGet("skills")]
        public async Task<IActionResult> GetMostSoughtSkills([FromQuery] int? top = 5)
        {
            try
            {
                var jobOffers = await _jobOfferService.GetJobOffersAsync(string.Empty);
                
                // Concaténer tous les requirements
                var allRequirements = string.Join(" ", jobOffers.SelectMany(o => o.Requirements));
                
                // Extraire et compter les compétences
                var skillsCount = await _skillExtractorService.ExtractSkillsAsync(allRequirements);

                // Trier et prendre le top N
                var topSkills = skillsCount
                    .OrderByDescending(x => x.Value)
                    .Take(top ?? 5)
                    .Select(x => new
                    {
                        Skill = x.Key,
                        Count = x.Value
                    });

                return Ok(topSkills);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching most sought skills");
                return StatusCode(500, "An error occurred while processing your request");
            }
        }
    }
} 