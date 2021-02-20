using Roulette.Api.Dtos;
using Roulette.Api.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Roulette.Api.Services.Contract
{
    public interface IRouletteService
    {
        Task<string> Create();
        Task<bool> Open(string idRoulette);
        Task<bool> PlayByNumber(BetByNumberEntity bet);
        Task<bool> PlayByColor(BetByColorEntity bet);
        Task<IEnumerable<WinnerDto>> Close(string idRoulette);
        Task<IEnumerable<RouletteDto>> GetAll();
    }
}
