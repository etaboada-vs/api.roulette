using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Roulette.Api.Entities
{
    public class BetEntity
    {
        public string IdUser { get; set; }
        public string IdRoulette { get; set; }
        public double Amount { get; set; }
    }
}
