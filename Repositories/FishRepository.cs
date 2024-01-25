using NauticalCatchChallenge.Models.Contracts;
using NauticalCatchChallenge.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NauticalCatchChallenge.Repositories
{
    public class FishRepository : IRepository<IFish>
    {
        List<IFish> fish;

        public FishRepository()
        {
            this.fish = new List<IFish>();
        }

        public IReadOnlyCollection<IFish> Models => fish;
        public void AddModel(IFish model) => fish.Add(model);
        public IFish GetModel(string name) => fish.FirstOrDefault(x => x.Name == name);
       
    }
}
