using NauticalCatchChallenge.Models.Contracts;
using NauticalCatchChallenge.Utilities.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NauticalCatchChallenge.Models
{
    public abstract class Diver : IDiver
    {
        private string name;
        private int oxygenLevel;
        private List<string> catches;
        private double competitionPoints;
        private bool hasHealthIssues;

        public Diver(string name, int oxygenLevel)
        {
            this.Name = name;
            this.OxygenLevel = oxygenLevel;
            this.CompetitionPoints = 0;
            this.HasHealthIssues = false;
            this.catches = new List<string>();
        }

        public string Name
        {
            get => name;
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException(ExceptionMessages.DiversNameNull);
                }
                name = value;
            }
        }
        public int OxygenLevel
        {
            get => oxygenLevel;
            protected set
            {
                if (value < 0)
                {
                    value = 0;
                }
                oxygenLevel = value;
            }
        }
        public IReadOnlyCollection<string> Catch => catches;
        public double CompetitionPoints
        {
            get => competitionPoints;
            private set
            {
                competitionPoints = Math.Round(value, 1);
            }
        }
        public bool HasHealthIssues
        {
            get => hasHealthIssues;
            private set
            {
                hasHealthIssues = value;
            }
        }

        public void Hit(IFish fish)
        {
            this.OxygenLevel -= fish.TimeToCatch;
            catches.Add(fish.ToString());
            this.CompetitionPoints += fish.Points;
        }

        public abstract void Miss(int TimeToCatch);

        public abstract void RenewOxy();

        public void UpdateHealthStatus()
        {
            if (!HasHealthIssues)
            {
                HasHealthIssues = true;
            }
            else
            {
                hasHealthIssues = false;
            }
        }
        public override string ToString()
        {
            return $"Diver [ Name: {this.Name}, Oxygen left: {this.OxygenLevel}, Fish caught: {this.catches.Count}" +
                $", Points earned: {this.CompetitionPoints} ]";
        }
    }
}
