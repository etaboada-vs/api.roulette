using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Roulette.Api.Entities
{
    public class RouletteEntity
    {
        public string Id { get; set; }
        public bool IsOpen { get; set; }
        public IList<BetByColorEntity> BetsByColor { get; set; }
        public IList<BetByNumberEntity> BetsByNumber { get; set; }
    }
}
