using System;
using System.Collections.Generic;
using System.Linq;
using Game.Core.Research;

namespace ShapezShifter.Flow.Research
{
    public class ByIdMilestoneSelector : IMilestoneSelector
    {
        private readonly ResearchUpgradeId MilestoneId;

        public ByIdMilestoneSelector(ResearchUpgradeId milestoneId)
        {
            MilestoneId = milestoneId;
        }

        public ResearchLevel Select(string scenarioId, IReadOnlyList<ResearchLevel> milestones)
        {
            try
            {
                ResearchLevel milestone = milestones.SingleOrDefault(x => x.Id == MilestoneId);
                return milestone ?? throw new Exception($"Could not find a milestone with id {MilestoneId}");
            }
            catch (InvalidOperationException)
            {
                throw new Exception($"More than one element match milestone id {MilestoneId}");
            }
        }
    }
}