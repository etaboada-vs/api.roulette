using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Roulette.Api.Entities
{
    public class BetByNumberEntity : BetEntity
    {
        public int? Number { get; set; }
    }
}
