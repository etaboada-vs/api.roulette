using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Roulette.Api.Dtos;
using Roulette.Api.Entities;
using Roulette.Api.Services.Contract;

namespace Roulette.Api.Controllers
{
    [Route("api/Roulette")]
    [ApiController]
    public class RouletteController : ControllerBase
    {
        private readonly IRouletteService _rouletteService;
        public RouletteController(IRouletteService rouletteService)
        {
            _rouletteService = rouletteService;
        }
        [Produces("application/json")]
        [Route("Create")]
        [HttpPost]
        public async Task<IActionResult> Create()
        {
            try
            {
                string idRoulette = await _rouletteService.Create();

                return Ok(idRoulette);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [Produces("application/json")]
        [Route("Open")]
        [HttpPost]
        public async Task<IActionResult> Open([FromBody] string idRoulette)
        {
            try
            {
                bool success = await _rouletteService.Open(idRoulette);

                return Ok(success);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [Produces("application/json")]
        [Route("PlayByNumber")]
        [HttpPost]
        public async Task<IActionResult> PlayByNumber([FromHeader] string idUser, [FromBody] BetByNumberEntity bet)
        {
            try
            {
                bet.IdUser = idUser;
                bool success = await _rouletteService.PlayByNumber(bet);

                return Ok(success);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [Produces("application/json")]
        [Route("PlayByColor")]
        [HttpPost]
        public async Task<IActionResult> PlayByColor([FromHeader] string idUser, [FromBody] BetByColorEntity bet)
        {
            try
            {
                bet.IdUser = idUser;
                bool success = await _rouletteService.PlayByColor(bet);

                return Ok(success);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [Produces("application/json")]
        [Route("Close")]
        [HttpPost]
        public async Task<IActionResult> Close([FromBody] string idRoulette)
        {
            try
            {
                IEnumerable<WinnerDto> winners = await _rouletteService.Close(idRoulette);

                return Ok(winners);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [Produces("application/json")]
        [Route("GetAll")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                IEnumerable<RouletteDto> roulettes = await _rouletteService.GetAll();

                return Ok(roulettes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
