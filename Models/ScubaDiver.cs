using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NauticalCatchChallenge.Models
{
    public class ScubaDiver : Diver
    {
        private const int oxygenLevel = 540;
        public ScubaDiver(string name) 
            : base(name, oxygenLevel)
        {
        }

        public override void Miss(int TimeToCatch)
        {
            this.OxygenLevel -= (int)Math.Round(TimeToCatch * 0.3, MidpointRounding.AwayFromZero);
        }


        public override void RenewOxy()
        {
            this.OxygenLevel = oxygenLevel;
        }
    }
}
