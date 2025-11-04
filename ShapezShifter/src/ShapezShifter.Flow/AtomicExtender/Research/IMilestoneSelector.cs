using System.Collections.Generic;

namespace ShapezShifter.Flow.Research
{
    public interface IMilestoneSelector
    {
        public ResearchLevel Select(string scenarioId, IReadOnlyList<ResearchLevel> milestones);
    }
}