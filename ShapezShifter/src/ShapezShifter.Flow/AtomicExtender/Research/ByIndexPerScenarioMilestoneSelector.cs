using System;
using System.Collections.Generic;

namespace ShapezShifter.Flow.Research
{
    public class ByIndexPerScenarioMilestoneSelector : IMilestoneSelector
    {
        private readonly Func<string, Index> IndexFunc;

        public ByIndexPerScenarioMilestoneSelector(Func<string, Index> indexFunc)
        {
            IndexFunc = indexFunc;
        }

        public ResearchLevel Select(string scenarioId, IReadOnlyList<ResearchLevel> milestones)
        {
            return milestones[IndexFunc.Invoke(scenarioId)];
        }
    }
}