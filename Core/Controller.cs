using NauticalCatchChallenge.Core.Contracts;
using NauticalCatchChallenge.Models;
using NauticalCatchChallenge.Models.Contracts;
using NauticalCatchChallenge.Repositories;
using NauticalCatchChallenge.Repositories.Contracts;
using NauticalCatchChallenge.Utilities.Messages;
using System.Globalization;
using System.Text;

namespace NauticalCatchChallenge.Core
{
    public class Controller : IController
    {
        private IRepository<IFish> fishRepository;
        private IRepository<IDiver> divers;

        public Controller()
        {
            this.fishRepository = new FishRepository();
            this.divers = new DiverRepository();
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
        }

        public string ChaseFish(string diverName, string fishName, bool isLucky)
        {
            IDiver diver = divers.GetModel(diverName);
            IFish fish = fishRepository.GetModel(fishName);
            if (diver == null)
            {
                return string.Format(OutputMessages.DiverNotFound, divers.GetType().Name, diverName);
            }
            if (fish == null)
            {
                return string.Format(OutputMessages.FishNotAllowed, fishName);
            }
            if (diver.HasHealthIssues)
            {
                return string.Format(OutputMessages.DiverHealthCheck, diverName);
            }
            if (diver.OxygenLevel < fish.TimeToCatch)
            {
                diver.Miss(fish.TimeToCatch);
                OxigenLevelCheck(diver);
                return string.Format(OutputMessages.DiverMisses, diverName, fishName);
            }
            else if (diver.OxygenLevel == fish.TimeToCatch)
            {
                if (!isLucky)
                {
                    diver.Miss(fish.TimeToCatch);
                    OxigenLevelCheck(diver);
                    return string.Format(OutputMessages.DiverMisses, diverName , fishName);
                }
                else
                {
                    diver.Hit(fish);
                    OxigenLevelCheck(diver);
                    return string.Format(OutputMessages.DiverHitsFish, diverName, fish.Points, fishName);
                }
            }
            else
            {
                diver.Hit(fish);
                OxigenLevelCheck(diver);
                return string.Format(OutputMessages.DiverHitsFish, diverName, fish.Points, fishName);
            }
           
        }

        public string CompetitionStatistics()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("**Nautical-Catch-Challenge**");
            foreach (var diver in divers.Models
                .OrderByDescending(p=>p.CompetitionPoints)
                .ThenByDescending(c=>c.Catch.Count)
                .ThenBy(n=>n.Name)
                .Where(h => h.HasHealthIssues == false))
            {
                sb.AppendLine(diver.ToString());
            }
            return sb.ToString().TrimEnd();
        }

        public string DiveIntoCompetition(string diverType, string diverName)
        {
            IDiver diver;
            if (diverType == nameof(ScubaDiver))
            {
                diver = new ScubaDiver(diverName);
            }
            else if (diverType == nameof(FreeDiver))
            {
                diver = new FreeDiver(diverName);
            }
            else
            {
                return string.Format(OutputMessages.DiverTypeNotPresented, diverType);
            }
            if (divers.GetModel(diverName) != null)
            {
                return string.Format(OutputMessages.DiverNameDuplication, diverName, divers.GetType().Name);
            }
            divers.AddModel(diver);
            return string.Format(OutputMessages.DiverRegistered, diverName, divers.GetType().Name);

        }

        public string DiverCatchReport(string diverName)
        {
            IDiver diver = divers.GetModel(diverName);
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(diver.ToString());
            sb.AppendLine("Catch Report:");
            foreach (var fish in diver.Catch)
            {
                sb.AppendLine(fish.ToString());
            }
            return sb.ToString().TrimEnd();
        }

        public string HealthRecovery()
        {
            int counter = 0;
            foreach (var diver in divers.Models.Where(x=>x.HasHealthIssues == true))
            {
                diver.UpdateHealthStatus();
                diver.RenewOxy();
                counter++;
            }
            return string.Format(OutputMessages.DiversRecovered, counter);
        }

        public string SwimIntoCompetition(string fishType, string fishName, double points)
        {
            IFish fish;
            if (fishType == nameof(ReefFish))
            {
                fish = new ReefFish(fishName, points);
            }
            else if (fishType == nameof(DeepSeaFish))
            {
                fish = new DeepSeaFish(fishName, points);
            }
            else if (fishType == nameof(PredatoryFish))
            {
                fish = new PredatoryFish(fishName, points);
            }
            else
            {
                return string.Format(OutputMessages.FishTypeNotPresented, fishType);
            }
            if (this.fishRepository.GetModel(fishName) != null)
            {
                return string.Format(OutputMessages.FishNameDuplication, fishName, fishRepository.GetType().Name);
            }
            fishRepository.AddModel(fish);
            return string.Format(OutputMessages.FishCreated, fishName);
        }

        public void OxigenLevelCheck(IDiver diver)
        {
            if (diver.OxygenLevel <= 0)
            {
                diver.UpdateHealthStatus();
            }
        }
    }
}