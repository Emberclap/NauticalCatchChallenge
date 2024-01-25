using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NauticalCatchChallenge.Models
{
    internal class DeepSeaFish : Fish
    {
        public const int timeToCatch = 180;

        public DeepSeaFish(string name, double points) 
            : base(name, points, timeToCatch)
        {
        }
    }
}
