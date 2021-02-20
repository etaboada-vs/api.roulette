using Microsoft.Extensions.Caching.Distributed;
using Roulette.Api.Dtos;
using Roulette.Api.Entities;
using Roulette.Api.Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Roulette.Api.Services.Impl
{
    public class RouletteService : IRouletteService
    {
        private readonly IDistributedCache _distributedCache;
        public RouletteService(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }
        public async Task<string> Create()
        {
            RouletteEntity roulette = new RouletteEntity()
            {
                Id = Guid.NewGuid().ToString(),
                IsOpen = false,
                BetsByNumber = new List<BetByNumberEntity>(),
                BetsByColor = new List<BetByColorEntity>()
            };
            await SaveToCache(roulette);

            return roulette.Id;
        }
        public async Task<bool> Open(string idRoulette)
        {
            RouletteEntity roulette = await GetFromCache(idRoulette);
            bool success = false;
            if (!roulette.IsOpen)
            {
                roulette.IsOpen = true;
                await SaveToCache(roulette);
                success = true;
            }

            return success;
        }
        public async Task<bool> PlayByNumber(BetByNumberEntity bet)
        {
            if (bet.Number < 0 || bet.Number > 36)
            {
                throw new Exception("El número a apostar debe estar entre 0 y 36.");
            }
            if (bet.Amount < 1 || bet.Amount > 10000)
            {
                throw new Exception("El monto a apostar debe estar entre 1 y 10,000.");
            }
            RouletteEntity roulette = await GetFromCache(bet.IdRoulette);
            roulette.BetsByNumber.Add(bet);
            await SaveToCache(roulette);

            return true;
        }
        public async Task<bool> PlayByColor(BetByColorEntity bet)
        {
            if (bet.Amount < 1 || bet.Amount > 10000)
            {
                throw new Exception("El monto a apostar debe estar entre 1 y 10,000.");
            }
            RouletteEntity roulette = await GetFromCache(bet.IdRoulette);
            roulette.BetsByColor.Add(bet);
            await SaveToCache(roulette);

            return true;
        }
        public async Task<IEnumerable<WinnerDto>> Close(string idRoulette)
        {
            RouletteEntity roulette = await GetFromCache(idRoulette);
            List<WinnerDto> winners = new List<WinnerDto>();
            if (roulette.IsOpen)
            {
                int numberWinner = new Random().Next(0, 36);
                winners = GetWinners(roulette, numberWinner);
            }
            roulette.IsOpen = false;
            await SaveToCache(roulette);

            return winners;
        }
        private List<WinnerDto> GetWinners(RouletteEntity roulette, int numberWinner)
        {
            List<WinnerDto> winners = new List<WinnerDto>();
            string colorWinner = (numberWinner % 2 == 0) ? "Red" : "Black";
            var winnersByNumber = roulette.BetsByNumber.Where(x => x.Number == numberWinner).ToList();
            var winnersByColor = roulette.BetsByColor.Where(x => x.Color == colorWinner).ToList();
            winnersByNumber.ForEach(x => winners.Add(new WinnerDto
            {
                IdUser = x.IdUser,
                Amount = x.Amount * 5
            }));
            winnersByColor.ForEach(x => winners.Add(new WinnerDto
            {
                IdUser = x.IdUser,
                Amount = x.Amount * 1.8
            }));
            return winners;
        }
        public Task<IEnumerable<RouletteDto>> GetAll()
        {
            throw new NotImplementedException();
        }
        private async Task<RouletteEntity> GetFromCache(string idRoulette)
        {
            var bytes = await _distributedCache.GetAsync(idRoulette);
            if (bytes == null)
            {
                throw new Exception($"Ruleta con id: {idRoulette} no encontrada.");
            }

            return FromByteArray(bytes);
        }
        private async Task SaveToCache(RouletteEntity roulette)
        {
            await _distributedCache.SetAsync(roulette.Id, ToByteArray(roulette));
        }
        private byte[] ToByteArray(RouletteEntity roulette)
        {
            return JsonSerializer.SerializeToUtf8Bytes(roulette);
        }
        private RouletteEntity FromByteArray(byte[] bytes)
        {
            return JsonSerializer.Deserialize<RouletteEntity>(bytes);
        }
    }
}
