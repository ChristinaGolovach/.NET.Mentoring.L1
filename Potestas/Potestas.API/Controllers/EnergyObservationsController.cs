using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Potestas.API.Services;
using Potestas.API.ViewModels;
using System.Threading.Tasks;

namespace Potestas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnergyObservationsController : ControllerBase
    {
        private readonly IEnergyObservationService _energyObservationService;

        public EnergyObservationsController(IEnergyObservationService energyObservationService)
        {
            _energyObservationService = energyObservationService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllObservations()
        {
            return Ok(await _energyObservationService.GetAllObservationsAsync());
        }

        [HttpGet("count")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCount()
        {
            return Ok(await _energyObservationService.GetCountAsync());
        }

        [HttpPost("checking/existence")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CheckExistenceAsync([FromBody] EnergyObservationModel energyObservation)
        {
            return Ok(await _energyObservationService.CheckExistenceAsync(energyObservation));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post([FromBody] EnergyObservationModel energyObservation)
        {
            await _energyObservationService.AddObservationAsync(energyObservation);

            return Ok();
        }
        //The[ApiController] attribute makes model validation errors automatically trigger an HTTP 400 response.
        // !ModelState.IsValid is unnecessary in an action method


        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteObservationAsync([FromBody] EnergyObservationModel flashObservation)
        {
            await _energyObservationService.DeleteObservationAsync(flashObservation);

            return NoContent(); // ask: or return Ok();
        }

        [HttpDelete("clearing")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ClearObservationsAsync()
        {
            await _energyObservationService.ClearObservationsAsync();

            return NoContent(); // ask: or return Ok();
        }
    }

    //TODO
    //https://code-maze.com/global-error-handling-aspnetcore/#builtinmiddleware
    //https://weblog.west-wind.com/posts/2016/oct/16/error-handling-and-exceptionfilter-dependency-injection-for-aspnet-core-apis
}
