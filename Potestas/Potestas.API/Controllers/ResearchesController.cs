using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Potestas.API.Services;
using Potestas.API.ViewModels;

namespace Potestas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResearchesController : ControllerBase
    {
        private readonly IResearcherService _researcherService;

        public ResearchesController(IResearcherService researcherService)
        {
            _researcherService = researcherService;
        }

        [HttpGet("general/averageEnergy")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAverageEnergy()
        {
            return Ok(await _researcherService.GetAverageEnergyAsync());
        }

        [HttpGet("byDates/averageEnergy")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAverageEnergy([FromQuery] DateTime startFrom, [FromQuery] DateTime endBy)
        {
            //Todo add validation 

            return Ok(await _researcherService.GetAverageEnergyAsync(startFrom, endBy));
        }

        [HttpPost("byCoordinates/averageEnergy")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAverageEnergyAsync([MaxLength(2), FromBody] CoordinatesModel[] coordinates)
        {
            return Ok(await _researcherService.GetAverageEnergyAsync(coordinates[0], coordinates[1]));
        }

        [HttpGet("byCoordinates/distribution")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetDistributionByCoordinatesAsync()
        {
            return Ok(await _researcherService.GetDistributionByCoordinatesAsync());
        }

        [HttpGet("byEnergy/distribution")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetDistributionByEnergyValueAsync()
        {
            return Ok(await _researcherService.GetDistributionByEnergyValueAsync());
        }

        [HttpGet("byTime/distribution")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetDistributionByObservationTimeAsync()
        {
            return Ok(await _researcherService.GetDistributionByObservationTimeAsync());
        }

        [HttpGet("general/maxEnergy")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMaxEnergyAsync()
        {
            return Ok(await _researcherService.GetMaxEnergyAsync());
        }

        [HttpPost("byCoordinates/maxEnergy")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMaxEnergy([FromBody] CoordinatesModel coordinate)
        {
            return Ok(await _researcherService.GetMaxEnergyAsync(coordinate));
        }

        [HttpGet("byDate/maxEnergy")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMaxEnergyAsync([FromQuery] DateTime dateTime)
        {
            return Ok(await _researcherService.GetMaxEnergyAsync(dateTime));
        }

        [HttpGet("general/maxEnergyPosition")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMaxEnergyPositionAsync()
        {
            return Ok(await _researcherService.GetMaxEnergyPositionAsync());
        }

        [HttpGet("general/maxEnergyTime")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMaxEnergyTimeAsync()
        {
            return Ok(await _researcherService.GetMaxEnergyTimeAsync());
        }

        [HttpGet("general/minEnergy")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMinEnergyAsync()
        {
            return Ok(await _researcherService.GetMinEnergyAsync());
        }

        [HttpPost("byCoordinates/minEnergy")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMinEnergyAsync([FromBody] CoordinatesModel coordinate)
        {
            return Ok(await _researcherService.GetMinEnergyAsync(coordinate));
        }

        [HttpGet("byDate/minEnergy")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMinEnergyAsync([FromQuery] DateTime dateTime)
        {
            return Ok(await _researcherService.GetMinEnergyAsync(dateTime));
        }

        [HttpGet("general/minEnergyPosition")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMinEnergyPositionAsync()
        {
            return Ok(await _researcherService.GetMinEnergyPositionAsync());
        }

        [HttpGet("general/minEnergyTime")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMinEnergyTimeAsync()
        {
            return Ok(await _researcherService.GetMinEnergyTimeAsync());
        }
    }
}
