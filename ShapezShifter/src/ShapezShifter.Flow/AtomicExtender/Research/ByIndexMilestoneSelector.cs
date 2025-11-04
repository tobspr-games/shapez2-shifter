using System;
using System.Collections.Generic;

namespace ShapezShifter.Flow.Research
{
    public class ByIndexMilestoneSelector : IMilestoneSelector
    {
        private readonly Index Index;

        public ByIndexMilestoneSelector(Index index)
        {
            Index = index;
        }

        public ResearchLevel Select(string scenarioId, IReadOnlyList<ResearchLevel> milestones)
        {
            return milestones[Index];
        }
    }
}